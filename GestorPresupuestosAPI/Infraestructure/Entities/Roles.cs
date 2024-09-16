using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Roles")]
public class Roles
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_rol")]
    public int RolUsuId { get; set; }


    [Column("nombre")]
    [StringLength(200)]
    public string nombre { get; set; }
}
