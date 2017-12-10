using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thingventory.Core.Data
{
    [Table("Item")]
    public sealed class ItemEntity
    {
        [Column("AcquiredDate")]
        public DateTimeOffset? AcquiredDate { get; set; }

        [Column("AcquiredFrom"), Required]
        public string AcquiredFrom { get; set; } = "";

        [Column("Comments"), Required]
        public string Comments { get; set; } = "";

        [Column("CreatedInstant"), Required]
        public DateTimeOffset CreatedInstant { get; set; } = DateTimeOffset.Now;

        [Key, Column("Id"), Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("LocationId"), Required]
        public int LocationId { get; set; }

        [Column("Name"), Required]
        public string Name { get; set; } = "";

        [Column("Quantity"), Required]
        public uint Quantity { get; set; } = 1;

        [Column("UpdatedInstant"), Required]
        public DateTimeOffset UpdatedInstant { get; set; } = DateTimeOffset.Now;

        [Column("Value")]
        public decimal? Value { get; set; }
    }
}
