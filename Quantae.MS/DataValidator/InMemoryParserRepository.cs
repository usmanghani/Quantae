﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.ParserLibrary;
using Quantae.DataModel;
using MongoDB.Bson;

namespace DataValidator
{
    public class InMemoryParserRepository : IParserRepository
    {
        Dictionary<int, Topic> topics = new Dictionary<int, Topic>();
        Dictionary<string, Sentence> sentences = new Dictionary<string, Sentence>();
        Dictionary<string, GrammarRole> grammarRoles = new Dictionary<string, GrammarRole>();
        Dictionary<string, GrammarEntry> grammarEntries = new Dictionary<string, GrammarEntry>();
        Dictionary<string, VocabEntry> vocabEntries = new Dictionary<string, VocabEntry>();

        public Topic GetTopicByIndex(int topic)
        {
            if (!topics.ContainsKey(topic)) return null;
            return topics[topic];
        }

        public void SaveSentence(Sentence sentence)
        {
            if (sentence.ObjectId == null)
            {
                sentence.ObjectId = BsonObjectId.GenerateNewId();
            }

            sentences[sentence.SentenceText] = sentence;
        }

        public Sentence GetSentenceByText(string text)
        {
            if (!sentences.ContainsKey(text)) return null; 
            return sentences[text];
        }

        public IEnumerable<VocabEntry> GetVocabEntriesByText(string text)
        {
            if (!vocabEntries.ContainsKey(text))
            {
                return Enumerable.Empty<VocabEntry>();
            }

            return new VocabEntry[] { vocabEntries[text] };
        }

        public void SaveVocabEntry(VocabEntry entry)
        {
            if (entry.ObjectId == null)
            {
                entry.ObjectId = BsonObjectId.GenerateNewId();
            }

            vocabEntries[entry.Text] = entry;
        }

        public IEnumerable<GrammarEntry> GetGrammarEntriesByText(string text)
        {
            if (!grammarEntries.ContainsKey(text))
            {
                return Enumerable.Empty<GrammarEntry>();
            }

            return new GrammarEntry[] { grammarEntries[text] };
        }

        public void SaveGrammarEntry(GrammarEntry entry)
        {
            if (entry.ObjectId == null)
            {
                entry.ObjectId = BsonObjectId.GenerateNewId();
            }

            grammarEntries[entry.Text] = entry;
        }

        public GrammarRole GetGrammarRoleByName(string name)
        {
            if (!grammarRoles.ContainsKey(name))
            {
                return null;
            }

            return grammarRoles[name];
        }

        public IEnumerable<GrammarRole> GetGrammarRolesByName(string name)
        {
            if (!grammarRoles.ContainsKey(name))
            {
                return Enumerable.Empty<GrammarRole>();
            }

            return new GrammarRole[] { GetGrammarRoleByName(name) };
        }

        public void SaveGrammarRole(GrammarRole role)
        {
            if (role.ObjectId == null)
            {
                role.ObjectId = BsonObjectId.GenerateNewId();
            }

            this.grammarRoles[role.RoleName] = role;
        }

        public long GetTopicCount()
        {
            return topics.Count;
        }

        public Topic GetTopicByName(string name)
        {
            foreach (var t in topics.Values)
            {
                if (t.TopicName.Equals(name))
                {
                    return t;
                }
            }

            return null;
        }

        public void SaveTopic(Topic t)
        {
            if (t.ObjectId == null)
            {
                t.ObjectId = BsonObjectId.GenerateNewId();
            }

            topics[t.Index] = t;
        }
    }
}
