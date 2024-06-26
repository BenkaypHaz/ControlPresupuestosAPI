using GestorPresupuestosAPI.Infraestructure.Modelos;

public class PresupuestoCuentaInfoDTO
{
    public int IdCuentas { get; set; }
    public int IdPresu { get; set; }
    public int IdCuenta { get; set; }
    public string Descripcion { get; set; }
    public decimal Cantidad { get; set; }
    public string Comentarios { get; set; }
    public bool Ejecutada { get; set; }
    public bool EjecucionParcial { get; set; }
    public decimal TotalSumado { get; set; }
    public DateTime fecha_compra {  get; set; }  

    public List<ListadoEjecParciales> EjecucionesParciales {  get; set; }  
}
