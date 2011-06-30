using System.Collections.Generic;

namespace Quantae.DataModel
{
    public class Sentence : QuantaeObject
    {
        public string SentenceText { get; set; }

        public string SentenceTranslation { get; set; }

        #region Structural Properties

        public SentenceType Type { get; set; }

        public List<VocabEntryHandle> VocabEntries { get; set; }

        public List<GrammarEntry> GrammarEntries { get; set; }

        public List<GrammarAnalysisElement> GrammarAnalysis { get; set; }

        public List<int[]> ContextualAnalysis { get; set; }

        // Automatic. Determined by the number of constructions in the sentence.
        // Its determined by Words.Length and Complexity(GrammarAnalysis).
        // Complexity(GrammarAnalysis) -> if (1->1) +=1; if ( N->M ) += N;
        // FUTURE: Final score = vacabentries.length + complexity
        // NOW: vocabentries.Count
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

        public List<Question> Questions { get; set; }

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
            this.Questions = new List<Question>();
            this.Tags = new List<string>();
        }
    }

    public class SentenceHandle : QuantaeObjectHandle<Sentence>
    {
        public SentenceHandle(Sentence s) : base(s)
        {
        }
    }
}