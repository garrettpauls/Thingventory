using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Template10.Utils;
using Thingventory.Core.Models;
using Thingventory.Core.Services;
using Thingventory.Views;
using Thingventory.Views.Dialogs;

namespace Thingventory.ViewModels
{
    public sealed class HomePageViewModel : ViewModelBase
    {
        private readonly IItemService mItemService;
        private readonly ILocationService mLocationService;

        public HomePageViewModel(ILocationService locationService, IItemService itemService)
        {
            mLocationService = locationService;
            mItemService = itemService;

            AddLocationCommand = new DelegateCommand(_AddLocation);
        }

        public DelegateCommand AddLocationCommand { get; }

        public ObservableCollection<HomePageLocationViewModel> Locations { get; } = new ObservableCollection<HomePageLocationViewModel>();

        private async void _AddLocation()
        {
            var dialog = new RenameLocationDialog()
            {
                Title = "Create location",
                PrimaryButtonText = "Add location"
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var loc = await mLocationService.CreateLocationAsync(dialog.Location.Name, dialog.Location.Notes);
                Locations.Add(new HomePageLocationViewModel(loc, mLocationService, mItemService, NavigationService));
            }
        }

        public async Task DeleteLocationAsync(HomePageLocationViewModel vm)
        {
            var dialog = new ContentDialog
            {
                Title = $"Delete {vm.Location.Name}",
                Content = $"Are you sure you want to delete location {vm.Location.Name} and all items it contains? This action can not be undone.",
                PrimaryButtonText = "Delete",
                SecondaryButtonText = "Keep",
                DefaultButton = ContentDialogButton.Secondary
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                Locations.Remove(vm);
                await mLocationService.DeleteLocationAsync(vm.Location.Id).ConfigureAwait(false);
            }
        }

        public void EditItem(ItemSummary item)
        {
            NavigationService.Navigate(typeof(EditItemPage), new EditItemPagePayload(item.LocationId, item.Id));
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var locations = await mLocationService.GetLocationsAsync();
            foreach (var location in locations.OrderBy(item => item.Name))
            {
                var vm = new HomePageLocationViewModel(location, mLocationService, mItemService, NavigationService);
                await vm.InitializeAsync();
                Locations.Add(vm);
            }
        }
    }

    public sealed class HomePageLocationViewModel : BindableBase
    {
        private readonly IItemService mItemService;
        private readonly ILocationService mLocationService;
        private readonly INavigationService mNavService;

        public HomePageLocationViewModel(
            Location location, ILocationService locationService,
            IItemService itemService, INavigationService navService)
        {
            Location = location;
            mLocationService = locationService;
            mItemService = itemService;
            mNavService = navService;
        }

        public ObservableCollection<ItemSummary> Items { get; } = new ObservableCollection<ItemSummary>();
        public Location Location { get; }

        public Task AddItemAsync()
        {
            return mNavService.NavigateAsync(typeof(EditItemPage), new EditItemPagePayload(Location.Id, null));
        }

        public async Task InitializeAsync()
        {
            var items = await mItemService.GetItemListForLocation(Location.Id);
            Items.AddRange(items.OrderBy(item => item.Name));
        }

        public async Task RenameAsync()
        {
            var dialog = new RenameLocationDialog(Location);

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                await mLocationService.SaveLocationAsync(Location);
            }
        }
    }
}
