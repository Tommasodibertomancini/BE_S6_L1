using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BE_S6_L1.Data;
using BE_S6_L1.Models;
using System.Linq;
using System.Threading.Tasks;


namespace BE_S6_L1.Controllers
{
    public class StudentiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentiController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public async Task<IActionResult> Index()
        {
            return View(await _context.Studenti.ToListAsync());
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studente = await _context.Studenti
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studente == null)
            {
                return NotFound();
            }

            return View(studente);
        }

        
        public async Task<IActionResult> GetStudentiPartial()
        {
            var studenti = await _context.Studenti.ToListAsync();
            return PartialView("_StudentiPartial", studenti);
        }

        
        public async Task<IActionResult> GetStudenteDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studente = await _context.Studenti
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studente == null)
            {
                return NotFound();
            }

            return PartialView("_DetailPartial", studente);
        }
    }
}
