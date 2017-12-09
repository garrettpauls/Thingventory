using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thingventory.Core.Data
{
    [Table("Item")]
    public sealed class ItemEntity
    {
        [Column("CreatedInstant"), Required]
        public DateTimeOffset CreatedInstant { get; set; }

        [Key, Column("Id"), Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("LocationId"), Required]
        public int LocationId { get; set; }

        [Column("Name"), Required]
        public string Name { get; set; }

        [Column("UpdatedInstant"), Required]
        public DateTimeOffset UpdatedInstant { get; set; }
    }
}
