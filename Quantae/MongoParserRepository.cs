using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;
using Quantae.Repositories;

namespace Quantae.ParserLibrary
{
    public class MongoParserRepository : IParserRepository
    {
        public Topic GetTopicByIndex(int topic)
        {
            return Repositories.Repositories.Topics.FindOneAs(TopicQueries.GetTopicByIndex(topic));
        }

        public Sentence GetSentenceByText(string text)
        {
            return Repositories.Repositories.Sentences.FindOneAs(SentenceQueries.GetSentencesByText(text));
        }

        public void SaveSentence(Sentence sentence)
        {
            Repositories.Repositories.Sentences.Save(sentence);
        }

        public IEnumerable<VocabEntry> GetVocabEntriesByText(string text)
        {
            return Repositories.Repositories.Vocabulary.FindAs(VocabQueries.GetVocabEntryByText(text), indexHint: "Text");
        }

        public void SaveVocabEntry(VocabEntry entry)
        {
            Repositories.Repositories.Vocabulary.Save(entry);
        }

        public IEnumerable<GrammarEntry> GetGrammarEntriesByText(string text)
        {
            return Repositories.Repositories.GrammarEntries.FindAs(GrammarEntryQueries.GetGrammarEntryByText(text), indexHint: "Text");
        }

        public void SaveGrammarEntry(GrammarEntry entry)
        {
            Repositories.Repositories.GrammarEntries.Save(entry);
        }

        public GrammarRole GetGrammarRoleByName(string name)
        {
            return Repositories.Repositories.GrammarRoles.FindOneAs(GrammarRoleQueries.GetGrammarRoleByName(name));
        }

        public IEnumerable<GrammarRole> GetGrammarRolesByName(string name)
        {
            return Repositories.Repositories.GrammarRoles.FindAs(GrammarRoleQueries.GetGrammarRoleByName(name), indexHint: "RoleName");
        }

        public void SaveGrammarRole(GrammarRole role)
        {
            Repositories.Repositories.GrammarRoles.Save(role);
        }

        public long GetTopicCount()
        {
            return Repositories.Repositories.Topics.CountItems();
        }

        public Topic GetTopicByName(string name)
        {
            return Repositories.Repositories.Topics.FindOneAs(TopicQueries.GetTopicByName(name));

        }

        public void SaveTopic(Topic t)
        {
            Repositories.Repositories.Topics.Save(t);
        }
    }
}
