using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thingventory.Core.Data
{
    [Table("Image")]
    public sealed class ImageEntity
    {
        [Column("CreatedInstant"), Required]
        public DateTimeOffset CreatedInstant { get; set; } = DateTimeOffset.Now;

        [Key, Column("Id"), Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("LocalFileName"), Required]
        public string LocalFileName { get; set; } = "";

        [Column("Name"), Required]
        public string Name { get; set; } = "";
    }
}
