using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

using Quantae.DataModel;

namespace Quantae.ParserLibrary
{
    public class TopicsParser
    {
        IParserRepository repositoryContext;
        public TopicsParser(IParserRepository repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }

        public void PopulateTopics(string filename)
        {
            var lines = File.ReadAllLines(filename);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var tokens = line.Split("\t".ToCharArray());

                int index = int.Parse(tokens[0]);
                string name = tokens[1];

                List<int> dependencies = new List<int>();
                if (tokens.Length > 2)
                {
                    dependencies.AddRange(tokens.Skip(2).Where(i => !string.IsNullOrWhiteSpace(i)).Select(i => int.Parse(i)));
                }

                Topic t = new Topic() { Index = index, TopicName = name };

                t.Dependencies = dependencies;

                // check if topic already exists
                bool topicExistsByName = repositoryContext.GetTopicByName(name) != null;
                bool topicExistsByIndex = repositoryContext.GetTopicByIndex(index) != null;
                if (!topicExistsByName && !topicExistsByIndex) // if topic doesn't exist then save it.
                {
                    repositoryContext.SaveTopic(t);
                }
                else // update all fields
                {
                    Topic t2 = repositoryContext.GetTopicByIndex(index);
                    t2.TopicName = name;
                    t2.Index = index;
                    t2.Dependencies = t.Dependencies;
                    repositoryContext.SaveTopic(t2);
                }
            }

            CreateForwardLinks();
        }

        private void CreateForwardLinks()
        {
            var count = repositoryContext.GetTopicCount();
            for (var i = 0; i < count; i++)
            {
                Topic t = repositoryContext.GetTopicByIndex(i);

                if (t == null || t.Dependencies == null || t.Dependencies.Count == 0)
                {
                    continue;
                }

                foreach (var d in t.Dependencies)
                {
                    Topic f = repositoryContext.GetTopicByIndex(d);
                    if (f.ForwardLinks == null)
                    {
                        f.ForwardLinks = new List<int>();
                    }

                    if (!f.ForwardLinks.Contains(t.Index))
                    {
                        f.ForwardLinks.Add(t.Index);
                    }

                    repositoryContext.SaveTopic(f);
                }
            }
        }
    }
}
