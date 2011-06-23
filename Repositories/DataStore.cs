using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace Repositories
{
    public class DataStore
    {
        private MongoServer server = null;
        public Uri Uri { get; set; }

        public DataStore()
        {
            this.server = MongoServer.Create();
        }

        public DataStore (Uri uri)
        {
            this.Uri = uri;
            this.server = MongoServer.Create(uri);
        }

        public void Connect()
        {
            this.server.Connect();
        }

        public void Disconnect()
        {
            this.server.Disconnect();
        }
    }
}
