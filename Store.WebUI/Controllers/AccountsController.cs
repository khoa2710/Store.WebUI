using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Store.WebUI.Entities;
using Store.WebUI.Models.SubmitModel;

namespace Store.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterSubmitModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                AvataUrl = model.AvataUrl
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok(new { message = "Registered successfully" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginSubmitModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            var ok = await _userManager.CheckPasswordAsync(user, model.Password);
            if (ok == false)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            var token = GenerateJwtToken(user);
            return Ok(new { access_token = token });
        }
        //=================================================================

        [HttpGet("/Account/Register")] //link
        public IActionResult RegisterForm()
        {
            return View("Register");
        }

        [HttpPost("/Account/Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterForm(RegisterSubmitModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View("Register", model);
            }

            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                AvataUrl = model.AvataUrl
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("Register", model);
        }

        [HttpGet("/Account/Login")]
        public IActionResult LoginForm()
        {
            return View("Login");
        }

        [HttpPost("/Account/Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginForm(LoginSubmitModel model, string? returnUrl = null)
        {
            if (ModelState.IsValid == false)
            {
                return View("Login", model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.UserName, model.Password, false, false);

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl ?? "/");
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View("Login", model);
        }


        private string GenerateJwtToken(AppUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(
                          Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                           double.Parse(jwtSettings["ExpiryMinutes"]!)),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
