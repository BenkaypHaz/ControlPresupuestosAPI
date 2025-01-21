namespace GestorPresupuestosAPI.Infraestructure.Modelos
{
    public class StatPresupuestoIndividualDTO
    {
        public decimal TotalEjecutado { get; set; }
        public decimal TotalRestante { get; set; }
        public int NoEjecutadas { get; set; }
        public int Ejecutadas { get; set; }
    }
}
