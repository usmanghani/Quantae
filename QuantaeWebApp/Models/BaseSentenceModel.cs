using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuantaeWebApp.Models;

namespace Quantae.ViewModels
{
    public class BaseSentenceModel
    {       
        public ObjectTypes ObjectType { get; set; }
        public SentenceTypes SentenceType { get; set; }
        public string SentenceText { get; set; }
        public string SentenceTranslation { get; set; }
    }
}