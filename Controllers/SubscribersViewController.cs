using Microsoft.AspNetCore.Mvc;
using ParkingLot.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

[ApiController]
[Route("[controller]/[action]")]
public class SubscribersViewController : Controller
{
    private readonly AppDbContext _context;

    public SubscribersViewController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Produces("text/html")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Index()
    {
        var subscribers = _context.Subscribers
            .Where(s => !s.IsDeleted)
            .ToList();

        return View(subscribers); 
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public IActionResult Create([FromBody] Subscriber model)
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine("❌ ModelState INVALID");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(" - " + error.ErrorMessage);
            }
            return BadRequest(ModelState);
        }

        bool exists = _context.Subscribers
            .Any(s => s.IdCardNumber == model.IdCardNumber && !s.IsDeleted);

        if (exists)
        {
            Console.WriteLine("⚠️ Abonenti ekziston me ID: " + model.IdCardNumber);
            return Conflict("Ky ID Card ekziston në sistem.");
        }

        model.IsDeleted = false;
        _context.Subscribers.Add(model);

        try
        {
            int rows = _context.SaveChanges();
            Console.WriteLine($"✅ U ruajt me sukses. Rows affected: {rows}");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine("🔥 Gabim gjatë ruajtjes: " + ex.Message);
            return StatusCode(500, "Gabim gjatë ruajtjes.");
        }

        return Ok(model);
    }
    [HttpPost]
    public IActionResult Delete(int id)
    {
        var subscriber = _context.Subscribers.FirstOrDefault(s => s.Id == id && !s.IsDeleted);

        if (subscriber == null)
            return NotFound();

        subscriber.IsDeleted = true;
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}