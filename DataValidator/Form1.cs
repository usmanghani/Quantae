using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Quantae.ParserLibrary;
using Microsoft.Office.Interop.Excel;

namespace DataValidator
{
    public partial class Form1 : Form
    {
        TopicsParser topicParser = null;
        SentenceParser sentenceParser = null;
        public Form1()
        {
            InMemoryParserRepository repository = new InMemoryParserRepository();
            topicParser = new TopicsParser(repository);
            sentenceParser = new SentenceParser(repository);

            InitializeComponent();
        }

        private void btnLoadTopicInputFile_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(this);

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(openFileDialog1.FileName))
                {
                    txtTopicFileName.Text = openFileDialog1.FileName;
                    try
                    {
                        DoTopicParsingLogic(openFileDialog1.FileName);
                        txtTopicParsingLog.AppendText("Success!");
                    }
                    catch (Exception ex)
                    {
                        txtTopicParsingLog.AppendText(ex.ToString() + Environment.NewLine);
                    }
                }
            }
        }

        private void DoTopicParsingLogic(string excelFile)
        {
            txtTopicParsingLog.AppendText(string.Format("Exporting from excel file: {0}{1}", excelFile, Environment.NewLine));
            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            //excelApp.Visible = true;

            Workbook workBook = excelApp.Workbooks.Open(excelFile, UpdateLinks: 3);

            workBook.RunAutoMacros(XlRunAutoMacro.xlAutoOpen);
            workBook.Activate();
            var outputSheet = (Worksheet)workBook.Sheets.get_Item("Output");
            outputSheet.Activate();

            string filename = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "graph.txt");

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            try
            {
                outputSheet.SaveAs(Filename: filename, FileFormat: XlFileFormat.xlUnicodeText);
            }
            catch
            {
            }

            workBook.Close(SaveChanges: false);

            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

            System.Windows.Forms.Application.DoEvents();

            txtTopicParsingLog.AppendText("Succesfully exported from excel!" + Environment.NewLine);

            txtTopicParsingLog.AppendText("Parsing topics now..." + Environment.NewLine);

            topicParser.PopulateTopics(filename);

            txtTopicParsingLog.AppendText("Done parsing topics" + Environment.NewLine);
        }

        private void btnOpenSentencesFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog(this);

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(folderBrowserDialog1.SelectedPath))
                {
                    txtSentenceFolderName.Text = folderBrowserDialog1.SelectedPath;
                    try
                    {
                        DoSentenceParsingLogic(folderBrowserDialog1.SelectedPath);
                        txtSentenceParsingLog.AppendText("Success!");
                    }
                    catch (Exception ex)
                    {
                        txtSentenceParsingLog.AppendText(ex.ToString() + Environment.NewLine);
                    }
                }
            }
        }

        private void DoSentenceParsingLogic(string folderName)
        {
            string intermediatedirectoryname = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "intermediate-sentence-parser-store");
            if (!Directory.Exists(intermediatedirectoryname))
            {
                Directory.CreateDirectory(intermediatedirectoryname);
            }

            txtSentenceParsingLog.AppendText("Exporting from Excel!" + Environment.NewLine);

            ///// EXPORT FROM EXCEL
            var files = Directory.GetFiles(folderName, "Database - Topic*xlsm");

            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            //excelApp.Visible = true;
            int i = 1;
            foreach (var file in files)
            {
                txtSentenceParsingLog.AppendText(string.Format("Exporting file: {0}{1}", file, Environment.NewLine));

                Workbook workBook = excelApp.Workbooks.Open(file, UpdateLinks: 3);
                workBook.RunAutoMacros(XlRunAutoMacro.xlAutoOpen);
                workBook.Activate();
                var outputSheet = (Worksheet)workBook.Sheets.get_Item("Output");
                outputSheet.Activate();
                string filename = string.Format("{0}\\topic{1}.txt", intermediatedirectoryname, i.ToString("D3"));
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                try
                {
                    outputSheet.SaveAs(Filename: filename, FileFormat: XlFileFormat.xlUnicodeText);
                }
                catch
                {
                }

                workBook.Close(SaveChanges: false);
                i++;

                System.Windows.Forms.Application.DoEvents();
            }

            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            /////// EXPORT FROM EXCEL

            txtSentenceParsingLog.AppendText("Done exporting from excel!" + Environment.NewLine);

            txtSentenceParsingLog.AppendText("Cleaning up files" + Environment.NewLine);

            //// TRANSFORM TEXT FILES
            
            files = Directory.GetFiles(intermediatedirectoryname, "topic*.txt");
            
            foreach (var file in files)
            {
                txtSentenceParsingLog.AppendText(string.Format("Cleaning file {0}{1}", file, Environment.NewLine));
                var lines = File.ReadAllLines(file);
                File.Delete(file);
                File.WriteAllLines(file, lines.Skip(1));
                System.Windows.Forms.Application.DoEvents();
            }

            ///// TRANSFORM TEXT FILES

            txtSentenceParsingLog.AppendText("Done cleaning up files!" + Environment.NewLine);


            txtSentenceParsingLog.AppendText("Parsing sentences now... Be patient" + Environment.NewLine);

            HashSet<int> skippedTopics = new HashSet<int>();

            for (int topic = 1; topic <= 20; topic++)
            {
                if (skippedTopics.Contains(topic))
                {
                    txtSentenceParsingLog.AppendText(string.Format("Skipping topic {0}{1}", topic, Environment.NewLine));
                    continue;
                }

                System.Windows.Forms.Application.DoEvents();

                string filename = string.Format("{0}\\topic{1}.txt", intermediatedirectoryname, topic.ToString("D3"));
                txtSentenceParsingLog.AppendText(string.Format("Parsing file {0}{1}", filename, Environment.NewLine));

                sentenceParser.PopulateSentences(filename, topic);

                System.Windows.Forms.Application.DoEvents();
            }

            txtSentenceParsingLog.AppendText("Done parsing sentences!" + Environment.NewLine);
        }

    }
}
