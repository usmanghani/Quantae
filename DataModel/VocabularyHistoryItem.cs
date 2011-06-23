using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae
{
    public class VocabularyHistoryItem : HistoryItem
    {
        public Word Word { get; set; }

        /// <summary>
        ///  This score is updated when:
        ///  1. In Vocab testing when the user gets the word right.
        ///  2. In Instruction and Sample slides where he is not tested on vocabulary, 
        ///  but still exposed to words. This will increase the success score by 2 or 3 based on
        ///  the place where he sees it.  Add 2 for sample slids and 3 for instructions slides.
        ///  FailureScore is only updated when he is tested for vocab and gets it wrong.
        /// </summary>

        public override bool Equals(object obj)
        {
            return Word.Equals(obj as Word);
        }

        public override int GetHashCode()
        {
            return Word.GetHashCode();
        }
    }
}
