using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class UserCurrentState : QuantaeObject<long>
    {
        /// <summary>
        /// Current word length that the user is supposed to see.
        /// Equals to the number of vocab elements the sentence contains.
        /// </summary>
        /// <value>
        /// The length of the current sentence.
        /// </value>
        public int CurrentSentenceLength { get; set; }

        /// <summary>
        /// Gets or sets the index of the current batch.
        /// number of items to skip. Used for batch paginations.
        /// </summary>
        /// <value>
        /// The index of the current batch.
        /// </value>
        public int CurrentBatchIndex { get; set; }

        /// <summary>
        /// Current Noun Conjugation Rank. It could be 0,1 or 2 based on which Number Rule the user is currently allowed to see.
        /// </summary>
        public int CurrentNounConjugationRank { get; set; }

        /// <summary>
        /// Current Verb Conjugation Ranks by Tense. This is per tense and can go from 0 to 6 or 0 to 2 based on which TenseRule it is.
        /// </summary>
        public Dictionary<TenseRule, int> CurrentVerbConjugationRanksByTense { get; set; }

        /// <summary>
        /// Gets or sets the current topic.
        /// </summary>
        /// <value>
        /// The current topic.
        /// </value>
        public TopicHandle CurrentTopic { get; set; }

        public CourseStateMachineState CourseStateMachineState { get; set; }
        public TopicStateMachineState TopicStateMachineState { get; set; }
        public SectionStateMachineState SectionStateMachineState { get; set; }

        public UserCurrentState()
        {
            CurrentVerbConjugationRanksByTense = new Dictionary<TenseRule, int>() { { TenseRule.Past, 0 }, { TenseRule.PresentFuture, 0 }, { TenseRule.Command, 0 } };
        }
    }
}
