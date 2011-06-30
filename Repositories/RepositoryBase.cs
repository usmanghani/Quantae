using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using Quantae.DataModel;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace Quantae.Repositories
{
    public abstract class RepositoryBase<TObject> where TObject : QuantaeObject
    {
        public DataStore DataStore { get; set; }
        public string CollectionName { get; set; }
        public MongoCollection<TObject> Collection { get; set; }

        public RepositoryBase(DataStore store, string collectionName)
        {
            this.DataStore = store;
            this.CollectionName = collectionName;
            this.Collection = store.GetCollection<TObject>(collectionName);
        }

        public TObject GetItemByHandle<HandleType>(HandleType handle) where HandleType : QuantaeObjectHandle<TObject>
        {
            return this.Collection.FindOneByIdAs<TObject>(new BsonObjectId(handle.ObjectId.Value));
        }

        public void Save(TObject doc)
        {
            SafeModeResult result = this.Collection.Save<TObject>(doc, SafeMode.True);

            if (!result.Ok)
            {
                string errorMessage = result.HasLastErrorMessage ? result.LastErrorMessage : string.Empty;
                throw new ApplicationException(string.Format("Failed to save {0} to the database. Error Message: {1}", doc.ToJson(), errorMessage));
            }
        }

        public IEnumerable<TObject> GetAllItems()
        {
            var cursor = this.Collection.FindAllAs<TObject>();
            return cursor.AsEnumerable<TObject>();
        }

        public void Remove<HandleType>(HandleType handle) where HandleType : QuantaeObjectHandle<TObject>
        {
            this.Collection.Remove(Query.EQ("ObjectID", new BsonObjectId(handle.ObjectId.Value)));
        }

        public int CountItems()
        {
            return this.Collection.Count();
        }
    }
}
