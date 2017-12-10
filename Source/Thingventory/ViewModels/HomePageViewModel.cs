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
        }

        public ObservableCollection<HomePageLocationViewModel> Locations { get; } = new ObservableCollection<HomePageLocationViewModel>();

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
            var dialog = new RenameLocationDialog
            {
                LocationName = Location.Name,
                LocationNotes = Location.Notes
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                if (string.IsNullOrWhiteSpace(dialog.LocationName))
                {
                    // TODO: show validation error
                }
                else
                {
                    Location.Name = dialog.LocationName;
                    Location.Notes = dialog.LocationNotes;

                    await mLocationService.SaveLocationAsync(Location);
                }
            }
        }
    }
}
