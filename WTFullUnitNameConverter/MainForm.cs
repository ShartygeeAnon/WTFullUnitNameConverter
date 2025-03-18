using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using CsvHelper;
using System.IO;
using System.Globalization;
using CsvHelper.Configuration;
using System.Collections.Generic;

namespace WTFullUnitNameConverter
{
    public partial class MainForm : Form
    {
        public class InternalStructure
        {
            //[Name("<ID|readonly|noverify>")]
            public string Name { get; set; }
            //[Name("<English>")]
            public string English { get; set; }
            //[Name("<French>")]
            public string French { get; set; }
            //[Name("<Italian>")]
            public string Italian { get; set; }
            //[Name("<German>")]
            public string German { get; set; }
            //[Name("<Spanish>")]
            public string Spanish { get; set; }
            //[Name("<Russian>")]
            public string Russian { get; set; }
            //[Name("<Polish>")]
            public string Polish { get; set; }
            //[Name("<Czech>")]
            public string Czech { get; set; }
            //[Name("<Turkish>")]
            public string Turkish { get; set; }
            //[Name("<Chinese>")]
            public string Chinese { get; set; }
            //[Name("<Japanese>")]
            public string Japanese { get; set; }
            //[Name("<Portuguese>")]
            public string Portuguese { get; set; }
            //[Name("<Vietnamese>")]
            //public string Vietnamese { get; set; }
            //[Name("<Ukrainian>")]
            public string Ukrainian { get; set; }
            //[Name("<Serbian>")]
            public string Serbian { get; set; }
            //[Name("<Hungarian>")]
            public string Hungarian { get; set; }
            //[Name("<Korean>")]
            public string Korean { get; set; }
            //[Name("<Belarusian>")]
            public string Belarusian { get; set; }
            //[Name("<Romanian>")]
            public string Romanian { get; set; }
            //[Name("<TChinese>")]
            public string TChinese { get; set; }
            //[Name("<HChinese>")]
            public string HChinese { get; set; }
            //[Name("<Comments>")]
            public string Comments { get; set; }
            //[Name("<max_chars>")]
            public string max_chars { get; set; }
            //[Name("SORT-NATION")]
            public string SORT_NATION { get; set; }
            //[Name("SORT-VEH")]
            public string SORT_VEH { get; set; }
            //[Name("SORT-ORDER")]
            public string SORT_ORDER { get; set; }
            //[Name("ORIG.ORD")]
            public string ORIG_ORD { get; set; }
        }

        public class InternalStructureMap : ClassMap<InternalStructure>
        {
            public InternalStructureMap()
            {
                Map(m => m.Name).Index(0).Name("<ID|readonly|noverify>");
                Map(m => m.English).Index(1).Name("<English>");
                Map(m => m.French).Index(2).Name("<French>");
                Map(m => m.Italian).Index(3).Name("<Italian>");
                Map(m => m.German).Index(4).Name("<German>");
                Map(m => m.Spanish).Index(5).Name("<Spanish>");
                Map(m => m.Russian).Index(6).Name("<Russian>");
                Map(m => m.Polish).Index(7).Name("<Polish>");
                Map(m => m.Czech).Index(8).Name("<Czech>");
                Map(m => m.Turkish).Index(9).Name("<Turkish>");
                Map(m => m.Chinese).Index(10).Name("<Chinese>");
                Map(m => m.Japanese).Index(11).Name("<Japanese>");
                Map(m => m.Portuguese).Index(12).Name("<Portuguese>");
                //Map(m => m.Vietnamese).Index(13).Name("<Vietnamese>");
                Map(m => m.Ukrainian).Index(13).Name("<Ukrainian>");
                Map(m => m.Serbian).Index(14).Name("<Serbian>");
                Map(m => m.Hungarian).Index(15).Name("<Hungarian>");
                Map(m => m.Korean).Index(16).Name("<Korean>");
                Map(m => m.Belarusian).Index(17).Name("<Belarusian>");
                Map(m => m.Romanian).Index(18).Name("<Romanian>");
                Map(m => m.TChinese).Index(19).Name("<TChinese>");
                Map(m => m.HChinese).Index(20).Name("<HChinese>");
                Map(m => m.Comments).Index(21).Name("<Comments>");
                Map(m => m.max_chars).Index(22).Name("<max_chars>");
                //Map(m => m.SORT_NATION).Index(23).Name("SORT-NATION");
                //Map(m => m.SORT_VEH).Index(24).Name("SORT-VEH");
                //Map(m => m.SORT_ORDER).Index(25).Name("SORT-ORDER");
                //Map(m => m.ORIG_ORD).Index(26).Name("ORIG.ORD");
            }
        }

        public class InternalStructureMapIFN1 : ClassMap<InternalStructure>
        {
            public InternalStructureMapIFN1()
            {
                Map(m => m.Name).Index(0).Name("<ID|readonly|noverify>");
                Map(m => m.English).Index(1).Name("<English>");
                //Map(m => m.French).Index(2).Name("<French>");
                //Map(m => m.Italian).Index(3).Name("<Italian>");
                //Map(m => m.German).Index(4).Name("<German>");
                //Map(m => m.Spanish).Index(5).Name("<Spanish>");
                //Map(m => m.Russian).Index(6).Name("<Russian>");
                //Map(m => m.Polish).Index(7).Name("<Polish>");
                //Map(m => m.Czech).Index(8).Name("<Czech>");
                //Map(m => m.Turkish).Index(9).Name("<Turkish>");
                //Map(m => m.Chinese).Index(10).Name("<Chinese>");
                //Map(m => m.Japanese).Index(11).Name("<Japanese>");
                //Map(m => m.Portuguese).Index(12).Name("<Portuguese>");
                //Map(m => m.Vietnamese).Index(13).Name("<Vietnamese>");
                //Map(m => m.Ukrainian).Index(13).Name("<Ukrainian>");
                //Map(m => m.Serbian).Index(14).Name("<Serbian>");
                //Map(m => m.Hungarian).Index(15).Name("<Hungarian>");
                //Map(m => m.Korean).Index(16).Name("<Korean>");
                //Map(m => m.Belarusian).Index(17).Name("<Belarusian>");
                //Map(m => m.Romanian).Index(18).Name("<Romanian>");
                //Map(m => m.TChinese).Index(19).Name("<TChinese>");
                //Map(m => m.HChinese).Index(20).Name("<HChinese>");
                //Map(m => m.Comments).Index(21).Name("<Comments>");
                //Map(m => m.max_chars).Index(22).Name("<max_chars>");
                //Map(m => m.SORT_NATION).Index(23).Name("SORT-NATION");
                //Map(m => m.SORT_VEH).Index(24).Name("SORT-VEH");
                //Map(m => m.SORT_ORDER).Index(25).Name("SORT-ORDER");
                //Map(m => m.ORIG_ORD).Index(26).Name("ORIG.ORD");
            }
        }

        public MainForm()
        {
            InitializeComponent();
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = "Select the \"units.csv\" file in the \"lang\" folder in your War Thunder directory";
            openFileDialog1.Filter = ".csv files (*.csv)|*.csv";
        }

        string[] arguments = Environment.GetCommandLineArgs();

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
            if (openFileDialog1.FileName == null || (!openFileDialog1.FileName.Contains("units") && !openFileDialog1.FileName.Contains(".csv")) || !File.Exists(openFileDialog1.FileName))
            {
                if (!arguments.Contains("-silent"))
                    MessageBox.Show("No or incorrect file selected!", "Error!");
            }
            else
            {
                try
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        Delimiter = ";",
                    };
                    var records = new List<InternalStructure>();
                    var recordsListFull = new List<InternalStructure>();
                    var indexListFull = new List<int>();
                    bool ifn1 = false;
                    using (var reader = new StreamReader(openFileDialog1.FileName))
                    using (var csvReader = new CsvReader(reader, config))
                    {
                        string headerLine = reader.ReadLine();
                        if (openFileDialog1.FileName.Contains("IFN1"))
                        {
                            csvReader.Context.RegisterClassMap<InternalStructureMapIFN1>();
                            ifn1 = true;
                        }
                        else
                            csvReader.Context.RegisterClassMap<InternalStructureMap>();
                        reader.BaseStream.Position = 0;
                        reader.DiscardBufferedData();
                        records = csvReader.GetRecords<InternalStructure>().ToList();
                        int j = 0;
                        for (int i = 0; i < records.Count; i++)
                        {
                            j++;
                            if (records.ElementAt(i).Name.EndsWith("_0") && records.ElementAt(i + 1).Name.EndsWith("_1") && records.ElementAt(i + 2).Name.EndsWith("_2"))
                            {
                                try
                                {
                                    if (i != 0 && records.ElementAt(i - 1).Name.EndsWith("_shop"))
                                    {
                                        recordsListFull.Add(records.ElementAt(i - 1));
                                        indexListFull.Add(j);
                                    }

                                    else
                                    {
                                        recordsListFull.Add(records.ElementAt(i));
                                        indexListFull.Add(j + 1);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    using (var writer = new StreamWriter(openFileDialog1.FileName))
                    using (var csvWriter = new CsvWriter(writer, config))
                    {
                        if (ifn1 == true)
                            csvWriter.Context.RegisterClassMap<InternalStructureMapIFN1>();
                        else
                            csvWriter.Context.RegisterClassMap<InternalStructureMap>();
                        csvWriter.WriteHeader<InternalStructure>();
                        csvWriter.NextRecord();
                        int i = 0;
                        try
                        {
                            while (i < records.Count)
                            {
                                if (csvWriter.Row == indexListFull[i])
                                {
                                    if (recordsListFull[i].Name.EndsWith("_shop"))
                                    {
                                        InternalStructure recordShop = new InternalStructure();
                                        recordShop = records[indexListFull[i] - 2];
                                        csvWriter.WriteRecord(recordShop);
                                        csvWriter.NextRecord();
                                        InternalStructure record0 = new InternalStructure();
                                        record0 = records[indexListFull[i] - 1];
                                        record0.Name = record0.Name.Remove(record0.Name.Length - 1, 1);
                                        record0.Name = record0.Name.Insert(record0.Name.Length, "0");
                                        csvWriter.WriteRecord(record0);
                                        csvWriter.NextRecord();
                                        InternalStructure record1 = new InternalStructure();
                                        record1 = recordsListFull[i];
                                        record1.Name = record1.Name.Remove(record1.Name.Length - 4, 4);
                                        record1.Name = record1.Name.Insert(record1.Name.Length, "1");
                                        csvWriter.WriteRecord(record1);
                                        csvWriter.NextRecord();
                                        InternalStructure record2 = new InternalStructure();
                                        record2 = recordsListFull[i];
                                        record2.Name = record2.Name.Remove(record2.Name.Length - 1, 1);
                                        record2.Name = record2.Name.Insert(record2.Name.Length, "2");
                                        csvWriter.WriteRecord(record2);
                                        csvWriter.NextRecord();
                                        csvWriter.Flush();
                                    }
                                    else
                                    {
                                        csvWriter.WriteRecord(recordsListFull[i]);
                                        csvWriter.NextRecord();
                                        InternalStructure record1 = new InternalStructure();
                                        record1 = recordsListFull[i];
                                        record1.Name = record1.Name.Remove(record1.Name.Length - 1, 1);
                                        record1.Name = record1.Name.Insert(record1.Name.Length, "1");
                                        csvWriter.WriteRecord(record1);
                                        csvWriter.NextRecord();
                                        InternalStructure record2 = new InternalStructure();
                                        record2 = recordsListFull[i];
                                        record2.Name = record2.Name.Remove(record2.Name.Length - 1, 1);
                                        record2.Name = record2.Name.Insert(record2.Name.Length, "2");
                                        csvWriter.WriteRecord(record2);
                                        csvWriter.NextRecord();
                                        csvWriter.Flush();
                                    }
                                    i++;
                                }
                                else
                                {
                                    csvWriter.WriteRecord(records[csvWriter.Row - 2]);
                                    csvWriter.NextRecord();
                                    csvWriter.Flush();
                                }
                            }
                            csvWriter.WriteRecord(records[records.Count - 2]);
                            csvWriter.WriteRecord(records[records.Count - 1]);
                            csvWriter.Flush();
                        }
                        catch
                        {
                        }
                    }
                    if (!arguments.Contains("-silent"))
                        MessageBox.Show("Conversion complete", "Message");
                }
                catch
                {
                    if (!arguments.Contains("-silent"))
                        MessageBox.Show("File is in use!", "Error!");
                }
            }
        }

        public void MainForm_Load(object sender, EventArgs e)
        {
            if (arguments.Length > 1)
            {
                if (arguments.Length == 2 && arguments.Contains("-silent"))
                {
                    return;
                }
                else
                {
                    openFileDialog1.FileName = arguments[1];
                    textBox1.Text = openFileDialog1.FileName;
                    if (openFileDialog1.FileName == null || (!openFileDialog1.FileName.Contains("units") && !openFileDialog1.FileName.Contains(".csv")) || !File.Exists(openFileDialog1.FileName))
                    {
                        if (!arguments.Contains("-silent"))
                            MessageBox.Show("No or incorrect file selected!", "Error!");
                        else
                            Application.Exit();
                    }
                    else
                    {
                        try
                        {
                            button2.PerformClick();
                            Application.Exit();
                        }
                        catch
                        {
                            if (!arguments.Contains("-silent"))
                                MessageBox.Show("File is in use!", "Error!");
                            else
                                Application.Exit();
                        }
                    }
                }
            }
        }
    }
}
