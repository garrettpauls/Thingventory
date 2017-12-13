using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Core;
using Common.Logging;
using Microsoft.EntityFrameworkCore;
using Thingventory.Core.Data;
using Thingventory.Core.Models;
using Thingventory.Core.Models.Collections;

namespace Thingventory.Core.Services
{
    public interface IImageService
    {
        Task<ImageData> AddImageAsync(StorageFile file);
        IncrementalLoadingCollection<ImageData> GetAllImages();
    }

    public sealed class ImageService : InventoryDataAccessBase, IImageService, IService
    {
        private readonly ILog mLog;

        public ImageService(Inventory inventory, ILog log) : base(inventory)
        {
            mLog = log;
        }

        public async Task<ImageData> AddImageAsync(StorageFile file)
        {
            var imageFolder = await _GetImageFolderAsync();

            using (var context = GetContext())
            {
                var entity = new ImageEntity
                {
                    CreatedInstant = DateTimeOffset.Now,
                    Name = file.DisplayName
                };
                context.Images.Add(entity);
                await context.SaveChangesAsync();

                try
                {
                    var desiredNewName = $"{entity.Id}{Path.GetExtension(file.Name)}";
                    var localFile = await file.CopyAsync(imageFolder, desiredNewName, NameCollisionOption.GenerateUniqueName);

                    entity.LocalFileName = localFile.Name;
                    await context.SaveChangesAsync();

                    return new ImageData(localFile);
                }
                catch (Exception ex)
                {
                    mLog.Error($"Failed to add image from file: {file.Path}", ex);

                    context.Images.Remove(entity);
                    await context.SaveChangesAsync();

                    return null;
                }
            }
        }

        public IncrementalLoadingCollection<ImageData> GetAllImages()
        {
            var dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            return new IncrementalLoadingCollection<ImageData>(_LoadImagesAsync, dispatcher);
        }

        private async Task<StorageFolder> _GetImageFolderAsync()
        {
            var rootFolder = await Inventory.OpenDataFolderAsync();
            var imageFolder = await rootFolder.CreateFolderAsync("Images", CreationCollisionOption.OpenIfExists);
            return imageFolder;
        }

        private async Task<ImageData[]> _LoadImagesAsync(uint offset, uint countToLoad)
        {
            var imageFolder = await _GetImageFolderAsync();

            ImageEntity[] items;

            using (var context = GetContext())
            {
                items = await context
                    .Images
                    .OrderByDescending(img => img.CreatedInstant).ThenBy(img => img.Id)
                    .Skip((int) offset).Take((int) countToLoad)
                    .ToArrayAsync();
            }

            var imageData = new List<ImageData>();

            foreach (var item in items)
            {
                var file = await imageFolder.GetFileAsync(item.LocalFileName);
                imageData.Add(new ImageData(file));
            }

            return imageData.ToArray();
        }
    }
}
