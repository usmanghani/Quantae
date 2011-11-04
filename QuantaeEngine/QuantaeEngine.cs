using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.Repositories;

namespace Quantae.Engine
{
    public class QuantaeEngine
    {
        public static void Init(string dbName, string url = "")
        {
            Uri uri = null;
            if (!string.IsNullOrEmpty(url))
            {
                uri = new Uri(url);
            }

            DataStore dataStore = new DataStore(dbName, uri);
            dataStore.Connect();
            Repositories.Repositories.Init(dataStore);
            FilterManager.CreateFilters();

            Repositories.Repositories.Vocabulary.EnsureIndex(new string[] { "Text" });
            Repositories.Repositories.GrammarEntries.EnsureIndex(new string[] { "Text" });
            Repositories.Repositories.GrammarRoles.EnsureIndex(new string[] { "RoleName" });
            Repositories.Repositories.Sentences.EnsureIndex(new string[] { "SentenceText" });

        }
    }
}
