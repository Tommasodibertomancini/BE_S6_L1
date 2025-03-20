using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BE_S6_L1.Models;
using BE_S6_L1.ViewModels;
using System.Threading.Tasks;


namespace BE_S6_L1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        // Aggiungi questo metodo per visualizzare i ruoli disponibili nella vista
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            var viewModel = new RegisterViewModel
            {
                RoliDisponibili = new List<string> { "Studente", "Docente" }
            };
            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Nome = model.Nome,
                    Cognome = model.Cognome
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Utente creato con successo.");


                    string ruolo = model.Ruolo ?? "Studente";

                    _logger.LogInformation($"Tentativo di assegnare il ruolo {ruolo} all'utente {user.Email}");

                    if (await _roleManager.RoleExistsAsync(ruolo))
                    {
                        var roleResult = await _userManager.AddToRoleAsync(user, ruolo);
                        if (roleResult.Succeeded)
                        {
                            _logger.LogInformation($"Assegnato ruolo {ruolo} all'utente {user.Email}");
                        }
                        else
                        {
                            _logger.LogError($"Errore durante l'assegnazione del ruolo: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"Il ruolo {ruolo} non esiste nel database");
                    }


                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Studenti");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError($"Errore durante la creazione dell'utente: {error.Description}");
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            
            model.RoliDisponibili = new List<string> { "Studente", "Docente" };
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Utente loggato con successo.");
                    return RedirectToLocal(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Account bloccato.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tentativo di accesso non valido.");
                    return View(model);
                }
            }

            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Utente disconnesso.");
            return RedirectToAction("Index", "Studenti");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Studenti");
            }
        }
    }
}