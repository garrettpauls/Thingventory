using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;

namespace Thingventory.Core.Models
{
    [DataContract(Name = "Inventory", Namespace = "Thingventory")]
    public sealed class Inventory
    {
        private const string INVENTORIES_FOLDER_NAME = "Inventories";

        public Inventory(Guid id)
        {
            Id = id;
        }

        [DataMember(Name = "Id")]
        public Guid Id { get; private set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [IgnoreDataMember]
        public string RootDataPath => Path.Combine(ApplicationData.Current.LocalFolder.Path, INVENTORIES_FOLDER_NAME, Id.ToString("N"));

        public async Task<StorageFolder> OpenDataFolderAsync()
        {
            var invFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(INVENTORIES_FOLDER_NAME, CreationCollisionOption.OpenIfExists);
            var dataFolder = await invFolder.CreateFolderAsync(Id.ToString("N"), CreationCollisionOption.OpenIfExists);
            return dataFolder;
        }
    }
}
