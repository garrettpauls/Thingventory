using Windows.Storage;
using Windows.UI.Xaml.Data;

namespace Thingventory.Core.Models
{
    public sealed class ImageData : BindingBase
    {
        private readonly StorageFile mFile;

        public ImageData(StorageFile file)
        {
            mFile = file;
        }
    }
}
