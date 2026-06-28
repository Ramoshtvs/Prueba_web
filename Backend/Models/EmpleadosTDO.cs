namespace Backend.Models
{
    public class EmpleadosTDO
    {

        public int ID_Empleado { get; set; }
        public string Nombre_Empleado { get; set; }
        public int Edad { get; set; }
        public string Sexo { get; set; }
        public int ID_Sucursal { get; set; }
        public string Nombre_Sucursal { get; set; } = string.Empty;
        public int ID_Puesto { get; set; }
        public string Nombre_Puesto { get; set; } = string.Empty;
    }
}
