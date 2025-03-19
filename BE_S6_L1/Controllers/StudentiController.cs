using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BE_S6_L1.Data;
using BE_S6_L1.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace BE_S6_L1.Controllers
{
    [Authorize]
    public class StudentiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StudentiController> _logger;

        public StudentiController(ApplicationDbContext context, ILogger<StudentiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Studenti
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Visualizzazione dell'elenco completo degli studenti");
            return View(await _context.Studenti.ToListAsync());
        }

        // GET: Studenti/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Tentativo di visualizzare i dettagli di uno studente con ID nullo");
                return NotFound();
            }

            var studente = await _context.Studenti
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studente == null)
            {
                _logger.LogWarning("Studente con ID {StudenteId} non trovato", id);
                return NotFound();
            }

            _logger.LogInformation("Visualizzazione dei dettagli dello studente con ID {StudenteId}", id);
            return View(studente);
        }

        // GET: Studenti/GetStudentiPartial
        public async Task<IActionResult> GetStudentiPartial()
        {
            _logger.LogDebug("Caricamento parziale dell'elenco studenti");
            var studenti = await _context.Studenti.ToListAsync();
            return PartialView("_StudentiPartial", studenti);
        }

        // GET: Studenti/GetStudenteDetails/5
        public async Task<IActionResult> GetStudenteDetails(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Tentativo di caricare i dettagli di uno studente con ID nullo");
                return NotFound();
            }

            var studente = await _context.Studenti
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studente == null)
            {
                _logger.LogWarning("Studente con ID {StudenteId} non trovato durante il caricamento dei dettagli", id);
                return NotFound();
            }

            _logger.LogDebug("Caricamento parziale dei dettagli dello studente con ID {StudenteId}", id);
            return PartialView("_DetailPartial", studente);
        }

        // GET: Studenti/Create
        public IActionResult Create()
        {
            _logger.LogDebug("Richiesta del form per la creazione di un nuovo studente");
            return PartialView("_CreatePartial");
        }

        // POST: Studenti/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Cognome,DataDiNascita,Email")] Studente studente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(studente);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Nuovo studente creato: {Nome} {Cognome} con ID {StudenteId}",
                                      studente.Nome, studente.Cognome, studente.Id);

                    var studenti = await _context.Studenti.ToListAsync();
                    return PartialView("_StudentiPartial", studenti);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Errore durante la creazione dello studente {Nome} {Cognome}",
                               studente.Nome, studente.Cognome);
                    ModelState.AddModelError("", "Si è verificato un errore durante il salvataggio. Riprova.");
                }
            }
            else
            {
                _logger.LogWarning("Validazione fallita durante la creazione di un nuovo studente");
            }

            return PartialView("_CreatePartial", studente);
        }

        // GET: Studenti/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Tentativo di modificare uno studente con ID nullo");
                return NotFound();
            }

            var studente = await _context.Studenti.FindAsync(id);
            if (studente == null)
            {
                _logger.LogWarning("Studente con ID {StudenteId} non trovato durante la modifica", id);
                return NotFound();
            }

            _logger.LogDebug("Apertura del form di modifica per lo studente con ID {StudenteId}", id);
            return PartialView("_EditPartial", studente);
        }

        // POST: Studenti/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Cognome,DataDiNascita,Email")] Studente studente)
        {
            if (id != studente.Id)
            {
                _logger.LogWarning("ID non corrispondente durante la modifica: {RequestId} vs {ModelId}", id, studente.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studente);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Studente aggiornato: ID {StudenteId} - {Nome} {Cognome}",
                                      studente.Id, studente.Nome, studente.Cognome);

                    return await GetStudenteDetails(id);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!StudenteExists(studente.Id))
                    {
                        _logger.LogWarning("Studente con ID {StudenteId} non trovato durante l'aggiornamento", studente.Id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Errore di concorrenza durante l'aggiornamento dello studente con ID {StudenteId}", studente.Id);
                        ModelState.AddModelError("", "Si è verificato un errore di concorrenza. Un altro utente ha modificato questo record.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Errore durante l'aggiornamento dello studente con ID {StudenteId}", studente.Id);
                    ModelState.AddModelError("", "Si è verificato un errore durante il salvataggio. Riprova.");
                }
            }
            else
            {
                _logger.LogWarning("Validazione fallita durante l'aggiornamento dello studente con ID {StudenteId}", id);
            }

            return PartialView("_EditPartial", studente);
        }

        // POST: Studenti/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var studente = await _context.Studenti.FindAsync(id);
            if (studente == null)
            {
                _logger.LogWarning("Tentativo di eliminare uno studente inesistente con ID {StudenteId}", id);
                return Json(new { success = false, message = "Studente non trovato" });
            }

            try
            {
                _context.Studenti.Remove(studente);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Studente eliminato: ID {StudenteId} - {Nome} {Cognome}",
                                  id, studente.Nome, studente.Cognome);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'eliminazione dello studente con ID {StudenteId}", id);
                return Json(new { success = false, message = "Si è verificato un errore durante l'eliminazione" });
            }
        }

       
        private bool StudenteExists(int id)
        {
            return _context.Studenti.Any(e => e.Id == id);
        }
    }
}
