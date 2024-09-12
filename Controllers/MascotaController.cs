using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TAREA___CRUD_CON_EF.Data;
using TAREA___CRUD_CON_EF.Models;
using TAREA___CRUD_CON_EF.ViewModel;

namespace TAREA___CRUD_CON_EF.Controllers
{
    public class MascotaController : Controller
    {
        private readonly ILogger<MascotaController> _logger;
        private readonly ApplicationDbContext _context;

        public MascotaController(ILogger<MascotaController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var viewModel = new MascotaVM
            {
                FormMascota = new Mascota(),
                ListMascota = _context.Mascotas.ToList()
            };
            ViewData["Accion"] = "Guardar";
            ViewData["Color"] = "primary";
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Insertar(MascotaVM viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Mascotas.Add(viewModel.FormMascota);
                    _context.SaveChanges();
                    TempData["Message"] = "Mascota insertada con éxito";
                    TempData["Color"] = "primary";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al insertar mascota");
                    ModelState.AddModelError("", "Ocurrió un error al insertar la mascota.");
                }
            }

            viewModel.ListMascota = _context.Mascotas.ToList();
            ViewData["Accion"] = "Guardar";
            return View("Index", viewModel);
        }

        [HttpGet]
        public IActionResult Editar(long id)
        {
            var mascota = _context.Mascotas.Find(id);
            if (mascota == null)
            {
                return NotFound();
            }

            var viewModel = new MascotaVM
            {
                FormMascota = mascota,
                ListMascota = _context.Mascotas.ToList()
            };
            ViewData["Accion"] = "Modificar";
            ViewData["Color"] = "warning";
            return View("Index", viewModel);
        }

        [HttpPost]
        public IActionResult Modificar(MascotaVM viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var mascotaExistente = _context.Mascotas.Find(viewModel.FormMascota.Id);
                    if (mascotaExistente != null)
                    {
                        _context.Entry(mascotaExistente).CurrentValues.SetValues(viewModel.FormMascota);
                        _context.SaveChanges();
                        TempData["Message"] = "Mascota modificada con éxito";
                        TempData["Color"] = "warning";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Mascota no encontrada");
                        TempData["Color"] = "danger";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al modificar mascota");
                    ModelState.AddModelError("", "Ocurrió un error al modificar la mascota.");
                }
            }

            viewModel.ListMascota = _context.Mascotas.ToList();
            ViewData["Accion"] = "Modificar";
            return View("Index", viewModel);
        }

        [HttpGet]
        public IActionResult Eliminar(long id)
        {
            var mascota = _context.Mascotas.Find(id);
            if (mascota != null)
            {
                _context.Mascotas.Remove(mascota);
                _context.SaveChanges();
                TempData["Message"] = "Mascota eliminada con éxito";
                TempData["Color"] = "danger";
            }
            else
            {
                TempData["Message"] = "Mascota no encontrada";
                TempData["Color"] = "danger"; 
            }

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}