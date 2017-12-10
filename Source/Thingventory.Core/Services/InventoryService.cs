using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Common.Logging;
using Microsoft.EntityFrameworkCore;
using Thingventory.Core.Data;
using Thingventory.Core.Models;

namespace Thingventory.Core.Services
{
    public interface IInventoryService
    {
        Task<Inventory> CreateInventoryAsync(string name);
        Task<Inventory> GetCurrentInventoryAsync();
        Task MigrateInventoryAsync(Inventory inventory);
        Task SaveInventoryAsync(Inventory inventory);
    }

    public sealed class InventoryService : IInventoryService, IService
    {
        private readonly Func<Inventory, ILocationService> mLocationServiceFactory;
        private readonly ILog mLog;
        private readonly IAsyncOperation<StorageFolder> mRootFolder;

        public InventoryService(Func<Inventory, ILocationService> locationServiceFactory, ILog log)
        {
            mLocationServiceFactory = locationServiceFactory;
            mLog = log;
            mRootFolder = ApplicationData.Current.LocalFolder.CreateFolderAsync("Inventories", CreationCollisionOption.OpenIfExists);
        }

        public async Task<Inventory> CreateInventoryAsync(string name)
        {
            var id = Guid.NewGuid();

            var inventory = new Inventory(id)
            {
                Name = name
            };

            await SaveInventoryAsync(inventory);
            await MigrateInventoryAsync(inventory);

            var loc = mLocationServiceFactory(inventory);

            await loc.CreateLocationAsync("Kitchen");
            await loc.CreateLocationAsync("Library");
            await loc.CreateLocationAsync("Living room");
            await loc.CreateLocationAsync("Office");

            return inventory;
        }

        public async Task<Inventory> GetCurrentInventoryAsync()
        {
            var root = await mRootFolder;
            var folders = await root.GetFoldersAsync();
            foreach (var invFolder in folders.OrderByDescending(f => f.DateCreated))
            {
                var inv = await _TryLoadInventoryAsync(invFolder);
                if (inv != null)
                {
                    return inv;
                }
            }

            return await CreateInventoryAsync("default");
        }

        public async Task MigrateInventoryAsync(Inventory inventory)
        {
            using (var context = ThingDataContext.Create(inventory))
            {
                try
                {
                    await context.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    mLog.Error($"Failed to migrate inventory: {inventory.Id}", ex);
                    throw;
                }
            }
        }

        public async Task SaveInventoryAsync(Inventory inventory)
        {
            var root = await mRootFolder;
            var folder = await root.CreateFolderAsync(inventory.Id.ToString("N"), CreationCollisionOption.OpenIfExists);
            var file = await folder.CreateFileAsync("inventory.xml", CreationCollisionOption.OpenIfExists);

            var serializer = new DataContractSerializer(typeof(Inventory));
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                serializer.WriteObject(stream, inventory);
                await stream.FlushAsync();
            }
        }

        private async Task<Inventory> _TryLoadInventoryAsync(IStorageFolder folder)
        {
            try
            {
                var file = await folder.GetFileAsync("inventory.xml");
                var serializer = new DataContractSerializer(typeof(Inventory));

                using (var stream = await file.OpenStreamForReadAsync())
                {
                    return (Inventory) serializer.ReadObject(stream);
                }
            }
            catch (Exception ex)
            {
                mLog.Warn($"Failed to load inventory from folder: {folder.Path}", ex);
                return null;
            }
        }
    }
}
