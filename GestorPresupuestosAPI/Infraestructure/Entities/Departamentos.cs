using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Departamentos")]
public class Departamentos
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_departamento")]
    public int IdDepartamento { get; set; }

    [Required]
    [Column("nombre")]
    [StringLength(200)]
    public string Nombre { get; set; }

}
