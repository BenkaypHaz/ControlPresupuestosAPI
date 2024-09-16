using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Cuentas")]
public class Cuenta
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_cuenta")]
    public int IdCuenta { get; set; }

    [Required]
    [Column("Descripcion")]
    [StringLength(150)]
    public string Descripcions { get; set; }

}
