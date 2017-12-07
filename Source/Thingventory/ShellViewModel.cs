using Template10.Mvvm;
using Template10.Services.NavigationService;

namespace Thingventory
{
    public sealed class ShellViewModel : ViewModelBase
    {
        private readonly INavigationService mNavService;

        public ShellViewModel(INavigationService navService)
        {
            mNavService = navService;
        }
    }
}
