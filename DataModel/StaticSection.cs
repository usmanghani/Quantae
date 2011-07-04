using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Quantae.DataModel
{
    [BsonKnownTypes(typeof(IntroSection), typeof(DepthSection))]
    public abstract class StaticSection
    {
        public List<StaticPage> Pages { get; set; }
    }
}