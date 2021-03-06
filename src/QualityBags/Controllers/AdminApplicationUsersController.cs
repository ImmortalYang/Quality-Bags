using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityBags.Data;
using QualityBags.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace QualityBags.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminApplicationUsersController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: AdminApplicationUsers
        public async Task<IActionResult> Index()
        {
            return View(await ReturnAllMembers());
        }

        // GET: AdminApplicationUsers/Details/1
        public async Task<IActionResult> Details(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var member = await _context.ApplicationUser
                .Include(m => m.Orders)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);
            if(member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // GET: AdminApplicationUsers/Delete/1
        public async Task<IActionResult> Delete(string id, bool? saveChangesError = false)
        {
            if(id == null)
            {
                return NotFound();
            }
            var member = await _context.ApplicationUser
                .Include(m => m.Orders)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. " +
                    "Please make sure member is disabled " +
                    "and has no orders. " +
                    "If problem persists, please contact administrator. ";
            }
            return View(member);
        }

        // POST: AdminApplicationUsers/Delete/5
        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var member = await _context.ApplicationUser
                .SingleOrDefaultAsync(m => m.Id == id);
            if (member.Enabled)
            {
                return RedirectToAction("Delete",
                    new { id = id, saveChangesError = true });
            }
            _context.Remove(member);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return RedirectToAction("Delete", 
                    new { id = id, saveChangesError = true });
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Return all member users from database
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<ApplicationUser>> ReturnAllMembers()
        {
            IdentityRole role = await _context.Roles.SingleOrDefaultAsync(r => r.Name == "Member");
            IEnumerable<ApplicationUser> users = await _context.Users
                .Where(u => u.Roles.Select(r => r.RoleId).Contains(role.Id))
                .ToListAsync();
            return users;
        }

        /// <summary>
        /// Enable or disable a user account
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns></returns>
        public async Task<IActionResult> EnableDisable(string userId)
        {
            if(userId == null)
            {
                return NotFound();
            }
            ApplicationUser member = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
            if(member == null)
            {
                return NotFound();
            }
            member.Enabled = !member.Enabled;
            _context.Update(member);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
