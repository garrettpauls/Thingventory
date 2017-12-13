using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Thingventory.Core.Models;

namespace Thingventory.Core.Data
{
    public sealed class ThingDataContext : DbContext
    {
        private readonly string mFile;

        public ThingDataContext() : this("data.sqlite")
        {
        }

        public ThingDataContext(string file)
        {
            mFile = file;
        }

        public DbSet<ImageEntity> Images { get; set; }
        public DbSet<ItemEntity> Items { get; set; }
        public DbSet<LocationEntity> Locations { get; set; }

        public static ThingDataContext Create(Inventory inventory)
        {
            var rootPath = Path.Combine(inventory.RootDataPath, "data.sqlite");
            return new ThingDataContext(rootPath);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connection = new SqliteConnectionStringBuilder();
            connection.DataSource = mFile;

            optionsBuilder.UseSqlite(connection.ToString());
        }
    }
}
