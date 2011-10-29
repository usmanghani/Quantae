using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuantaeWebApp.Models;

namespace Quantae.ViewModels
{
    public class ContextualExcerciseViewModel : BaseSentenceModel
    {
        public string PictureLink { get; set; }
        public ViewTypes ViewType { get; private set; }
        public List<GrammarEntry> GrammarEntries { get; set; }
        public List<ContextualAnalysis> ContextualAnalysis { get; set; }

        public ContextualExcerciseViewModel()
        {
            ObjectType = ObjectTypes.Sentence;
            ViewType = ViewTypes.Contextual;
            SentenceType = SentenceTypes.Sample;
            GrammarEntries = new List<GrammarEntry>();
            ContextualAnalysis = new List<ContextualAnalysis>();
        }
    }

    public class AnalyticalExcerciseViewModel : BaseSentenceModel
    {
        public string PictureLink { get; set; }
        public ViewTypes ViewType { get; private set; }
        public List<GrammarEntry> GrammarEntries { get; set; }
        public List<GrammarAnalysis> GrammarAnalysis { get; set; }

        public AnalyticalExcerciseViewModel()
        {
            ObjectType = ObjectTypes.Sentence;
            SentenceType = SentenceTypes.Sample;
            ViewType = ViewTypes.Analytical;
            GrammarAnalysis = new List<GrammarAnalysis>();
            GrammarEntries = new List<GrammarEntry>();
        }
    }

    public class QuestionExcerciseViewModel : BaseSentenceModel
    {
        public List<AnswerChoice> AnswerChoices { get; set; }
        public string QuestionSubText { get; set; }
        public List<QuestionFragment> QuestionFragments { get; set; }
        public int BlankIndex { get; set; }
        public int CorrectAnswerChoice { get; set; }

        public QuestionExcerciseViewModel()
        {
            ObjectType = ObjectTypes.Question;
            SentenceType = SentenceTypes.VocabQuestion;
            AnswerChoices = new List<AnswerChoice>();
            QuestionFragments = new List<QuestionFragment>();
        }
    }

    public class GrammarQuestionExcerciseViewModel : QuestionExcerciseViewModel
    {
        public List<GrammarEntry> GrammarEntries { get; set; }
        public List<GrammarAnalysis> GrammarAnalysis { get; set; }

        public GrammarQuestionExcerciseViewModel()
        {
            ObjectType = ObjectTypes.Question;
            SentenceType = SentenceTypes.GrammarQuestion;
            GrammarEntries = new List<GrammarEntry>();
            GrammarAnalysis = new List<GrammarAnalysis>();
        }
    }
        
     public class ExerciseReponseModel
    {
        public ExerciseReponseModel()
        {
        }
    }

     ///  Contextual
     //            Var sentence = {
     //ObjectType: “Sentence”
     //sentenceType: “Sample”,
     //viewType: “Contextual”
     //sentenceText: <Arabic text goes here>,
     //sentenceTranslation: <translation goes here>,
     //grammarEntries: [ { word: <word>, translation: <trans> }, {word: <word>, translation: <trans>}, … ]
     //contextualAnalysis: [ { startIndex: [5], endIndex: [1] }, {startIndex: [5], endIndex: [2]}, {startIndex: [5], endIndex: [5] } ],
     //pictureLink: <link goes here>
     //}

     ///  ANALYTICAL
     ///  
     //        Var sentence = {
     //ObjectType: “Sentence”
     //sentenceType: “Sample”,
     //viewType: “Analytical”
     //sentenceText: <Arabic text goes here>,
     //SentenceTranslation: <translation goes here>,
     //grammarEntries: [ { word: <word>,   translation: <trans> }, {word: <word>, translation: <trans>}, … ]
     //grammarAnalysis: [ { startIndex: [2, 3, 4] , endIndex: [0], roleStart: خبر, roleEnd: مبتدأ}, {startIndex: [1], endIndex: 0, roleStart: مضاف إليه
     //, roleEnd: مبتدأ}, {startIndex: 3, endIndex: 2, roleStart: فاعل, roleEnd: فعل}, { startIndex: 4, endIndex: 2, roleStart: مفعول به, roleEnd: فعل} ]
     //}

     /// GrammarQuestion
     ///
     //            Var sentence = {
     //ObjectType: “Sentence”
     //sentenceType: “GrammarQuestion”,
     //sentenceText: <text>,
     //sentenceTranslation: <text>
     //answerChoices: [ <choice1>, <choice2>, <choice3>, <choice4> ]
     //questionSubText: <might or might not exist>
     //questionFragments: [ <frag1>, <frag2>, … ]
     //blankIndex: 1
     //correctAnswerChoice: 2
     //grammarEntries: [ { word: <word>, translation: <trans> }, {word: <word>, translation: <trans>}, … ]
     //grammarAnalysis: [ { startIndex: [2, 3, 4] , endIndex: 1, roleStart: جملة صلة, roleEnd: صفة}, {startIndex: 1, endIndex: 0, roleStart: صفة, roleEnd: مبتدأ}, {startIndex: 5, endIndex: 0, roleStart: خبر, roleEnd: مبتدأ}, {startIndex: 3, endIndex: 2, roleStart: فاعل, roleEnd: فعل}, { startIndex: 4, endIndex: 2, roleStart: مفعول به, roleEnd: فعل} ]

     //}

     /// Question
     /// 
     //            Var sentence = {
     //ObjectType: “Question”
     //sentenceType: “VocabQuestion”,
     //sentenceText: <text>
     //answerChoices: [ <choice1>, <choice2>, <choice3>, … ]
     //questionSubText: <might or might not exist>
     //questionFragments: [ <frag1>, <frag2>, … ]
     //blankIndex: 1
     //correctAnswerChoice: 2
     //}
}