using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Quantae.DataModel.Sql
{
    [BsonKnownTypes(typeof(IntroSection), typeof(DepthSection))]
    public abstract class StaticSection
    {
        public List<Slide> Slides { get; set; }
    }
}