using System.IO;
using Windows.Storage;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Thingventory.Models;

namespace Thingventory.Services.Data
{
    public sealed class ThingDataContext : DbContext
    {
        private readonly string mFile;

        public ThingDataContext() : this("")
        {
        }

        public ThingDataContext(string rootPath)
        {
            mFile = Path.Combine(rootPath, "data.sqlite");
        }

        public DbSet<LocationEntity> Locations { get; set; }

        public static ThingDataContext Create(Inventory inventory)
        {
            var rootPath = Path.Combine(
                ApplicationData.Current.LocalFolder.Path,
                "Inventories",
                inventory.Id.ToString("N"));
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
