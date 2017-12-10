using Template10.Mvvm;

namespace Thingventory.Core.Models
{
    public sealed class ItemSummary : BindableBase
    {
        public ItemSummary(int id, string name, int locationId)
        {
            Id = id;
            Name = name;
            LocationId = locationId;
        }

        public int Id { get; }
        public int LocationId { get; }
        public string Name { get; }
    }
}
