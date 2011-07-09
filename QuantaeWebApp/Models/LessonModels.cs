using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quantae.ViewModels
{
    public class LessonHubModel
    {
        public string CurrentTopicName { get; set; }
        public List<string> TopicHistory { get; set; }

        public LessonHubModel()
        {
            TopicHistory = new List<string>();
        }
    }
}