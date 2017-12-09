namespace Thingventory.Core.Models
{
    public sealed class Location : ChangeTrackingModel
    {
        private string mName;

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
    }
}
