using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            TopicHistory = new List<string>();
        }
    }

    public class LessonHubResponseModel
    {
    }
}