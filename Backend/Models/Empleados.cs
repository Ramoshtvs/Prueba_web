using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Empleados
    {
        [Key]
        public int ID_Empleado { get; set; }
        public string Nombre_Empleado { get; set; }
        public int Edad { get; set; }
        public string Sexo { get; set; }      
        public int ID_Sucursal { get; set; }

        [ForeignKey("ID_Sucursal")]
        public virtual Sucursal? IDSucursal { get; set; } = null!;

        public int ID_Puesto { get; set; }

        [ForeignKey("ID_Puesto")]
        public virtual Puesto? IDPuesto { get; set; } = null!;

    }
}
