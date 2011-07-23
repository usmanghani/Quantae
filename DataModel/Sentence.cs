﻿using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Quantae.DataModel
{
    public class Sentence : QuantaeObject
    {
        public string SentenceText { get; set; }

        public string SentenceTranslation { get; set; }

        #region Structural Properties

        public List<VocabEntryHandle> VocabEntries { get; set; }

        // TODO: figure out a way to combine these if possible.
        //////////////////////////////////////////////////
        public List<GrammarEntryHandle> GrammarEntries { get; set; }
        public List<GrammarAnalysisElement> GrammarAnalysis { get; set; }
        public List<int[]> ContextualAnalysis { get; set; }
        /////////////////////////////////////////////////////

        // Automatic. Determined by the number of constructions in the sentence.
        // Its determined by Words.Length and Complexity(GrammarAnalysis).
        // Complexity(GrammarAnalysis) -> if (1->1) +=1; if ( N->M ) += N;
        // FUTURE: Final score = vacabentries.length + complexity
        // NOW: vocabentries.Count
        [BsonIgnore]
        public int DifficultyRank
        {
            get
            {
                return this.VocabEntries.Count;
            }
        }

        #endregion

        #region Topic and Rule Correspondence (this will be generated at input time when the grammarian inputs word attributes)

        public TopicHandle PrimaryTopic { get; set; }

        public List<TopicHandle> SecondaryTopics { get; set; }

        public List<QuantaeTuple<GrammarRoleHandle, Conjugation>> RoleConjugationPairs { get; set; }

        #endregion

        #region Questions

        public Dictionary<QuestionDimension, Question> Questions { get; set; }

        #endregion

        #region Media Properties

        public VideoRecording AnalyticalVideoRecording { get; set; }
        public VideoRecording ContextualVideoRecording { get; set; }
        public AudioRecording SpokenSentence { get; set; }
        public Picture ContextualPicture { get; set; }

        #endregion

        #region Misc Properties

        public List<string> Tags { get; set; }

        #endregion

        public Sentence()
        {
            this.SecondaryTopics = new List<TopicHandle>();
            this.RoleConjugationPairs = new List<QuantaeTuple<GrammarRoleHandle, Conjugation>>();
            this.Questions = new Dictionary<QuestionDimension, Question>();
            this.Tags = new List<string>();
            this.GrammarAnalysis = new List<GrammarAnalysisElement>();
            this.ContextualAnalysis = new List<int[]>();
            this.GrammarEntries = new List<GrammarEntryHandle>();
        }
    }

    public class SentenceHandle : QuantaeObjectHandle<Sentence>
    {
        public SentenceHandle(Sentence s) : base(s)
        {
        }
    }
}