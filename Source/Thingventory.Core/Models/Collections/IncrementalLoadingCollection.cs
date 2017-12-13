using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Data;

namespace Thingventory.Core.Models.Collections
{
    public delegate Task<TItem[]> LoadMoreItems<TItem>(uint offset, uint countToLoad);

    public sealed class IncrementalLoadingCollection<TItem> : ObservableCollection<TItem>, ISupportIncrementalLoading
    {
        private readonly CoreDispatcher mDispatcher;
        private readonly LoadMoreItems<TItem> mLoadMoreItems;
        private bool mHasMoreItems;
        private uint mOffset = 0;

        public IncrementalLoadingCollection(LoadMoreItems<TItem> loadMoreItems, CoreDispatcher dispatcher)
        {
            mLoadMoreItems = loadMoreItems;
            mDispatcher = dispatcher;
        }

        public bool HasMoreItems
        {
            get => mHasMoreItems;
            private set
            {
                if (mHasMoreItems != value)
                {
                    mHasMoreItems = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(HasMoreItems)));
                }
            }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count) => _LoadMoreItemsAsync(count).AsAsyncOperation();

        private async Task<LoadMoreItemsResult> _LoadMoreItemsAsync(uint count)
        {
            var items = await mLoadMoreItems(mOffset, count);
            mOffset += (uint) items.Length;

            await mDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                foreach (var item in items)
                {
                    base.InsertItem(Count, item);
                }

                if (count < items.Length)
                {
                    HasMoreItems = true;
                }
            });

            return new LoadMoreItemsResult
            {
                Count = (uint) items.Length
            };
        }

        protected override void ClearItems()
        {
            base.ClearItems();

            mOffset = 0;
            HasMoreItems = true;
        }

        protected override void InsertItem(int index, TItem item)
        {
            throw new NotSupportedException($"{GetType().Name} does not support directly adding items, instead call {nameof(LoadMoreItemsAsync)} to load more items.");
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            throw new NotSupportedException($"{GetType().Name} does not support moving items.");
        }

        protected override void RemoveItem(int index)
        {
            throw new NotSupportedException($"{GetType().Name} does not support removing individual items, instead the collection should be cleared.");
        }

        protected override void SetItem(int index, TItem item)
        {
            throw new NotSupportedException($"{GetType().Name} does not support setting items.");
        }
    }
}
