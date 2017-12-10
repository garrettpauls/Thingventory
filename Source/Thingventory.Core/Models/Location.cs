namespace Thingventory.Core.Models
{
    public sealed class Location : ChangeTrackingModel
    {
        private string mName;
        private string mNotes;

        public Location(int id)
        {
            Id = id;
        }

        public int Id { get; }

        public string Name
        {
            get => mName;
            set => Set(ref mName, value);
        }

        public string Notes
        {
            get => mNotes;
            set => Set(ref mNotes, value);
        }
    }
}
