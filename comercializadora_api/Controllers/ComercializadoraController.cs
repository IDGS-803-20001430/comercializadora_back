using comercializadora_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using comercializadora_api.Models; // Asegúrate de ajustar el espacio de nombres según tus modelos

namespace comercializadora_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosCategoriasController : ControllerBase
    {
        private readonly ComercializadoraContext _contexto;

        public ProductosCategoriasController(ComercializadoraContext contexto)
        {
            _contexto = contexto;
        }

        // Métodos para Productos

        [HttpGet]
        [Route("ListaProductos")]
        public async Task<IActionResult> ListaProductos()
        {
            var listaProductos = await _contexto.Productos.ToListAsync();
            return Ok(listaProductos);
        }

        [HttpGet]
        [Route("ProductosPorCategoria")]
        public async Task<IActionResult> ProductosPorCategoria()
        {
            var productosPorCategoria = await _contexto.Productos
                .GroupBy(p => p.IdCategoria)
                .Select(g => g.FirstOrDefault())
                .ToListAsync();

            return Ok(productosPorCategoria);
        }

        [HttpPost]
        [Route("AgregarProducto")]
        public async Task<IActionResult> AgregarProducto([FromBody] Producto producto)
        {
            await _contexto.Productos.AddAsync(producto);
            await _contexto.SaveChangesAsync();
            return Ok(producto);
        }

        [HttpPut]
        [Route("ModificarProducto/{id}")]
        public async Task<IActionResult> ModificarProducto(int id, [FromBody] Producto producto)
        {
            var productoModificar = await _contexto.Productos.FindAsync(id);

            if (productoModificar == null)
            {
                return BadRequest("El producto no existe");
            }

            productoModificar.Nombre = producto.Nombre;
            productoModificar.Descripcion = producto.Descripcion;
            productoModificar.Precio = producto.Precio;
            productoModificar.IdCategoria = producto.IdCategoria;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
                {
                    return NotFound("El producto no se encontró para actualizar");
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [HttpDelete]
        [Route("EliminarProducto/{id}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var productoEliminar = await _contexto.Productos.FindAsync(id);

            if (productoEliminar == null)
            {
                return BadRequest("El producto no existe");
            }

            _contexto.Productos.Remove(productoEliminar);
            await _contexto.SaveChangesAsync();
            return Ok();
        }

        // Métodos para Categorías

        [HttpGet]
        [Route("ListaCategorias")]
        public async Task<IActionResult> ListaCategorias()
        {
            var listaCategorias = await _contexto.Categorias.ToListAsync();
            return Ok(listaCategorias);
        }

        [HttpPost]
        [Route("AgregarCategoria")]
        public async Task<IActionResult> AgregarCategoria([FromBody] Categoria categoria)
        {
            await _contexto.Categorias.AddAsync(categoria);
            await _contexto.SaveChangesAsync();
            return Ok(categoria);
        }

        [HttpPut]
        [Route("ModificarCategoria/{id}")]
        public async Task<IActionResult> ModificarCategoria(int id, [FromBody] Categoria categoria)
        {
            var categoriaModificar = await _contexto.Categorias.FindAsync(id);

            if (categoriaModificar == null)
            {
                return BadRequest("La categoría no existe");
            }

            categoriaModificar.Nombre = categoria.Nombre;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
                {
                    return NotFound("La categoría no se encontró para actualizar");
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [HttpDelete]
        [Route("EliminarCategoria/{id}")]
        public async Task<IActionResult> EliminarCategoria(int id)
        {
            var categoriaEliminar = await _contexto.Categorias.FindAsync(id);

            if (categoriaEliminar == null)
            {
                return BadRequest("La categoría no existe");
            }

            _contexto.Categorias.Remove(categoriaEliminar);
            await _contexto.SaveChangesAsync();
            return Ok();
        }

        private bool ProductoExists(int id)
        {
            return _contexto.Productos.Any(e => e.IdProducto == id);
        }

        private bool CategoriaExists(int id)
        {
            return _contexto.Categorias.Any(e => e.IdCategoria == id);
        }
    }
}
