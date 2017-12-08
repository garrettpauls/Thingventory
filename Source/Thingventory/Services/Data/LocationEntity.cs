using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thingventory.Services.Data
{
    [Table("Location")]
    public sealed class LocationEntity
    {
        [Key, Column("Id"), Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Name"), Required]
        public string Name { get; set; }
    }
}
