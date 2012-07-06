using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Text.RegularExpressions;
using System.IO;

namespace PhotoCollector
{
    using AwsMTurkRequester;
    using System.Configuration;
    using System.Xml.Serialization;

    class Program
    {
        const string TemplateRegexPattern = "(?<variable>{{.*?}})";
        const string AwsAccessKeyIdSettingName = "AwsAccessKeyId";
        const string AwsSecretAccessKeySettingName = "AwsSecretAccessKey";
        const string MTurkServiceUrlSettingName = "MTurkServiceUrl";

        static void Main(string[] args)
        {
            Dictionary<string, string> env = new Dictionary<string, string>() { { "QuestionText", "The girl is walking to the church" }, { "QuestionId", "1234" }, };
            string hitdataoriginal = File.ReadAllText("hitdata.xml");
            string hitdatatransformed = Transform(hitdataoriginal, env, false);
            //Console.WriteLine(hitdatatransformed);
            CreateHIT createHit = new CreateHIT();
            createHit.AWSAccessKeyId = ConfigurationManager.AppSettings[AwsAccessKeyIdSettingName];
            createHit.Credential = ConfigurationManager.AppSettings[AwsSecretAccessKeySettingName];
            CreateHITRequest request = new CreateHITRequest();
            createHit.Request = new CreateHITRequest[] { request };
            
            XmlSerializer serializer = new XmlSerializer(typeof(QuestionForm));
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(hitdatatransformed));
            QuestionForm questionForm = (QuestionForm)serializer.Deserialize(stream);
            stream = new MemoryStream();
            serializer.Serialize(stream, questionForm);
            //Console.WriteLine(Encoding.UTF8.GetString(stream.GetBuffer()));
            AWSMechanicalTurkRequesterPortTypeClient client = new AWSMechanicalTurkRequesterPortTypeClient();
            client.CreateHIT(createHit);
        }

        static string Transform(string input, IDictionary<string, string> env, bool xmlEscapeValues)
        {
            StringBuilder sb = new StringBuilder(input);

            Regex regex = new Regex(TemplateRegexPattern, RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            Match m = regex.Match(sb.ToString());

            while (m.Success)
            {
                string tag = m.Groups["variable"].Value;
                string variableName = tag.Replace("{{", "").Replace("}}", "");
                if (env.ContainsKey(variableName))
                {
                    string valueForVar = xmlEscapeValues ? SecurityElement.Escape(env[variableName]) : env[variableName];
                    sb.Replace(tag, valueForVar, m.Index, m.Length);
                }

                m = regex.Match(sb.ToString(), m.Index + m.Length);
            }

            return sb.ToString();
        }
    }
}
