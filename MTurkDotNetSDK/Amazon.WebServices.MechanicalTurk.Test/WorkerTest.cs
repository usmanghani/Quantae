using System;
using System.Collections.Generic;
using System.Text;
using NUnit;
using NUnit.Framework;

namespace Amazon.WebServices.MechanicalTurk.Test
{
    class WorkerTest
    {
        private const string WORKER_ID_KEY = "MechanicalTurk.Test.WorkerID";

        private static string WorkerID;

        #region setup
        [TestFixtureSetUp]
        public void CheckSandbox()
        {
            TestUtil.CheckSandbox();
            WorkerID = TestUtil.Client.Config.GetConfig(WORKER_ID_KEY, null);
            if (WorkerID == null)
            {
                throw new InvalidOperationException(WORKER_ID_KEY + " is not specified. Cannot run worker tests.");
            }
        }
        #endregion

        #region tests
        [Test, Description("Finds a requester-worker statistic")]
        public void GetRequesterWorkerStatistic()
        {
            GetRequesterWorkerStatisticRequest req = new GetRequesterWorkerStatisticRequest();
            req.TimePeriod = TimePeriod.LifeToDate;
            req.Statistic = RequesterStatistic.PercentKnownAnswersCorrect;
            req.WorkerId = WorkerID;
            GetStatisticResult result = TestUtil.Client.GetRequesterWorkerStatistic(req);
            Assert.IsNotNull(result.DataPoint);
            Assert.AreEqual(TimePeriod.LifeToDate, result.TimePeriod);
            Assert.AreEqual(RequesterStatistic.PercentKnownAnswersCorrect, result.Statistic);
            Assert.AreEqual(WorkerID, result.WorkerId);
        }
        #endregion
    }
}
