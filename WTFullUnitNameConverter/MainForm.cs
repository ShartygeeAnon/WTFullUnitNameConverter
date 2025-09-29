using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CsvHelper;
using CsvHelper.Configuration;

namespace WTFullUnitNameConverter
{
    public partial class MainForm : Form
    {
        string[] arguments = Environment.GetCommandLineArgs();

        public MainForm()
        {
            InitializeComponent();
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = "Select the \"units.csv\" file in the \"lang\" folder in your War Thunder directory";
            openFileDialog1.Filter = ".csv files (*.csv)|*.csv";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "units.csv";
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            textBox1.Text = openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var filename = openFileDialog1.FileName;
            bool silent = arguments.Contains("-silent");

            if (string.IsNullOrEmpty(filename) || (!filename.Contains("units") && !filename.EndsWith(".csv", StringComparison.OrdinalIgnoreCase)) || !File.Exists(filename))
            {
                if (!silent)
                    MessageBox.Show("No or incorrect file selected!", "Error!");
                return;
            }

            try
            {
                int changes = ProcessFile(filename);
                if (!silent)
                {
                    if (changes > 0)
                        MessageBox.Show($"Conversion complete — {changes} text cell changes applied. Backup created as {Path.GetFileName(filename)}.bak", "Message");
                    else
                        MessageBox.Show("Processed file, no changes required.", "Message");
                }
            }
            catch (IOException)
            {
                if (!silent)
                    MessageBox.Show("File is in use!", "Error!");
            }
            catch (Exception ex)
            {
                if (!silent)
                    MessageBox.Show($"Error while processing file:\n{ex.Message}", "Error!");
            }
        }

        private int ProcessFile(string path)
        {
            // Read CSV dynamically into a list of dictionaries preserving headers & order
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                BadDataFound = null,
                IgnoreBlankLines = false
            };

            string[] header;
            var rows = new List<Dictionary<string, string>>();

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, config))
            {
                if (!csv.Read())
                    throw new InvalidOperationException("CSV appears empty.");

                csv.ReadHeader();
                header = csv.HeaderRecord;
                while (csv.Read())
                {
                    var record = new Dictionary<string, string>(StringComparer.Ordinal);
                    foreach (var h in header)
                    {
                        // Use GetField by header name to preserve empty strings rather than nulls
                        string val = "";
                        try { val = csv.GetField(h) ?? ""; } catch { val = ""; }
                        record[h] = val;
                    }
                    rows.Add(record);
                }
            }

            if (header == null || header.Length == 0)
                throw new InvalidOperationException("CSV header could not be read.");

            string idCol = header[0]; // first column is the ID column, per your CSV

            // Build groups: base -> (variant -> index)
            var groups = new Dictionary<string, Dictionary<string, int>>(StringComparer.Ordinal);
            for (int i = 0; i < rows.Count; i++)
            {
                var name = rows[i].ContainsKey(idCol) ? rows[i][idCol] ?? "" : "";
                var (baseId, variant) = SplitVariant(name);

                if (!groups.ContainsKey(baseId))
                    groups[baseId] = new Dictionary<string, int>(StringComparer.Ordinal);

                // If duplicate variants appear, keep the first occurrence (preserve order)
                if (!groups[baseId].ContainsKey(variant))
                    groups[baseId][variant] = i;
            }

            int changes = 0;
            // All columns except the ID column are considered "textual" and will be copied
            var copyCols = header.Where(h => !string.Equals(h, idCol, StringComparison.Ordinal)).ToArray();

            // Copy helper
            void CopyFromTo(int srcIdx, int dstIdx)
            {
                var src = rows[srcIdx];
                var dst = rows[dstIdx];
                foreach (var col in copyCols)
                {
                    var s = src.ContainsKey(col) ? src[col] : "";
                    var d = dst.ContainsKey(col) ? dst[col] : "";
                    if (!string.Equals(s, d, StringComparison.Ordinal))
                    {
                        dst[col] = s;
                        changes++;
                    }
                }
            }

            // Apply rules for every group
            foreach (var kv in groups)
            {
                var baseId = kv.Key;
                var variants = kv.Value;
                bool has0 = variants.ContainsKey("_0");
                bool has1 = variants.ContainsKey("_1");
                bool has2 = variants.ContainsKey("_2");
                bool hasshop = variants.ContainsKey("_shop");
                bool hasplain = variants.ContainsKey("");

                // Rule 1:
                // Case A: _0, _1, _2 present & no _shop & no plain -> copy from _1 -> _2 (leave _0 as is)
                if (has0 && has1 && has2 && (!hasshop) && (!hasplain))
                {
                    CopyFromTo(variants["_1"], variants["_2"]);
                    continue;
                }

                // Rule 2:
                // Case B: plain + _0, _1, _2 present & no _shop -> copy plain -> _1 and _2 (leave _0 as is)
                if (hasplain && has0 && has1 && has2 && (!hasshop))
                {
                    CopyFromTo(variants[""], variants["_1"]);
                    CopyFromTo(variants[""], variants["_2"]);
                    continue;
                }

                // Rule 3: shop present -> shop replaces _1/_2 (if they exist) and plain (if it exists). Leave _0 alone.
                if (hasshop)
                {
                    int shopIdx = variants["_shop"];
                    if (has1) CopyFromTo(shopIdx, variants["_1"]);
                    if (has2) CopyFromTo(shopIdx, variants["_2"]);
                    if (hasplain) CopyFromTo(shopIdx, variants[""]);
                    continue;
                }

                // Rule 4: fallback: plain exists and _1/_2 exist but no _0 and no _shop -> copy plain -> _1/_2
                if (hasplain && (!has0) && (!hasshop) && (has1 || has2))
                {
                    if (has1) CopyFromTo(variants[""], variants["_1"]);
                    if (has2) CopyFromTo(variants[""], variants["_2"]);
                    continue;
                }

                // Other combinations: leave untouched (safer)
            }

            // If any changes were made, make a backup and write the file
            if (changes > 0)
            {
                // Make backup
                string bak = path + ".bak";
                File.Copy(path, bak, true);

                // Write CSV back using same header order and ';' delimiter
                using (var writer = new StreamWriter(path))
                using (var csvWriter = new CsvWriter(writer, config))
                {
                    // Write header
                    foreach (var h in header)
                        csvWriter.WriteField(h);
                    csvWriter.NextRecord();

                    // Write rows in original order
                    foreach (var row in rows)
                    {
                        foreach (var h in header)
                        {
                            string val = row.ContainsKey(h) ? row[h] : "";
                            csvWriter.WriteField(val);
                        }
                        csvWriter.NextRecord();
                    }
                    csvWriter.Flush();
                }
            }

            return changes;
        }

        /// <summary>
        /// Split an id into (base, variant). Variant is one of: "_0", "_1", "_2", "_shop", or "" for plain.
        /// If it doesn't match any pattern, variant will be "" (treated as plain base)
        /// </summary>
        private (string baseId, string variant) SplitVariant(string id)
        {
            if (string.IsNullOrEmpty(id))
                return (id ?? "", "");

            if (id.EndsWith("_0", StringComparison.Ordinal))
                return (id.Substring(0, id.Length - 2), "_0");
            if (id.EndsWith("_1", StringComparison.Ordinal))
                return (id.Substring(0, id.Length - 2), "_1");
            if (id.EndsWith("_2", StringComparison.Ordinal))
                return (id.Substring(0, id.Length - 2), "_2");
            if (id.EndsWith("_shop", StringComparison.Ordinal))
                return (id.Substring(0, id.Length - 5), "_shop");
            // plain
            return (id, "");
        }

        public void MainForm_Load(object sender, EventArgs e)
        {
            // CLI behaviour: same semantics as original
            if (arguments.Length > 1)
            {
                // If only -silent supplied, do nothing and return
                if (arguments.Length == 2 && arguments.Contains("-silent"))
                {
                    return;
                }

                // Otherwise the first non-exe arg is treated as file path
                openFileDialog1.FileName = arguments[1];
                textBox1.Text = openFileDialog1.FileName;

                bool silent = arguments.Contains("-silent");

                if (string.IsNullOrEmpty(openFileDialog1.FileName) || (!openFileDialog1.FileName.Contains("units") && !openFileDialog1.FileName.Contains(".csv")) || !File.Exists(openFileDialog1.FileName))
                {
                    if (!silent)
                        MessageBox.Show("No or incorrect file selected!", "Error!");
                    else
                        Application.Exit();
                }
                else
                {
                    try
                    {
                        // Process once and exit (original behaviour)
                        button2.PerformClick();
                        Application.Exit();
                    }
                    catch
                    {
                        if (!silent)
                            MessageBox.Show("File is in use!", "Error!");
                        Application.Exit();
                    }
                }
            }
        }
    }
}
