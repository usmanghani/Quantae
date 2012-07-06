using System;
using System.Collections.Generic;
using System.Text;
using Amazon.WebServices.MechanicalTurk;
using Amazon.WebServices.MechanicalTurk.Domain;

namespace ReviewPolicySample
{
    class MTurkReviewPolicy
    {
        private SimpleClient client = new SimpleClient();

        /// <summary>
        /// Check if there are enough funds in your account in order to create the HIT
        /// on Mechanical Turk
        /// </summary>
        /// <returns>true if there are sufficient funds. False if not.</returns>
        public bool HasEnoughFunds()
        {
            return (client.GetAvailableAccountBalance() > 0.01m);
        }

        /// <summary>
        /// Creates a HIT that uses the Plurality hit review policy. This example will auto-approve assignments where the answer agrees with the majority and reject otherwise.
        /// </summary>
        public void CreateHITWithReviewPolicy()
        {
            string hitTypeId = client.RegisterHITType("Answer a plurality question", "This is a HIT created by the Mechanical Turk .Net SDK", 60, 60, 0.01m, ".net test", null);

            ReviewPolicy policy = new ReviewPolicy();
            PolicyParameter[] parameters = new PolicyParameter[4];
            PolicyParameter questionIds = new PolicyParameter();
            questionIds.Key = "QuestionIds";
            questionIds.Value = new string[1] { "1" };
            parameters[0] = questionIds;
            PolicyParameter agreementThreshold = new PolicyParameter();
            agreementThreshold.Key = "QuestionAgreementThreshold";
            agreementThreshold.Value = new string[1] { "50" };
            parameters[1] = agreementThreshold;
            PolicyParameter approveThreshold = new PolicyParameter();
            approveThreshold.Key = "ApproveIfWorkerAgreementScoreIsAtLeast";
            approveThreshold.Value = new string[1] { "100" };
            parameters[2] = approveThreshold;
            PolicyParameter rejectThreshold = new PolicyParameter();
            rejectThreshold.Key = "RejectIfWorkerAgreementScoreIsLessThan";
            rejectThreshold.Value = new string[1] { "100" };
            parameters[3] = rejectThreshold;
            policy.PolicyName = "SimplePlurality/2011-09-01";
            policy.Parameter = parameters;

            CreateHITRequest req = new CreateHITRequest();
            req.HITTypeId = hitTypeId;
            req.Description = "Plurality test hit created by Mechanical Turk .Net SDK";
            req.HITReviewPolicy = policy;
            req.MaxAssignments = 3;
            req.Question = QuestionUtil.ConvertSingleFreeTextQuestionToXML("Hello");
            req.Reward = new Price();
            req.Reward.Amount = 2;
            req.Reward.CurrencyCode = "USD";
            req.Title = "Hello <something>";
            req.AssignmentDurationInSeconds = 60;
            req.LifetimeInSeconds = 3600;

            HIT hit = client.Proxy.CreateHIT(req);

            // output ID and Url of new HIT (URL where HIT is available on the Mechanical Turk worker website)
            Console.WriteLine("Reward: {0}", req.Reward.Amount);
            Console.WriteLine("Created HIT: {0} ({1})", hit.HITId, client.GetPreviewURL(hit.HITTypeId));
        }
    }
}
