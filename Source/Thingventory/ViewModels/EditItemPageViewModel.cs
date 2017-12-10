using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Template10.Mvvm;
using Thingventory.Core.Models;
using Thingventory.Core.Services;

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
        private readonly IItemService mItemService;
        private readonly ILocationService mLocationService;
        private readonly EditItemPagePayload mPayload;
        private string mHeaderText;
        private ItemDetails mItem;

        public EditItemPageViewModel(EditItemPagePayload payload, IItemService itemService, ILocationService locationService)
        {
            mPayload = payload;
            mItemService = itemService;
            mLocationService = locationService;

            SaveCommand = new DelegateCommand(_Save);
        }

        public string HeaderText
        {
            get => mHeaderText;
            private set => Set(ref mHeaderText, value);
        }

        public ItemDetails Item
        {
            get => mItem;
            private set => Set(ref mItem, value);
        }

        public DelegateCommand SaveCommand { get; }

        private async void _Save()
        {
            var result = Item.ValidateAll();
            if (result.IsValid)
            {
                await mItemService.SaveItemAsync(Item);
                NavigationService.GoBack();
            }
            else
            {
                // TODO: show validation error
            }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var location = await mLocationService.GetLocationAsync(mPayload.LocationId);
            HeaderText = mPayload.ItemId.HasValue ? $"Edit item in {location.Name}" : $"Add item to {location.Name}";

            if (mPayload.ItemId.HasValue)
            {
                Item = await mItemService.GetItemDetailsAsync(mPayload.ItemId.Value);
            }
            else
            {
                var item = new ItemDetails(-1)
                {
                    LocationId = mPayload.LocationId
                };

                item.ResetHasChanges();
                Item = item;
            }
        }
    }
}
