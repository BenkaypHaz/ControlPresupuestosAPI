using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Notificaciones")]
public class Notificaciones
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_noti")]
    public int IdNoti { get; set; }

    [Column("presupuesto_id")]
    public int PresupuestoId { get; set; }

    [Column("cuenta_id")]
    public int CuentaId { get; set; }

    [Column("modificacion")]
    public string Modificacion { get; set; }
    [Column("usu_presupuesto")]
    public int UsuPresupuesto { get; set; }
    [Column("usu_modifica")]
    public int UsuModifica { get; set; }

    [Column("comentario")]
    public string Comentario { get; set; }

    [Column("Leida")]
    [StringLength(250)]
    public bool Leida { get; set; }
}
