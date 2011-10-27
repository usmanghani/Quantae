using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMvcApp.Models
{
    public struct ContextualAnalysis
    {
        public int[] startIndex { get; set; }
        public int[] endIndex { get; set; }
    }

    public struct GrammarEntry
    {
        public string Word { get; set; }
        public string Translation { get; set; }
    }

    public class TestModel
    {
        public List<ContextualAnalysis> ContextualAnalysis { get; set; }
        public List<GrammarEntry> GrammarEntries { get; set; }

        public TestModel()
        {
            ContextualAnalysis = new List<ContextualAnalysis>();
            GrammarEntries = new List<GrammarEntry>();
        }
    }

    public class TestModelDerived : TestModel
    {
        public int Index { get; set; }
    }
}