using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.ParserLibrary
{
    public interface IParserRepository
    {
        Topic GetTopicByIndex(int topic);
        void SaveSentence(Sentence sentence);
        Sentence GetSentenceByText(string text);
        IEnumerable<VocabEntry> GetVocabEntriesByText(string text);
        void SaveVocabEntry(VocabEntry entry);
        IEnumerable<GrammarEntry> GetGrammarEntriesByText(string text);
        void SaveGrammarEntry(GrammarEntry entry);
        GrammarRole GetGrammarRoleByName(string name);
        IEnumerable<GrammarRole> GetGrammarRolesByName(string name);
        void SaveGrammarRole(GrammarRole role);
    }
}
