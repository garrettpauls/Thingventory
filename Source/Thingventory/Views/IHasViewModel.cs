using Template10.Services.NavigationService;

namespace Thingventory.Views
{
    public interface IHasViewModel<TViewModel>
        where TViewModel : INavigable
    {
        TViewModel ViewModel { get; }
    }
}
