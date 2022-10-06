using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrabajoPractico.Models;

namespace TrabajoPractico.Controllers
{
    public class LibrosController : Controller
    {
        private readonly appDBcontext _context;
        private readonly IWebHostEnvironment env;

        public LibrosController(IWebHostEnvironment env)
        {
            _context = new appDBcontext();
            this.env = env;
        }

        // GET: Libros
        public async Task<IActionResult> Index()
        {
            var appDBcontext = _context.libros.Include(l => l.autor).Include(l => l.genero);
            return View(await appDBcontext.ToListAsync());
        }

        // GET: Libros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.libros == null)
            {
                return NotFound();
            }

            var libro = await _context.libros
                .Include(l => l.autor)
                .Include(l => l.genero)
                .FirstOrDefaultAsync(m => m.id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libros/Create
        public IActionResult Create()
        {
            ViewData["autorid"] = new SelectList(_context.autores, "id", "id");
            ViewData["autornombre"] = new SelectList(_context.autores, "nombre", "nombre");
            ViewData["generoid"] = new SelectList(_context.generos, "id", "id");
            ViewData["generodescripcion"] = new SelectList(_context.generos, "descripcion", "descripcion");
            return View();
        }

        // POST: Libros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,titulo,resumen,foto,fechaPublicacion,generoid,autorid")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count > 0)
                {
                    var archivoFoto = archivos[0];
                    var pathDestino = Path.Combine(env.WebRootPath, "images");
                    if (archivoFoto.Length > 0)
                    {
                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoFoto.FileName);

                        using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
                        {
                            archivoFoto.CopyTo(filestream);
                            libro.foto = archivoDestino;
                        };

                    }
                }
                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["autorid"] = new SelectList(_context.autores, "id", "id", libro.autorid);
            ViewData["generoid"] = new SelectList(_context.generos, "id", "id", libro.generoid);
            return View(libro);
        }

        // GET: Libros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.libros == null)
            {
                return NotFound();
            }

            var libro = await _context.libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            ViewData["autorid"] = new SelectList(_context.autores, "id", "id", libro.autorid);
            ViewData["generoid"] = new SelectList(_context.generos, "id", "id", libro.generoid);
            return View(libro);
        }

        // POST: Libros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,titulo,resumen,foto,fechaPublicacion,generoid,autorid")] Libro libro)
        {
            if (id != libro.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["autorid"] = new SelectList(_context.autores, "id", "id", libro.autorid);
            ViewData["generoid"] = new SelectList(_context.generos, "id", "id", libro.generoid);
            return View(libro);
        }

        // GET: Libros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.libros == null)
            {
                return NotFound();
            }

            var libro = await _context.libros
                .Include(l => l.autor)
                .Include(l => l.genero)
                .FirstOrDefaultAsync(m => m.id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.libros == null)
            {
                return Problem("Entity set 'appDBcontext.libros'  is null.");
            }
            var libro = await _context.libros.FindAsync(id);
            if (libro != null)
            {
                _context.libros.Remove(libro);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibroExists(int id)
        {
          return _context.libros.Any(e => e.id == id);
        }
    }
}
