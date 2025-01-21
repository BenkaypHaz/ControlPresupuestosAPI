using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("versiones")]
public class Versiones
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("numero_version")]
    [StringLength(50)]
    public string NumeroVersion { get; set; }

    [Column("descripcion")]
    [StringLength(150)]
    public string Descripcion { get; set; }

    [Column("fecha")]
    public DateTime Fecha { get; set; }
}
