using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuantaeWebApp.Models;

namespace Quantae.ViewModels
{
    public class ReviewViewModel : BaseSentenceModel
    {
        public ReviewViewModel(string text, string translation): 
            base (text, translation)
        {
            ObjectType = ObjectTypes.Sentence;
            SentenceType = SentenceTypes.Sample;
        }
    }
}