using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuantaeWebApp.Models
{
    public enum ObjectTypes
    {
        Sentence,
        Question,
    }

    public enum SentenceTypes
    {
        Sample,
        VocabQuestion,
        GrammarQuestion,
    }

    public enum ViewTypes
    {
        Contextual,
        Analytical,
    }

    public struct ContextualAnalysis
    {
        // Example:
        //contextualAnalysis: [ { startIndex: [5], endIndex: [1] }, {startIndex: [5], endIndex: [2]}, {startIndex: [5], endIndex: [5] } ],

        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
    }

    public struct GrammarEntry
    {
        // Example:
        // grammarEntries: [ { word: <word>, translation: <trans> }, {word: <word>, translation: <trans>}, … ]
        
        public string Word { get; set; }
        public string Translation { get; set; }
    }

    
    public struct GrammarAnalysis
    {
        // Example:
        // grammarAnalysis: [ { startIndex: [2, 3, 4] , endIndex: [0], roleStart: خبر, roleEnd: مبتدأ}, {startIndex: [1], endIndex: 0, roleStart: مضاف إليه
        //, roleEnd: مبتدأ}, {startIndex: 3, endIndex: 2, roleStart: فاعل, roleEnd: فعل}, { startIndex: 4, endIndex: 2, roleStart: مفعول به, roleEnd: فعل} ]
        
        public int[] StartIndex { get; set; }
        public int[] EndIndex { get; set; }
        public string RoleStart { get; set; }
        public string RoleEnd { get; set; }
    }

    public struct AnswerChoice
    {
        // Example:
        //answerChoices: [ <choice1>, <choice2>, <choice3>, <choice4> ]
        public string Choice { get; set; }
    }

    public struct QuestionFragment
    {
        // Example:
        //questionFragments: [ <frag1>, <frag2>, … ]
        public string Fragment { get; set; }
    }



}