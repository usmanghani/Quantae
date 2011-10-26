using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quantae.ViewModels
{
    public class ExerciseViewModel
    {
        public ExerciseViewModel()
        {

        ///  THE AWESOME STUFF FROM Usman's ppt
        ///  Contextual
        //            Var sentence = {
        //ObjectType: “Sentence”
        //sentenceType: “Sample”,
        //viewType: “Contextual”
        //sentenceText: <Arabic text goes here>,
        //sentenceTranslation: <translation goes here>,
        //grammarEntries: [ { word: <word>, translation: <trans> }, {word: <word>, translation: <trans>}, … ]
        //contextualAnalysis: [ { startIndex: 5, endIndex: 1 }, {startIndex: 5, endIndex: 2}, {startIndex: 5, endIndex: 5 } ],
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
        //grammarAnalysis: [ { startIndex: [2, 3, 4] , endIndex: 0, roleStart: خبر, roleEnd: مبتدأ}, {startIndex: 1, endIndex: 0, roleStart: مضاف إليه
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
    }

    public class ExerciseReponseModel
    {
        public ExerciseReponseModel()
        {
        }
    }
}