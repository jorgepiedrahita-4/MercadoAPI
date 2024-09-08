using MercadoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO ;
using System.Text.Json;

namespace MercadoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProductos()
        {
            string rutaArchivo = "./archivo.txt";
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
            string rutaArchivo = "./archivo.txt";
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
            string rutaArchivo = "./archivo.txt";
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
            string rutaArchivo = "./archivo.txt";
            var productos = System.IO.File.ReadAllLines(rutaArchivo).ToList();

            string nuevoProductoLinea = $"{nuevoProducto.Id},{nuevoProducto.Nombre},{nuevoProducto.Desripcion},{nuevoProducto.Precio},{nuevoProducto.Stock}";
            productos.Add(nuevoProductoLinea);

            System.IO.File.WriteAllLines(rutaArchivo, productos);

            return Ok("Producto insertado correctamente");
        }

    }


}
