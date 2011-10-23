using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quantae.ViewModels
{
    public class DepthSlideViewModel
    {
        public DepthSlideViewModel()
        {
        }

        public DepthSlideViewModel(string content)
        {
            this.Content = content;
        }

        public string Content { get; set; }
    }
}