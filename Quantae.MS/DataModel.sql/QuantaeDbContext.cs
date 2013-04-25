using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Quantae.DataModel.Sql
{
    public class QuantaeDbContext : DbContext
    {
        public DbSet<UserProfile> Users { get; set; }
    }
}
