using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Template10.Mvvm;
using Template10.Utils;
using Thingventory.Core.Models;
using Thingventory.Core.Services;

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
                var vm = new HomePageLocationViewModel(location, mItemService);
                await vm.InitializeAsync();
                Locations.Add(vm);
            }
        }
    }

    public sealed class HomePageLocationViewModel : BindableBase
    {
        private readonly IItemService mItemService;

        public HomePageLocationViewModel(Location location, IItemService itemService)
        {
            Location = location;
            mItemService = itemService;
        }
        
        public ObservableCollection<ItemSummary> Items { get; } = new ObservableCollection<ItemSummary>();
        public Location Location { get; }

        public async Task InitializeAsync()
        {
            var items = await mItemService.GetItemListForLocation(Location.Id);
            Items.AddRange(items.OrderBy(item => item.Name));
        }
    }
}
