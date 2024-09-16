using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("RolesUsuario")]
public class RolesUsuario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("rolusu_Id")]
    public int RolUsuId { get; set; }

    [Column("usu_Id")]
    public int UsuId { get; set; }

    [Column("rol_Id")]
    public int RolId { get; set; }
}
