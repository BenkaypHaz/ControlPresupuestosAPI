namespace GestorPresupuestosAPI.Infraestructure.Modelos
{
    public class PresupuestoWithCuentasDTO
    {
        public decimal cantidad {  get; set; } 
        public List<PresupuestoCuenta> Cuentas { get; set; }
    }

}
