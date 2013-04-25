using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quantae.ViewModels
{
    public class IntroSlideViewModel
    {
        public IntroSlideViewModel()
        {
        }

        public IntroSlideViewModel(string content)
        {
            this.Content = content;
        }

        public string Content { get; set; }
    }
}