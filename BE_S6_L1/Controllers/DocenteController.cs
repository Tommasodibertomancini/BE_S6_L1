using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BE_S6_L1.Data;
using BE_S6_L1.Models;
using System.Threading.Tasks;

namespace BE_S6_L1.Controllers
{
    [Authorize(Roles = "Docente,Admin")]
    public class DocenteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DocenteController> _logger;

        public DocenteController(ApplicationDbContext context, ILogger<DocenteController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Docente
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Accesso all'area docenti");

            // Recupera tutti gli studenti per il docente
            var studenti = await _context.Studenti.ToListAsync();
            return View(studenti);
        }

        // GET: Docente/Valutazioni
        public async Task<IActionResult> Valutazioni()
        {
            var studenti = await _context.Studenti.ToListAsync();
            return View(studenti);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssegnaVoto(int id, int voto, string materia)
        {
            var studente = await _context.Studenti.FindAsync(id);
            if (studente == null)
            {
                return NotFound();
            }

           
            TempData["Message"] = $"Voto {voto} assegnato a {studente.Nome} {studente.Cognome} per {materia}";

            return RedirectToAction(nameof(Valutazioni));
        }
    }
}
