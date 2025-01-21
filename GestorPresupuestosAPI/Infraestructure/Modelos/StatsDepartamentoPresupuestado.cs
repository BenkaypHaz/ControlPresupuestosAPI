namespace GestorPresupuestosAPI.Infraestructure.Modelos
{
    public class StatsDepartamentoPresupuestado
    {
        public string Departamento { get; set; }
        public decimal TotalAsignado { get; set; }
        public decimal TotalEjecutado { get; set; }
        public decimal TotalDepto { get; set; }
    }
}
