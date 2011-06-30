﻿namespace Quantae.DataModel
{
    public class VocabEntry : QuantaeObject
    {
        public string RootWord { get; set; }
        public string Text { get; set; }
        public string Translation { get; set; }
        public WordType WordType { get; set; }
        public Conjugation Conjugation { get; set; }
        public int VerbForm { get; set; }
        public NounType NounType { get; set; }

        public override bool Equals(object obj)
        {
            return (obj as GrammarEntry).GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.RootWord.GetHashCode() + this.Translation.GetHashCode();
        }
    }

    public class VocabEntryHandle : QuantaeObjectHandle<VocabEntry>
    {
        public VocabEntryHandle(VocabEntry entry) : base(entry)
        {
        }
    }
}