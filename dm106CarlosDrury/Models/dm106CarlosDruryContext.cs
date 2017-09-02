using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace dm106CarlosDrury.Models
{
    public class dm106CarlosDruryContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public dm106CarlosDruryContext() : base("name=dm106CarlosDruryContext")
        {
        }

        public System.Data.Entity.DbSet<dm106CarlosDrury.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<dm106CarlosDrury.Models.Order> Orders { get; set; }
    }
}
