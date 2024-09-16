public class PresupuestoCuentaUpdateDTO
{
    public int IdCuentas { get; set; }
    public int IdCuenta { get; set; }
    public string Descripcion { get; set; }
    public decimal Cantidad { get; set; }
    public bool Modificada { get; set; }
    public int UsuModifica { get; set; }
}
