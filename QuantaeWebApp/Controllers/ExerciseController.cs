using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quantae.Engine;
using Quantae.DataModel;
using Quantae.ViewModels;
using Quantae.Repositories;

namespace QuantaeWebApp.Controllers
{
    public class ExerciseController : Controller
    {
        //
        // GET: /Exercise/
        [Authorize]
        public ActionResult Index()
        {
            ISentenceSelectionEngine engine = new SentenceSelectionEngine();
            UserProfile profile = UserOperations.GetUserProfileFromSession(User.Identity.Name);

            // HACK: Remove this when intro slides are in.
            #region Intro slide work around hack
            if (profile.History.VocabHistory.Count <= 0)
            {
                TopicHistoryItem currentTopicHistoryItem = profile.CurrentState.CourseLocationInfo.CurrentTopic;
                Sentence sentence = Repositories.Sentences.FindAs(SentenceQueries.GetSentencesByTopic(currentTopicHistoryItem.Topic)).First();
                foreach (var ve in sentence.VocabEntries)
                {
                    profile.History.VocabHistory.Add(new VocabularyHistoryItem
                    {
                        Rank = VocabRankTypes.CorrectOrSeenInIntro,
                        FailureCount = 0,
                        SuccessCount = 0,
                        LastTimestamp = DateTime.UtcNow,
                        VocabEntry = ve
                    });
                }
            }
            #endregion

            GetNextSentenceResult result = engine.GetNextSentence(profile);
            
            BaseSentenceModel model = null;
            
            if (result.Success)
            {
                model = ExcerciseViewModelFactory.CreateExcerciseViewModel(result, profile);                                
            }

            return View(ViewNames.Exercise.ExerciseIndexView, model);            
        }

    }
}
