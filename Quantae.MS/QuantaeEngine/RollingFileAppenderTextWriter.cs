using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Quantae.Engine
{
    public class RollingFileAppenderTextWriter : System.Diagnostics.TextWriterTraceListener
    {
        private const long Meg = 1024 * 1024;

        string dirName = Environment.CurrentDirectory;
        long maxFileSizeInMegs = 10 * Meg;
        string fileNamePrefix = "QuantaeTraces--";
        string defaultFileExtension = "log";
        string currentFileName = string.Empty;
        StreamWriter streamWriter = null;

        public RollingFileAppenderTextWriter()
        {
            if (this.Attributes.ContainsKey("dirName"))
            {
                dirName = this.Attributes["dirName"];
            }

            if (this.Attributes.ContainsKey("maxFileSize"))
            {
                maxFileSizeInMegs = int.Parse(this.Attributes["maxFileSize"]);
            }

            if (this.Attributes.ContainsKey("fileNamePrefix"))
            {
                fileNamePrefix = this.Attributes["fileNamePrefix"];
            }

            if (this.Attributes.ContainsKey("defaultFileExtension"))
            {
                fileNamePrefix = this.Attributes["defaultFileExtension"];
            }

            UpdateStreams();
        }

        public override void Flush()
        {
            this.streamWriter.Flush();
        }

        protected override string[] GetSupportedAttributes()
        {
            return new string[] { "dirName", "maxFileSize", "fileNamePrefix", "defaultFileExtension" };
        }

        public override void Write(string message)
        {
            this.WriteWithRollover(message);
        }

        public override void WriteLine(string message)
        {
            this.WriteLineWithRollover(message);
        }

        public override void TraceData(System.Diagnostics.TraceEventCache eventCache, string source, System.Diagnostics.TraceEventType eventType, int id, object data)
        {
            this.WriteLineWithRollover(CreateMessage(source, eventCache, id, data));
        }

        public override void TraceData(System.Diagnostics.TraceEventCache eventCache, string source, System.Diagnostics.TraceEventType eventType, int id, params object[] data)
        {
            this.WriteLineWithRollover(CreateMessage(source, eventType, id, data));
        }

        public override void TraceEvent(System.Diagnostics.TraceEventCache eventCache, string source, System.Diagnostics.TraceEventType eventType, int id)
        {
            this.WriteLineWithRollover(CreateMessage(source, eventType, id));
        }

        public override void TraceEvent(System.Diagnostics.TraceEventCache eventCache, string source, System.Diagnostics.TraceEventType eventType, int id, string format, params object[] args)
        {
            this.WriteLineWithRollover(CreateMessage(source, eventType, id, format, args));
        }

        public override void TraceEvent(System.Diagnostics.TraceEventCache eventCache, string source, System.Diagnostics.TraceEventType eventType, int id, string message)
        {
            this.WriteLineWithRollover(CreateMessage(source, eventCache, id, message));
        }

        public override void TraceTransfer(System.Diagnostics.TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            this.WriteLineWithRollover(CreateMessage(source, id, message, relatedActivityId));
        }

        public override void Close()
        {
            this.streamWriter.Close();
        }

        public override void Write(object o)
        {
            this.WriteWithRollover(CreateMessage(o));
        }

        public override void Write(object o, string category)
        {
            this.WriteWithRollover(CreateMessage(category, o));
        }

        public override void Write(string message, string category)
        {
            this.WriteWithRollover(CreateMessage(category, message));
        }

        protected override void WriteIndent()
        {
            this.WriteLineWithRollover(CreateMessage(null));
        }

        public override void WriteLine(object o)
        {
            this.WriteLineWithRollover(CreateMessage(o));
        }

        public override void WriteLine(object o, string category)
        {
            this.WriteLineWithRollover(CreateMessage(category, o));
        }

        public override void WriteLine(string message, string category)
        {
            this.WriteLineWithRollover(CreateMessage(category, message));
        }

        private void WriteWithRollover(string message)
        {
            RolloverIfRequired();
            this.streamWriter.Write(message);
        }

        private void WriteLineWithRollover(string message)
        {
            RolloverIfRequired();
            this.streamWriter.WriteLine(message);
        }

        private void RolloverIfRequired()
        {
            if (RolloverRequired())
            {
                RolloverFiles();
            }
        }

        private string CreateMessage(params object[] objs)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[");
            sb.Append(DateTime.UtcNow.ToString());
            sb.Append("]");

            if (objs == null)
            {
                return sb.ToString();
            }

            PrintArray(sb, objs);

            return sb.ToString();
        }

        private void PrintArray(StringBuilder sb, object[] p)
        {
            foreach (var a in p)
            {
                if (a.GetType().IsArray)
                {
                    PrintArray(sb, a as object[]);
                    continue;
                }

                sb.Append(",");
                sb.Append(a);
            }
        }

        private void RolloverFiles()
        {
            this.streamWriter.Flush();
            this.streamWriter.Close();

            UpdateStreams();
        }

        private bool RolloverRequired()
        {
            FileInfo fi = new FileInfo(GetFullFilePath());
            return (fi.Length > maxFileSizeInMegs);
        }

        private string GenerateNewFileName()
        {
            return fileNamePrefix + System.DateTime.UtcNow.ToString("yyyy.MM.dd.HH.mm.ss.fff") + "." + defaultFileExtension;
        }

        private string GetFullFilePath()
        {
            return System.IO.Path.Combine(dirName, currentFileName);
        }

        private string UpdateFileNameAndGetNewFullFileName()
        {
            currentFileName = GenerateNewFileName();
            string filename = System.IO.Path.Combine(dirName, currentFileName);

            return filename;
        }

        private void UpdateStreams()
        {
            string filename = UpdateFileNameAndGetNewFullFileName();
            streamWriter = new StreamWriter(filename, true);
        }
    }
}
