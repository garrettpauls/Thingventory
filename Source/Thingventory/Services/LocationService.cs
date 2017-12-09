using System.Linq;
using System.Threading.Tasks;
using Thingventory.Core.Data;
using Thingventory.Core.Models;

namespace Thingventory.Services
{
    public interface ILocationService
    {
        Task<Location> CreateLocationAsync(string name);
        Task<Location[]> GetLocationsAsync();
    }

    public sealed class LocationService : ILocationService, IService
    {
        private readonly Inventory mInventory;

        public LocationService(Inventory inventory)
        {
            mInventory = inventory;
        }

        public async Task<Location> CreateLocationAsync(string name)
        {
            using (var context = _GetContextAsync())
            {
                var entity = new LocationEntity
                {
                    Name = name
                };

                context.Locations.Add(entity);
                await context.SaveChangesAsync();

                return _Translate(entity);
            }
        }

        public async Task<Location[]> GetLocationsAsync()
        {
            using (var context = _GetContextAsync())
            {
                return await context
                    .Locations
                    .ToAsyncEnumerable()
                    .Select(_Translate)
                    .ToArray();
            }
        }

        private ThingDataContext _GetContextAsync()
        {
            return ThingDataContext.Create(mInventory);
        }

        private Location _Translate(LocationEntity entity) => new Location(entity.Id) {Name = entity.Name};
    }
}
