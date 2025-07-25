using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingLot.Dto;
using ParkingLot.Models;

namespace ParkingLot.Controllers
{
    public class LogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PriceCalculatorService _calculator;

        public LogController(AppDbContext context)
        {
            _context = context;
            _calculator = new PriceCalculatorService(_context);
        }

        public IActionResult DailyReport(DateTime? date)
        {
            var selectedDate = date ?? DateTime.Today;

            var logs = _context.Logs
                .Include(l => l.Subscription)
                .Where(l => l.CheckInTime.Date == selectedDate.Date || l.CheckOutTime.Date == selectedDate.Date)
                .ToList();

            ViewData["SelectedDate"] = selectedDate.ToString("dd/MM/yyyy");
            return View("DailyReport", logs);
        }

        [HttpPost]
        [Route("api/log/manual")]
        public IActionResult CreateManualLog([FromBody] ManualLogDto dto)
        {
            if (dto == null || dto.CheckInTime >= dto.CheckOutTime)
                return BadRequest("Check-in/Check-out të pavlefshme");

            var subscription = _context.Subscriptions
                .Include(s => s.Subscriber)
                .FirstOrDefault(s => s.PlateNumber == dto.PlateNumber && !s.IsDeleted);

            var isSubscribed = subscription != null;
            var log = new Log
            {
                PlateNumber = dto.PlateNumber,
                CheckInTime = dto.CheckInTime,
                CheckOutTime = dto.CheckOutTime,
                Subscription = subscription,
                Price = isSubscribed ? 0 : _calculator.CalculateManual(dto.CheckInTime, dto.CheckOutTime),
                Code = Guid.NewGuid().ToString()
            };

            _context.Logs.Add(log);
            _context.SaveChanges();

            return Ok(log);
        }
        [HttpPost]
        [Route("api/log")]
        public IActionResult CreateLog([FromBody] Log log)
        {
            if (log == null || log.CheckInTime >= log.CheckOutTime)
                return BadRequest("Invalid check-in/check-out time");

            if (_context.Logs.Any(l => l.Code == log.Code))
                return BadRequest("Log with this code already exists");

            log.Price = _calculator.CalculatePrice(log);
            _context.Logs.Add(log);
            _context.SaveChanges();

            return Ok(log);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddDailyLog(string PlateNumber, DateTime CheckInTime, DateTime CheckOutTime)
        {
            if (CheckInTime >= CheckOutTime)
            {
                TempData["Error"] = "Ora e Check-In duhet të jetë më e hershme se Check-Out.";
                return RedirectToAction("DailyReport", new { date = DateTime.Today });
            }

            var subscription = _context.Subscriptions
                .Include(s => s.Subscriber)
                .FirstOrDefault(s => s.PlateNumber == PlateNumber && !s.IsDeleted);

            var isSubscribed = subscription != null;

            var log = new Log
            {
                PlateNumber = PlateNumber,
                CheckInTime = CheckInTime,
                CheckOutTime = CheckOutTime,
                Subscription = subscription,
                Price = isSubscribed ? 0 : _calculator.CalculateManual(CheckInTime, CheckOutTime),
                Code = Guid.NewGuid().ToString()
            };

            _context.Logs.Add(log);
            _context.SaveChanges();

            TempData["Success"] = "Hyrja u shtua me sukses.";
            return RedirectToAction("DailyReport", new { date = DateTime.Today });
        }
        [HttpGet]
        [Route("api/log/date/{date}")]
        public IActionResult GetLogsByDate(DateTime date)
        {
            var logs = _context.Logs
                .Include(l => l.Subscription)
                .Where(l => l.CheckInTime.Date == date.Date)
                .ToList();

            return Ok(logs);
        }

        [HttpGet]
        [Route("api/log/search")]
        public IActionResult SearchLogs(string term)
        {
            var logs = _context.Logs
                .Include(l => l.Subscription)
                    .ThenInclude(s => s.Subscriber)
                .Where(l => l.Code.Contains(term) ||
                            (l.Subscription != null &&
                             (l.Subscription.Subscriber.FirstName.Contains(term) ||
                              l.Subscription.Subscriber.LastName.Contains(term))))
                .ToList();

            return Ok(logs);
        }

        [HttpDelete]
        [Route("api/log/{id}")]
        public IActionResult DeleteLog(int id)
        {
            var log = _context.Logs.FirstOrDefault(l => l.Id == id);
            if (log == null) return NotFound();

            _context.Logs.Remove(log);
            _context.SaveChanges();

            return NoContent();
        }
    }
}