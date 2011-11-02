﻿using System;
using System.Collections.Generic;
using System.Linq;
using QuantaeWebApp.Models;
using Quantae.Engine;
using Quantae.DataModel;
using System.Diagnostics;

namespace Quantae.ViewModels
{
    public class ExcerciseViewModelFactory
    {
        public static BaseSentenceModel CreateExcerciseViewModel(GetNextSentenceResult sentenceResult, UserProfile profile)
        {
            BaseSentenceModel resultModel;
            var sentence = sentenceResult.Sentence;

            // While in Excercise Controller, sentence should either be a Sample sentence or a Question sentence
            // Review sentence would be an error
            if (sentenceResult.IsReview)
            {
                Trace.TraceError("Unexpected sentence result.  Should not receive Review sentence during excericse flow.");
            }
            if (sentenceResult.IsQuestion)
            {
                bool isGrammarQ = (sentenceResult.QuestionDimension == QuestionDimension.Grammar);

                if (!isGrammarQ)
                {
                    // If it's not grammar question it's a default contextual question
                    var model = new QuestionExcerciseViewModel(sentence.SentenceText, sentence.SentenceTranslation);
                    resultModel = model;

                    // Retriev all the Question related information from the sentence
                    PopulateQuestionFields(sentenceResult, model);
                }
                else
                {
                    // Create a Grammar Question
                    var model = new GrammarQuestionExcerciseViewModel(sentence.SentenceText, sentence.SentenceTranslation);
                    resultModel = model;

                    // Retriev all the Question related information from the sentence           
                    PopulateQuestionFields(sentenceResult, model);

                    //
                    // Populate Grammar Analysis related model fields
                    PopulateGrammarAnalysisFields(sentence, model.GrammarAnalysis);

                    //
                    // Populate Grammar Entries related model fields
                    PopulateGrammarEntriesFields(sentence, model.GrammarEntries);
                }
            }
            else
            {
                // It's a regular sample sentence.  Now create right model depending on whether user is 
                // Contextual or Analytical

                if (LearningTypeScorePolicies.IsAnalytical(profile))
                {
                    var model = new AnalyticalExcerciseViewModel(sentence.SentenceText, sentence.SentenceTranslation);
                    resultModel = model;

                    //
                    // Populate Grammar Analysis related model fields
                    PopulateGrammarAnalysisFields(sentence, model.GrammarAnalysis);

                    //
                    // Populate Grammar Entries related model fields
                    PopulateGrammarEntriesFields(sentence, model.GrammarEntries);
                }
                else
                {
                    var model = new ContextualExcerciseViewModel(sentence.SentenceText, sentence.SentenceTranslation);
                    resultModel = model;

                    //
                    // Populate Grammar Entries related model fields
                    PopulateGrammarEntriesFields(sentence, model.GrammarEntries);
                }
            }

            return resultModel;
        }

        private static void PopulateGrammarEntriesFields(Sentence sentence, List<GrammarEntryModel> entries)
        {
            foreach (var e in sentence.GrammarEntries)
            {
                GrammarEntryModel ge = new GrammarEntryModel();
                GrammarEntry entry = Repositories.Repositories.GrammarEntries.GetItemByHandle<GrammarEntryHandle>(e);

                ge.Word = entry.Text;
                ge.Translation = entry.Translation;

                // Update the entries
                entries.Add(ge);
            }
        }

        private static void PopulateGrammarAnalysisFields(Sentence sentence, List<GrammarAnalysis> analysis)
        {
            foreach (var a in sentence.GrammarAnalysis)
            {
                GrammarAnalysis ga = new GrammarAnalysis();

                // Start Role -----
                ga.StartIndex = new int[a.StartSegmentRolePair.Item1.Count];
                a.StartSegmentRolePair.Item1.CopyTo(ga.StartIndex);

                GrammarRole gr = Repositories.Repositories.GrammarRoles.GetItemByHandle<GrammarRoleHandle>(a.StartSegmentRolePair.Item2);
                ga.RoleStart = gr.RoleName;

                // End Role ----
                ga.EndIndex = new int[a.EndSegmentRolePair.Item1.Count];
                a.EndSegmentRolePair.Item1.CopyTo(ga.EndIndex);

                GrammarRole gr2 = Repositories.Repositories.GrammarRoles.GetItemByHandle<GrammarRoleHandle>(a.EndSegmentRolePair.Item2);
                ga.RoleEnd = gr2.RoleName;

                // Update the analysis
                analysis.Add(ga);
            }
        }

        private static void PopulateQuestionFields(GetNextSentenceResult sentenceResult, QuestionExcerciseViewModel model)
        {
            var sentence = sentenceResult.Sentence;
            Question q;
            if (sentence.Questions.TryGetValue(sentenceResult.QuestionDimension, out q))
            {
                model.QuestionSubText = q.QuestionSubstring;
                model.BlankIndex = q.BlankPosition;
                model.CorrectAnswerChoice = q.CorrectAnswerIndex;

                foreach (var a in q.AnswerChoices)
                {
                    ViewModelAnswerChoice vmac = new ViewModelAnswerChoice();
                    vmac.Choice = a.Answer;
                    model.AnswerChoices.Add(vmac);
                }

                foreach (var seg in q.AnswerSegments)
                {
                    AnswerSegment answerSeg = new AnswerSegment();
                    answerSeg.Text = seg;
                    model.AnswerSegments.Add(answerSeg);
                }
            }
            else
            {
                // TODO: Handle internal errors like this.  Should stop the flow?
                Trace.TraceError("Failed to get retrieve Question from sentence.");
            }
        }
    }
}
