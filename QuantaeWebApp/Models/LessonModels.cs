using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quantae.DataModel;
using Quantae.Engine;
using System.Diagnostics;

namespace Quantae.ViewModels
{
    public class LessonHubViewModel
    {
        public string CurrentTopicName { get; set; }
        public List<string> TopicHistory { get; set; }
        public bool IsCurrentTopicNew { get; set; }

        public LessonHubResponseModel ResponseModel { get; set; }

        public LessonHubViewModel()
        {
        }

        public LessonHubViewModel(string currentTopicName, bool isCurrentTopicNew)
        {
            TopicHistory = new List<string>();
            this.CurrentTopicName = currentTopicName;
            this.IsCurrentTopicNew = isCurrentTopicNew;
        }

        public void AddHistory(TopicHistoryItem thi)
        {
            Topic topicOperationsGetTopicFromHandle = TopicOperations.GetTopicFromHandle(thi.Topic);
            if (topicOperationsGetTopicFromHandle != null)
            {
                this.TopicHistory.Add(topicOperationsGetTopicFromHandle.TopicName);
            }
            else
            {
                Trace.TraceError("LessonHubModel: AddHistory encountered a null topic history item.");
            }
        }
    }

    public class LessonHubResponseModel
    {
    }
}