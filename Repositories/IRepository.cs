using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Quantae.Repositories
{
    public interface IRepository<TObject>
    {
        DataStore DataStore { get; }
        string CollectionName { get; }
        MongoCollection<TObject> Collection { get; }

        IEnumerable<TObject> GetAllItems();
        int CountItems();
        IEnumerable<TObject> FindAs(IMongoQuery query, IMongoSortBy sortBy = null, int skip = -1, int limit = -1, int batchsize = -1);
        TObject FindOneAs(IMongoQuery query);
        bool Update(IMongoQuery query, IMongoUpdate update, bool upsert = false, bool updateAllMatching = false);
        void Save(TObject doc);
        void Remove(IMongoQuery query);
    }
}
