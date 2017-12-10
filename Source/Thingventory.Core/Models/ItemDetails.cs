using System;

namespace Thingventory.Core.Models
{
    public sealed class ItemDetails : ChangeTrackingModel
    {
        private DateTimeOffset? mAcquiredDate;
        private string mAcquiredFrom = "";
        private string mComments = "";
        private DateTimeOffset mCreatedInstant;
        private int mLocationId;
        private string mName = "";
        private uint mQuantity = 1;
        private DateTimeOffset mUpdatedInstant;
        private decimal? mValue;

        public ItemDetails(int id)
        {
            Id = id;
            mCreatedInstant = mUpdatedInstant = DateTimeOffset.Now;
        }

        public DateTimeOffset? AcquiredDate
        {
            get => mAcquiredDate;
            set => Set(ref mAcquiredDate, value);
        }

        public string AcquiredFrom
        {
            get => mAcquiredFrom;
            set => Set(ref mAcquiredFrom, value);
        }

        public string Comments
        {
            get => mComments;
            set => Set(ref mComments, value);
        }

        public DateTimeOffset CreatedInstant
        {
            get => mCreatedInstant;
            set => Set(ref mCreatedInstant, value);
        }

        public int Id { get; }

        public int LocationId
        {
            get => mLocationId;
            set => Set(ref mLocationId, value);
        }

        public string Name
        {
            get => mName;
            set => Set(ref mName, value);
        }

        public uint Quantity
        {
            get => mQuantity;
            set => Set(ref mQuantity, value);
        }

        public DateTimeOffset UpdatedInstant
        {
            get => mUpdatedInstant;
            set => Set(ref mUpdatedInstant, value);
        }

        public decimal? Value
        {
            get => mValue;
            set => Set(ref mValue, value);
        }
    }
}
