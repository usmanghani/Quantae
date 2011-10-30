using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuantaeWebApp.Models;

namespace Quantae.ViewModels
{
    public class BaseSentenceModel
    {       
        public ObjectTypes ObjectType { get; protected set; }
        public SentenceTypes SentenceType { get; protected set; }
        public string SentenceText { get; set; }
        public string SentenceTranslation { get; set; }
    }
}