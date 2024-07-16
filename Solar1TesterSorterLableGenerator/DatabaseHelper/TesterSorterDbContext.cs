using Microsoft.EntityFrameworkCore;
using Solar1TesterSorterLableGenerator.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.Extensions;

namespace Solar1TesterSorterLableGenerator.DatabaseHelper
{
    public class TesterSorterDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationSettings.AppSettings["DatabaseQueryString"]);
        }

        public DbSet<BinRecipeModel> Bs_BinRecipeModel { get; set; }
        public DbSet<NewTesterSorterPackingModel> Bs_NewTesterSorterPacking { get; set; }
    }
}
