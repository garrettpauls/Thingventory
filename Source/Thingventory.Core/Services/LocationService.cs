using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Thingventory.Core.Data;
using Thingventory.Core.Models;

namespace Thingventory.Core.Services
{
    public interface ILocationService
    {
        Task<Location> CreateLocationAsync(string name, string notes = "");
        Task<Location> GetLocationAsync(int id);
        Task<Location[]> GetLocationsAsync();
        Task SaveLocationAsync(Location location);
    }

    public sealed class LocationService : InventoryDataAccessBase, ILocationService, IService
    {
        public LocationService(Inventory inventory) : base(inventory)
        {
        }

        public async Task<Location> CreateLocationAsync(string name, string notes = "")
        {
            using (var context = GetContext())
            {
                var entity = new LocationEntity
                {
                    Name = name,
                    Notes = notes ?? ""
                };

                context.Locations.Add(entity);
                await context.SaveChangesAsync();

                return _Translate(entity);
            }
        }

        public async Task<Location> GetLocationAsync(int id)
        {
            using (var context = GetContext())
            {
                var entity = await context.Locations.FirstAsync(loc => loc.Id == id);
                return _Translate(entity);
            }
        }

        public async Task<Location[]> GetLocationsAsync()
        {
            using (var context = GetContext())
            {
                return await context
                    .Locations
                    .ToAsyncEnumerable()
                    .Select(_Translate)
                    .ToArray();
            }
        }

        public async Task SaveLocationAsync(Location location)
        {
            using (var context = GetContext())
            {
                var entity = await context.Locations.FirstOrDefaultAsync(loc => loc.Id == location.Id);
                if (entity != null)
                {
                    entity.Name = location.Name;
                    entity.Notes = location.Notes;

                    await context.SaveChangesAsync();
                }
            }
        }

        private Location _Translate(LocationEntity entity) => new Location(entity.Id)
        {
            Name = entity.Name,
            Notes = entity.Notes
        };
    }
}
