using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LIBWebApplication1;

namespace LIBWebApplication1.Controllers
{
    public class GameRecordsController : Controller
    {
        private readonly DblibContext _context;

        public GameRecordsController(DblibContext context)
        {
            _context = context;
        }

        // GET: GameRecords
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if(id == null) return RedirectToAction("Index", "Tournaments");

            ViewBag.TournamentId = id;
            ViewBag.TournamentName = name;
            var gameRecordsByTournament = _context.GameRecords.Where(g => g.TournamentId == id).Include(g=>g.Tournament).Include(t1 => t1.Team1).Include(t2 => t2.Team2);
            return View(await gameRecordsByTournament.ToListAsync());
        }

        // GET: GameRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GameRecords == null)
            {
                return NotFound();
            }

            var gameRecord = await _context.GameRecords
                .Include(g => g.Team1)
                .Include(g => g.Team2)
                .Include(g => g.Tournament)
                .FirstOrDefaultAsync(m => m.GameRecordId == id);
            if (gameRecord == null)
            {
                return NotFound();
            }

            return View(gameRecord);
        }

        // GET: GameRecords/Create
        public IActionResult Create()
        {
            ViewData["Team1Id"] = new SelectList(_context.Teams, "TeamId", "TeamId");
            ViewData["Team2Id"] = new SelectList(_context.Teams, "TeamId", "TeamId");
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "TournamentId", "TournamentId");
            return View();
        }

        // POST: GameRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameRecordId,Team1Id,Team2Id,GameDate,Winner,Info,TournamentId")] GameRecord gameRecord)
        {
            if (ModelState.IsValid || true)
            {
                _context.Add(gameRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Team1Id"] = new SelectList(_context.Teams, "TeamId", "TeamId", gameRecord.Team1Id);
            ViewData["Team2Id"] = new SelectList(_context.Teams, "TeamId", "TeamId", gameRecord.Team2Id);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "TournamentId", "TournamentId", gameRecord.TournamentId);
            return View(gameRecord);
        }

        // GET: GameRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GameRecords == null)
            {
                return NotFound();
            }

            var gameRecord = await _context.GameRecords.FindAsync(id);
            if (gameRecord == null)
            {
                return NotFound();
            }
            ViewData["Team1Id"] = new SelectList(_context.Teams, "TeamId", "TeamId", gameRecord.Team1Id);
            ViewData["Team2Id"] = new SelectList(_context.Teams, "TeamId", "TeamId", gameRecord.Team2Id);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "TournamentId", "TournamentId", gameRecord.TournamentId);
            return View(gameRecord);
        }

        // POST: GameRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameRecordId,Team1Id,Team2Id,GameDate,Winner,Info,TournamentId")] GameRecord gameRecord)
        {
            if (id != gameRecord.GameRecordId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameRecordExists(gameRecord.GameRecordId))
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
            ViewData["Team1Id"] = new SelectList(_context.Teams, "TeamId", "TeamId", gameRecord.Team1Id);
            ViewData["Team2Id"] = new SelectList(_context.Teams, "TeamId", "TeamId", gameRecord.Team2Id);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "TournamentId", "TournamentId", gameRecord.TournamentId);
            return View(gameRecord);
        }

        // GET: GameRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GameRecords == null)
            {
                return NotFound();
            }

            var gameRecord = await _context.GameRecords
                .Include(g => g.Team1)
                .Include(g => g.Team2)
                .Include(g => g.Tournament)
                .FirstOrDefaultAsync(m => m.GameRecordId == id);
            if (gameRecord == null)
            {
                return NotFound();
            }

            return View(gameRecord);
        }

        // POST: GameRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GameRecords == null)
            {
                return Problem("Entity set 'DblibContext.GameRecords'  is null.");
            }
            var gameRecord = await _context.GameRecords.FindAsync(id);
            if (gameRecord != null)
            {
                _context.GameRecords.Remove(gameRecord);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameRecordExists(int id)
        {
          return (_context.GameRecords?.Any(e => e.GameRecordId == id)).GetValueOrDefault();
        }
    }
}
