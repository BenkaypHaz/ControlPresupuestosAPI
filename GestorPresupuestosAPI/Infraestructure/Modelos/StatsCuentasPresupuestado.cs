namespace GestorPresupuestosAPI.Infraestructure.Modelos
{
    public class StatsCuentasPresupuestado
    {
        public string Descripcion { get; set; }
        public decimal total_cantidad { get; set; }
        public decimal total_ejecutado { get; set; }
        public decimal total_restante { get; set; }

    }
}
