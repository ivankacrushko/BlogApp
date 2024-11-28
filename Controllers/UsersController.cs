using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogApp.Models;
using BlogApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

public class UsersController : Controller
{
	private readonly ApplicationDbContext _context;
	private readonly UserManager<ApplicationUser> _userManager;

	public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
	{
		_context = context;
		_userManager = userManager;
	}

	[Authorize]
	public async Task<IActionResult> Index()
	{
		// Pobranie aktualnie zalogowanego użytkownika
		var user = await _userManager.GetUserAsync(User);
		if (user == null)
		{
			return RedirectToAction("Login", "Account");  // Jeśli użytkownik nie jest zalogowany, przekieruj na stronę logowania
		}

		var posts = await _context.Posts
			.Where(p => p.UserId == user.Id)  // Wybieramy posty przypisane do użytkownika
			.OrderByDescending(p => p.CreatedAt)  // Sortujemy posty po dacie utworzenia
			.ToListAsync();

		var profileViewModel = new ProfileViewModel
		{
			User = user,
			Posts = posts
		};

		return View(profileViewModel);  // Przekazanie użytkownika do widoku
	}
}
