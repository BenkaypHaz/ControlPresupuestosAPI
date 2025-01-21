using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Presupuesto")]
public class PresupuestoDepa
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_presu")]
    public int IdPresu { get; set; }

    [Column("nombre")]
    [StringLength(250)]
    public string nombre { get; set; }
    [Column("cantidad")]
    [DataType(DataType.Currency)]
    public decimal Cantidad { get; set; }
    public string tipo_presupuesto { get; set; }
    [Column("estado")]
    public int estado { get; set; }
    [Column("fecha_creacion")]
    [DataType(DataType.Date)]
    public DateTime FechaCreacion { get; set; }

    [Column("Activo")]
    public bool Activo { get; set; } = true;
    [Column("usu_crea")]
    public int usu_crea { get; set; }
    public string Departamento { get; set; }
    public int Anio_presupuesto { get; set; }
}
