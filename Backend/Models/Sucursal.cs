using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Sucursal
    {
        [Key]
        public int ID_Sucursal { get; set; }
        public string Nombre_Sucursal { get; set; } = null!;
    }
}
