using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestorPresupuestosAPI.Infraestructure.Entities
{
    public class Tipo_presupuesto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("idtip")]
        public int idtip { get; set; }

        [Required]
        [Column("descripcion")]
        [StringLength(200)]
        public string descripcion { get; set; }

    }
}
