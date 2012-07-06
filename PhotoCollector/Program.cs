using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Text.RegularExpressions;

namespace PhotoCollector
{
    class Program
    {
        const string TemplateRegexPattern = "(?<variable>{{.*?}})";
        const string AwsAccessKeyIdSettingName = "AwsAccessKeyId";
        const string AwsSecretAccessKeySettingName = "AwsSecretAccessKey";
        const string MTurkServiceUrlSettingName = "MTurkServiceUrl";

        static void Main(string[] args)
        {
                        
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
                    sb.Replace(tag, env[variableName], m.Index, m.Length);
                }

                m = regex.Match(sb.ToString(), m.Index + m.Length);
            }

            return sb.ToString();
        }
    }
}
