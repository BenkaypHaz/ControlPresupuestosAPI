using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Items_Ejecutados")]
public class ItemsEjecutados
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id_ejecu { get; set; }

    public int id_cuenta { get; set; }


    public int proveedor { get; set; }

    [StringLength(500)]
    public string observaciones { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal cantidad_final { get; set; }
    [Column("fecha_compra")]
    [DataType(DataType.Date)]
    public DateTime fecha_compra { get; set; }
    [Column("Activo")]
    public bool Activo { get; set; }

    [Column("EsIngreso")]
    public bool EsIngreso { get; set; }
}
