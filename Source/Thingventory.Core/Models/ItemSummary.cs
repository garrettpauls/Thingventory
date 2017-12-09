using Template10.Mvvm;

namespace Thingventory.Core.Models
{
    public sealed class ItemSummary : BindableBase
    {
        public ItemSummary(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}
