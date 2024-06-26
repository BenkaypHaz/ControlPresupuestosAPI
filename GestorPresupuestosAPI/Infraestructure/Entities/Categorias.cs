using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("categorias")]
public class Categorias
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_cat")]
    public int IdCat { get; set; }

    [Required]
    [Column("nombre")]
    [StringLength(200)]
    public string Nombre { get; set; }

    [Column("activo")]
    public bool Activo { get; set; } = true;
}
