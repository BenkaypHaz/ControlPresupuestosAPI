using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestorPresupuestosAPI.Infraestructure.Entities
{
    [Table("Cuentas_Generales")]
    public class Cuentas_Generales
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_general")]
        public int IdCuenta { get; set; }

        [Required]
        [Column("nombre")]
        [StringLength(800)]
        public string nombre { get; set; }
    }
}
