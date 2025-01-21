using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestorPresupuestosAPI.Infraestructure.Modelos
{
    public class CuentaxSubCuentasDTO
    {

        public int Id { get; set; }


        public int IdCuenta { get; set; }

        public int IdSubCuenta { get; set; }

        public string Cuenta {  get; set; } 
        public string SubCuenta { get; set; }   
    }
}
