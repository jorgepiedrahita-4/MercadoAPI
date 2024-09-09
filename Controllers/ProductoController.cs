using MercadoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;

namespace MercadoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly string rutaArchivo = "./archivo.txt";

        [HttpGet]
        public IActionResult GetProductos()
        {
            if (!System.IO.File.Exists(rutaArchivo))
            {
                return NotFound("Archivo no encontrado");
            }

            string[] lineas = System.IO.File.ReadAllLines(rutaArchivo);
            List<Producto> productos = new List<Producto>();

            foreach (string linea in lineas)
            {
                string[] campos = linea.Split(',');
                Producto producto = new Producto
                {
                    Id = int.Parse(campos[0]),
                    Nombre = campos[1],
                    Desripcion = campos[2],
                    Precio = int.Parse(campos[3]),
                    Stock = int.Parse(campos[4])
                };
                productos.Add(producto);
            }

            return new JsonResult(productos);
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarProducto(int id)
        {
            if (!System.IO.File.Exists(rutaArchivo))
            {
                return NotFound("Archivo no encontrado");
            }

            var lineas = System.IO.File.ReadAllLines(rutaArchivo).ToList();
            var lineasFiltradas = lineas.Where(linea => int.Parse(linea.Split(',')[0]) != id).ToList();

            if (lineasFiltradas.Count == lineas.Count)
            {
                return NotFound("Producto no encontrado");
            }

            System.IO.File.WriteAllLines(rutaArchivo, lineasFiltradas);
            return Ok("Producto eliminado correctamente");
        }

        [HttpPut("{id}")]
        public IActionResult ActualizarProducto(int id, [FromBody] Producto productoActualizado)
        {
            if (!System.IO.File.Exists(rutaArchivo))
            {
                return NotFound("Archivo no encontrado");
            }

            var lineas = System.IO.File.ReadAllLines(rutaArchivo).ToList();

            for (int i = 0; i < lineas.Count; i++)
            {
                string[] campos = lineas[i].Split(',');
                if (int.Parse(campos[0]) == id)
                {
                    // Actualizar la línea con los nuevos datos del producto
                    lineas[i] = $"{productoActualizado.Id},{productoActualizado.Nombre},{productoActualizado.Desripcion},{productoActualizado.Precio},{productoActualizado.Stock}";
                    System.IO.File.WriteAllLines(rutaArchivo, lineas);
                    return Ok("Producto actualizado correctamente");
                }
            }

            return NotFound("Producto no encontrado");
        }

        [HttpPost]
        public IActionResult InsertarProducto([FromBody] Producto nuevoProducto)
        {
            if (!System.IO.File.Exists(rutaArchivo))
            {
                // Crear archivo si no existe
                System.IO.File.Create(rutaArchivo).Dispose();
            }

            var productos = System.IO.File.ReadAllLines(rutaArchivo).ToList();

            int nuevoId = ObtenerSiguienteId(productos);

            string nuevoProductoLinea = $"{nuevoId},{nuevoProducto.Nombre},{nuevoProducto.Desripcion},{nuevoProducto.Precio},{nuevoProducto.Stock}";
            productos.Add(nuevoProductoLinea);

            System.IO.File.WriteAllLines(rutaArchivo, productos);

            return Ok(new { Id = nuevoId, Mensaje = "Producto insertado correctamente" });
        }

        private int ObtenerSiguienteId(List<string> productos)
        {
            if (productos.Count == 0)
            {
                return 1;
            }

            var ids = productos.Select(linea => int.Parse(linea.Split(',')[0])).ToList();
            return ids.Max() + 1;
        }
    }
}
