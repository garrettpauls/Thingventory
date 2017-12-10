using System;
using System.Runtime.Serialization;

namespace Thingventory.Core.Models
{
    [DataContract(Name = "Inventory", Namespace = "Thingventory")]
    public sealed class Inventory
    {
        public Inventory(Guid id)
        {
            Id = id;
        }

        [DataMember(Name = "Id")]
        public Guid Id { get; private set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }
    }
}
