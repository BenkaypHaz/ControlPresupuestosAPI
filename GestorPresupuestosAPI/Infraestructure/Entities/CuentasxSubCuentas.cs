using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("CuentasxSubCuentas")]
public class CuentasxSubCuentas
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("id_cuenta")]
    public int IdCuenta { get; set; }

    [Required]
    [Column("id_subcuenta")]
    public int IdSubCuenta { get; set; }
}
