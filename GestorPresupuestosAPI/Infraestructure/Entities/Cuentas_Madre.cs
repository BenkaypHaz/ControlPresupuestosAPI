using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Cuentas_Madre")]
public class Cuentas_Madre
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_madre")]
    public int IdCuenta { get; set; }

    [Required]
    [Column("nombre")]
    [StringLength(150)]
    public string Descripcions { get; set; }

}
