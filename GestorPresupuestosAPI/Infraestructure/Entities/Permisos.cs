using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Permisos")]
public class Permisos
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_perm")]
    public int IdPerm { get; set; }

    [Column("roleId")]
    public int? RoleId { get; set; }

    [Column("Permiso")]
    [StringLength(255)]
    public string Permiso { get; set; }
}
