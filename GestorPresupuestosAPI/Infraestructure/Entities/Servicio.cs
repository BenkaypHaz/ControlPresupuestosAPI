using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestorPresupuestosAPI.Infraestructure.Entities
{
    [Table("Servicios")]
    public class Servicio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_servi")]
        public int IdServi { get; set; }

        [Required]
        [Column("nombre")]
        [StringLength(205)]
        public string Nombre { get; set; }
    }
}