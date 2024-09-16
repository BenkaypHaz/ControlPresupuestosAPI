namespace GestorPresupuestosAPI.Infraestructure.Modelos
{
    public class CuentaDetailDTO
    {
        public string DescripcionCuenta { get; set; }
        public List<PresupuestoCuentaDetail> Details { get; set; }
    }

    public class PresupuestoCuentaDetail
    {
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public string Comentarios { get; set; }
        public int IdCuenta { get; set; }   
        public bool Ejecutada { get; set; } 
        public bool EjecutadaParcial { get; set; }
        public bool Modificada { get; set; }
    }
}
