namespace GestorPresupuestosAPI.Infraestructure.Modelos
{
    public class PresupuestoSummaryDTO
    {
        public int Ejecutadas { get; set; }
        public int NoEjecutadas { get; set; }
        public decimal Presupuesto { get; set; }
        public decimal Disponible { get; set; }
        public decimal Gastado { get; set; }
        public bool ingresoExtra { get; set; }  
    }

}
