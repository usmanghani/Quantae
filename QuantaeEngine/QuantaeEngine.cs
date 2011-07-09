using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.Repositories;

namespace Quantae.Engine
{
    public class QuantaeEngine
    {
        public static void Init(string dbName)
        {
            DataStore dataStore = new DataStore(dbName);
            dataStore.Connect();
            Repositories.Repositories.Init(dataStore);
            FilterManager.CreateFilters();
        }
    }
}
