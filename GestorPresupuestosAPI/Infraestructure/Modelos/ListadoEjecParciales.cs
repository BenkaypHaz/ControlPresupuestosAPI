using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestorPresupuestosAPI.Infraestructure.Modelos
{
    public class ListadoEjecParciales
    {
        public int id_ejecupar { get; set; }

        public int id_cuenta { get; set; }

        public int proveedor { get; set; }
        public string proveedorName { get; set; }

        [StringLength(500)]
        public string observaciones { get; set; }

        [Column(TypeName = "decimal(15, 2)")]
        public decimal cantidad { get; set; }
        [Column("fecha_compra")]
        [DataType(DataType.Date)]
        public DateTime fecha_compra { get; set; }
        [Column("Bloqueado")]
        public bool Bloqueado { get; set; }
        [Column("Fecha_Bloqueo")]
        [DataType(DataType.Date)]
        public DateTime? Fecha_Bloqueo { get; set; }
    }
}
