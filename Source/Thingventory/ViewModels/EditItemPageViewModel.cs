using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Template10.Mvvm;
using Thingventory.Core.Models;
using Thingventory.Core.Services;
using Thingventory.Views.Dialogs;

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
        private const int NEW_ITEM_ID = -1;
        private readonly IItemService mItemService;
        private readonly ILocationService mLocationService;
        private readonly EditItemPagePayload mPayload;
        private string mHeaderText;
        private ItemDetails mItem;
        private Location mLocation;

        public EditItemPageViewModel(EditItemPagePayload payload, IItemService itemService, ILocationService locationService)
        {
            mPayload = payload;
            mItemService = itemService;
            mLocationService = locationService;

            CancelCommand = new DelegateCommand(_Cancel);
            SaveCommand = new DelegateCommand(_Save, () => Item?.HasChanges ?? false);
            SaveAndNewCommand = new DelegateCommand(_SaveAndAdd);
            UndoCommand = new DelegateCommand(_Undo, () => Item?.HasChanges ?? false);
        }

        public DelegateCommand CancelCommand { get; }

        public string HeaderText
        {
            get => mHeaderText;
            private set => Set(ref mHeaderText, value);
        }

        public ItemDetails Item
        {
            get => mItem;
            private set
            {
                if (mItem != value)
                {
                    if (mItem != null)
                    {
                        mItem.PropertyChanged -= _HandleItemPropertyChanged;
                    }

                    Set(ref mItem, value);

                    if (mItem != null)
                    {
                        mItem.PropertyChanged += _HandleItemPropertyChanged;
                        _HandleItemPropertyChanged(mItem, new PropertyChangedEventArgs(""));
                    }
                }
            }
        }

        public DelegateCommand SaveAndNewCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand UndoCommand { get; }

        private void _Cancel()
        {
            NavigationService.GoBack();
        }

        private void _HandleItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
            UndoCommand.RaiseCanExecuteChanged();
        }

        private void _NewItem()
        {
            HeaderText = $"Add item to {mLocation.Name}";

            var item = new ItemDetails(NEW_ITEM_ID)
            {
                LocationId = mPayload.LocationId
            };

            item.ResetHasChanges();
            Item = item;
        }

        private async void _Save()
        {
            await _SaveThen(() => NavigationService.GoBack());
        }

        private async void _SaveAndAdd()
        {
            await _SaveThen(_NewItem);
        }

        private async Task _SaveThen(Action next)
        {
            var result = Item.ValidateAll();
            if (result.IsValid)
            {
                await mItemService.SaveItemAsync(Item);
                next();
            }
            else
            {
                await new ValidationErrorDialog("The item cannot be saved due to the following errors:", result).ShowAsync();
            }
        }

        private async void _Undo()
        {
            if (Item.Id == NEW_ITEM_ID)
            {
                _NewItem();
            }
            else
            {
                Item = await mItemService.GetItemDetailsAsync(Item.Id);
            }
        }

        public void ClearAcquiredDate()
        {
            if (Item != null)
            {
                Item.AcquiredDate = null;
            }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            mLocation = await mLocationService.GetLocationAsync(mPayload.LocationId);

            if (mPayload.ItemId.HasValue)
            {
                HeaderText = $"Edit item in {mLocation.Name}";
                Item = await mItemService.GetItemDetailsAsync(mPayload.ItemId.Value);
            }
            else
            {
                _NewItem();
            }
        }
    }
}
