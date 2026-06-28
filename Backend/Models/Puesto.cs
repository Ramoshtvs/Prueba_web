using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Puesto
    {
        [Key]
        public int ID_Puesto { get; set; }
        public string Nombre_Puesto { get; set; } = null!;
    }
}
