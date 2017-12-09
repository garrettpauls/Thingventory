using Template10.Mvvm;

namespace Thingventory.ViewModels
{
    public sealed class EditItemPagePayload
    {
        public EditItemPagePayload(int locationId, int? itemId)
        {
            LocationId = locationId;
            ItemId = itemId;
        }

        public int? ItemId { get; }
        public int LocationId { get; }
    }

    public sealed class EditItemPageViewModel : ViewModelBase
    {
        private readonly EditItemPagePayload mPayload;

        public EditItemPageViewModel(EditItemPagePayload payload)
        {
            mPayload = payload;
        }
    }
}
