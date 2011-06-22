using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae
{
    public class Sentence : QuantaeObject<ulong>
    {
        public string SentenceText { get; set; }

        public string SentenceTranslation { get; set; }

        #region Structural Properties

        public SentenceType Type { get; set; }

        public List<Word> Words { get; set; }

        public List<GrammarAnalysisElement> GrammarAnalysis { get; set; }

        public List<int[]> ContextualAnalysis { get; set; }

        // Automatic. Determined by the number of constructions in the sentence.
        // Its determined by Words.Length and Complexity(GrammarAnalysis).
        // Complexity(GrammarAnalysis) -> if (1->1) +=1; if ( N->M ) += N;
        // Final score = words.length + complexity
        public double DifficultyRank { get; set; }

        #endregion

        #region Topic and Rule Correspondence (this will be generated at input time when the grammarian inputs word attributes)

        public List<Tuple<Topic, NounConjugation, VerbConjugation>> TopicRulePairs { get; set; }

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
    }
}
