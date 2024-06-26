using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestorPresupuestosAPI.Infraestructure.Entities
{
    public class Proveedores
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_prov")]
        public int id_prov { get; set; }

        [Column("descripcion")]
        [StringLength(250)]
        public string descripcion { get; set; }
    }
}
