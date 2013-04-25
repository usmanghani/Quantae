using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Text.RegularExpressions;
using System.IO;

namespace PhotoCollector
{
    using Amazon.WebServices.MechanicalTurk;
    using Amazon.WebServices.MechanicalTurk.Advanced;
    using Amazon.WebServices.MechanicalTurk.Domain;
    using System.Configuration;
    using System.Xml.Serialization;
    using System.Security.Cryptography;
    using System.Diagnostics;
    using System.Windows.Forms;

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

            MTurkConfig config = new MTurkConfig(ConfigurationManager.AppSettings[MTurkServiceUrlSettingName], ConfigurationManager.AppSettings[AwsAccessKeyIdSettingName], ConfigurationManager.AppSettings[AwsSecretAccessKeySettingName]);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm(config));
            
            //LoginForm loginForm = new LoginForm(config);
            //loginForm.ShowDialog();
            
            SimpleClient client = new SimpleClient(config);
            QuestionForm questionForm = QuestionUtil.DeserializeQuestionForm(hitdatatransformed);
            HIT hit = client.CreateHIT(null, "Find an Image for a given sentence.", "Find an Image for a given sentence.", null, questionForm, 0.10m, (long)TimeSpan.FromHours(4).TotalSeconds, null, (long)TimeSpan.FromDays(7).TotalSeconds, 1, null, null, null);
            Process.Start(client.GetPreviewURL(hit.HITTypeId));
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
