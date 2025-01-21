using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Presupuesto_cuentas")]
public class PresupuestoCuenta
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_cuentas")]
    public int IdCuentas { get; set; }

    [Required]
    [Column("id_presu")]
    public int IdPresu { get; set; } 

    [Required]
    [Column("id_cuenta")]
    public int IdCuenta { get; set; } 

    [Column("descripcion")]
    public string Descripcion { get; set; }

    [Column("cantidad")]
    [DataType(DataType.Currency)]
    public decimal Cantidad { get; set; }

    [Column("Comentarios")]
    public string Comentarios { get; set; }

    [Column("Ejecutada")]
    public bool Ejecutada { get; set; }
    [Column("EjecucionParcial")]
    public bool EjecucionParcial { get; set; }
    [Column("Activo")]
    public bool Activo { get; set; }
    [Column("Modificada")]
    public bool Modificada { get; set; }
    [Column("EsIngreso")]
    public bool EsIngreso { get; set; }



}
