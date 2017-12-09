using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Microsoft.Services.Store.Engagement;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Thingventory.Core.Services;

namespace Thingventory
{
    public sealed class ShellViewModel : BindableBase
    {
        private readonly INavigationService mNavService;

        private Visibility mFeedbackVisibility;

        public ShellViewModel(INavigationService navService)
        {
            mNavService = navService;

            if (!StoreServicesFeedbackLauncher.IsSupported())
            {
                FeedbackVisibility = Visibility.Collapsed;
            }
        }

        public Visibility FeedbackVisibility
        {
            get => mFeedbackVisibility;
            private set => Set(ref mFeedbackVisibility, value);
        }

        private IDictionary<string, string> _GetFeedbackData()
        {
            var data = new Dictionary<string, string>();
            var content = mNavService.Content as FrameworkElement;

            if (content?.DataContext is IFeedbackProvider feedbackProvider)
            {
                feedbackProvider.PopulateFeedbackData(data);
            }

            data["Page"] = content?.GetType().FullName;

            return data;
        }

        public async void ShowFeedbackAsync()
        {
            if (StoreServicesFeedbackLauncher.IsSupported())
            {
                var feedbackData = _GetFeedbackData();
                var launcher = StoreServicesFeedbackLauncher.GetDefault();
                await launcher.LaunchAsync(feedbackData);
            }
        }
    }
}
