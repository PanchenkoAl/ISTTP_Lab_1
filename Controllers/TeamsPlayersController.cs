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
    public class TeamsPlayersController : Controller
    {
        private readonly DblibContext _context;

        public TeamsPlayersController(DblibContext context)
        {
            _context = context;
        }

        // GET: TeamsPlayers
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Index", "Players");

            ViewBag.PlayerId = id;
            ViewBag.PlayerName = name;
            var teamsPlayersByPlayers = _context.TeamsPlayers.Where(t => t.PlayerId == id).Include(tp=>tp.Player).Include(r => r.Role).Include(te => te.Team);
            return View(await teamsPlayersByPlayers.ToListAsync());
        }

        // GET: TeamsPlayers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TeamsPlayers == null)
            {
                return NotFound();
            }

            var teamsPlayer = await _context.TeamsPlayers
                .Include(t => t.Player)
                .Include(t => t.Role)
                .Include(t => t.Team)
                .FirstOrDefaultAsync(m => m.TeamsPlayersId == id);
            if (teamsPlayer == null)
            {
                return NotFound();
            }

            return View(teamsPlayer);
        }

        // GET: TeamsPlayers/Create
        public IActionResult Create()
        {
            ViewData["PlayerId"] = new SelectList(_context.Players, "PlayerId", "Name");
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId");
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamId");
            return View();
        }

        // POST: TeamsPlayers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeamsPlayersId,JoinDate,LeftDate,RoleId,IsActive,PlayerId,TeamId")] TeamsPlayer teamsPlayer)
        {
            if (ModelState.IsValid || true)
            {
                _context.Add(teamsPlayer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlayerId"] = new SelectList(_context.Players, "PlayerId", "Name", teamsPlayer.PlayerId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", teamsPlayer.RoleId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamId", teamsPlayer.TeamId);
            return View(teamsPlayer);
        }

        // GET: TeamsPlayers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TeamsPlayers == null)
            {
                return NotFound();
            }

            var teamsPlayer = await _context.TeamsPlayers.FindAsync(id);
            if (teamsPlayer == null)
            {
                return NotFound();
            }
            ViewData["PlayerId"] = new SelectList(_context.Players, "PlayerId", "Name", teamsPlayer.PlayerId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", teamsPlayer.RoleId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamId", teamsPlayer.TeamId);
            return View(teamsPlayer);
        }

        // POST: TeamsPlayers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeamsPlayersId,JoinDate,LeftDate,RoleId,IsActive,PlayerId,TeamId")] TeamsPlayer teamsPlayer)
        {
            if (id != teamsPlayer.TeamsPlayersId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teamsPlayer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamsPlayerExists(teamsPlayer.TeamsPlayersId))
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
            ViewData["PlayerId"] = new SelectList(_context.Players, "PlayerId", "Name", teamsPlayer.PlayerId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", teamsPlayer.RoleId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamId", teamsPlayer.TeamId);
            return View(teamsPlayer);
        }

        // GET: TeamsPlayers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TeamsPlayers == null)
            {
                return NotFound();
            }

            var teamsPlayer = await _context.TeamsPlayers
                .Include(t => t.Player)
                .Include(t => t.Role)
                .Include(t => t.Team)
                .FirstOrDefaultAsync(m => m.TeamsPlayersId == id);
            if (teamsPlayer == null)
            {
                return NotFound();
            }

            return View(teamsPlayer);
        }

        // POST: TeamsPlayers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TeamsPlayers == null)
            {
                return Problem("Entity set 'DblibContext.TeamsPlayers'  is null.");
            }
            var teamsPlayer = await _context.TeamsPlayers.FindAsync(id);
            if (teamsPlayer != null)
            {
                _context.TeamsPlayers.Remove(teamsPlayer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamsPlayerExists(int id)
        {
          return (_context.TeamsPlayers?.Any(e => e.TeamsPlayersId == id)).GetValueOrDefault();
        }
    }
}
