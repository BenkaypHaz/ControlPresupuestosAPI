using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("subcategoria")]
public class Subcategorias
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_subcate")]
    public int IdSubcate { get; set; }

    [Required]
    [Column("nombre")]
    [StringLength(200)]
    public string Nombre { get; set; }

    [Column("categoria_id")]
    public int CategoriaId { get; set; }

}
