using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
[Table("GeneralesxMadre")]
public class GeneralesxMadre
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("id_general")]
    public int IdGeneral { get; set; }
    [Column("id_madre")]
    public int IdMadre { get; set; }
}
