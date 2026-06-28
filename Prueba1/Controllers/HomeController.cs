using Microsoft.AspNetCore.Mvc;
using Prueba1.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;

namespace Prueba1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient; 
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            // Inicializamos el HttpClient aquí mismo
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7275/");
        }

        public async Task<IActionResult> Index()
        {            
            ViewBag.PuestoSeleccionado = TempData["PuestoSeleccionado"] as int?;

            var listaS = new List<Sucursal>();
            var listaP = new List<Puesto>();

            HttpResponseMessage responsepuesto = await _httpClient.GetAsync("WeatherForecast/GetPuesto");
            HttpResponseMessage responsesucursal = await _httpClient.GetAsync("WeatherForecast/GetSucursal");

            if (responsepuesto.IsSuccessStatusCode)
            {
                string jsonString = await responsepuesto.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                listaP = JsonSerializer.Deserialize<List<Puesto>>(jsonString, opciones) ?? new List<Puesto>();
            }
            if (responsesucursal.IsSuccessStatusCode)
            {
                string jsonString = await responsesucursal.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                listaS = JsonSerializer.Deserialize<List<Sucursal>>(jsonString, opciones) ?? new List<Sucursal>();
            }

            ViewBag.ListaPuestos = listaP;
            ViewBag.ListaSucursal = listaS;

           var listaE = new List<Empleados>();

            if (TempData["EmpleadosFiltrados"] is string jsonEmpleados)
            {
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                listaE = JsonSerializer.Deserialize<List<Empleados>>(jsonEmpleados, opciones) ?? new List<Empleados>();
            }
            else
            {
                HttpResponseMessage responseEmpleados = await _httpClient.GetAsync("WeatherForecast/GetEmpleados?puesto=");
                if (responseEmpleados.IsSuccessStatusCode)
                {
                    string jsonString = await responseEmpleados.Content.ReadAsStringAsync();
                    var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    listaE = JsonSerializer.Deserialize<List<Empleados>>(jsonString, opciones) ?? new List<Empleados>();
                }
            }

           
            return View(listaE);
        }

        [HttpPost]
        public async Task<IActionResult> BuscarByIDAsync(int? puestoID)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"WeatherForecast/GetEmpleados?puesto={puestoID}");

            var listaE = new List<Empleados>();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                listaE = JsonSerializer.Deserialize<List<Empleados>>(jsonString, opciones) ?? new List<Empleados>();
            }
                        
            TempData["EmpleadosFiltrados"] = JsonSerializer.Serialize(listaE);
            TempData["PuestoSeleccionado"] = puestoID;

            return RedirectToAction("Index");

        }


        [HttpPost]
        public async Task<IActionResult> CrearEmpleado(Empleados nuevoEmpleado)
        {
            
            nuevoEmpleado.Nombre_Puesto = string.Empty;
            nuevoEmpleado.Nombre_Sucursal = string.Empty;

            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            string jsonEmpleado = JsonSerializer.Serialize(nuevoEmpleado, opciones);

            var contenido = new StringContent(jsonEmpleado, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("WeatherForecast/CrearEmpleado", contenido);

            if (response.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "Empleado guardado correctamente.";
            }
            else
            {
                TempData["MensajeError"] = "Hubo un problema al intentar guardar el empleado.";
            }

            return RedirectToAction("Index");
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
