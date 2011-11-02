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
                    var model = new QuestionExcerciseViewModel();
                    resultModel = model;
                    model.SentenceText = sentence.SentenceText;
                    model.SentenceTranslation = sentence.SentenceTranslation;

                    // Retriev all the Question related information from the sentence           
                    Question q;
                    if (sentence.Questions.TryGetValue(sentenceResult.QuestionDimension, out q))
                    {
                        model.QuestionSubText = q.QuestionSubstring;
                        model.BlankIndex = q.BlankPosition;
                        model.CorrectAnswerChoice = q.CorrectAnswerIndex;

                        for (int i = 0; i < q.AnswerChoices.Count; i++)
                        {
                            ViewModelAnswerChoice vmac = new ViewModelAnswerChoice();
                            vmac.Choice = q.AnswerChoices[i].Answer;
                            
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
                else
                {
                    // Create a Grammar Question
                    var model = new GrammarQuestionExcerciseViewModel();
                    resultModel = model;
                    model.SentenceText = sentence.SentenceText;
                    model.SentenceTranslation = sentence.SentenceTranslation;

                    // Retrieve all the Question related information from the sentence           
                    Question q;
                    if (sentence.Questions.TryGetValue(sentenceResult.QuestionDimension, out q))
                    {
                        model.QuestionSubText = q.QuestionSubstring;
                        model.BlankIndex = q.BlankPosition;
                        model.CorrectAnswerChoice = q.CorrectAnswerIndex;

                        for (int i = 0; i < q.AnswerChoices.Count; i++)
                        {
                            ViewModelAnswerChoice vmac = new ViewModelAnswerChoice();
                            vmac.Choice = q.AnswerChoices[i].Answer;

                            model.AnswerChoices.Add(vmac);
                        }

                        foreach (var seg in q.AnswerSegments)
                        {
                            AnswerSegment answerSeg = new AnswerSegment();
                            answerSeg.Text = seg;
                            model.AnswerSegments.Add(answerSeg);
                        }
                    }

                    //
                    // Populate Grammar Analysis related model fields
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

                        // Update the model
                        model.GrammarAnalysis.Add(ga);
                    }

                    //
                    // Populate Grammar Entries related model fields
                    foreach (var e in sentence.GrammarEntries)
                    {
                        GrammarEntryModel ge = new GrammarEntryModel();
                        GrammarEntry entry = Repositories.Repositories.GrammarEntries.GetItemByHandle<GrammarEntryHandle>(e);

                        ge.Word = entry.Text;
                        ge.Translation = entry.Translation;

                        // Update the model
                        model.GrammarEntries.Add(ge);
                    }
                }
            }
            else
            {
                // It's a regular sample sentence.  Now create right model depending on whether user is 
                // Contextual or Analytical

                if (LearningTypeScorePolicies.IsAnalytical(profile))
                {
                    var model = new AnalyticalExcerciseViewModel();
                    resultModel = model;
                    model.SentenceText = sentenceResult.Sentence.SentenceText;
                    model.SentenceTranslation = sentenceResult.Sentence.SentenceTranslation;


                    //
                    // Populate Grammar Analysis related model fields
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

                        // Update the model
                        model.GrammarAnalysis.Add(ga);
                    }

                    //
                    // Populate Grammar Entries related model fields
                    foreach (var e in sentence.GrammarEntries)
                    {
                        GrammarEntryModel ge = new GrammarEntryModel();
                        GrammarEntry entry = Repositories.Repositories.GrammarEntries.GetItemByHandle<GrammarEntryHandle>(e);

                        ge.Word = entry.Text;
                        ge.Translation = entry.Translation;

                        // Update the model
                        model.GrammarEntries.Add(ge);
                    }
                }
                else
                {
                    var model = new ContextualExcerciseViewModel();
                    resultModel = model;
                    model.SentenceText = sentenceResult.Sentence.SentenceText;
                    model.SentenceTranslation = sentenceResult.Sentence.SentenceTranslation;

                    //
                    // Populate Grammar Entries related model fields
                    foreach (var e in sentence.GrammarEntries)
                    {
                        GrammarEntryModel ge = new GrammarEntryModel();
                        GrammarEntry entry = Repositories.Repositories.GrammarEntries.GetItemByHandle<GrammarEntryHandle>(e);

                        ge.Word = entry.Text;
                        ge.Translation = entry.Translation;

                        // Update the model
                        model.GrammarEntries.Add(ge);
                    }
                }
            }

            return resultModel;
        }
    }
}
