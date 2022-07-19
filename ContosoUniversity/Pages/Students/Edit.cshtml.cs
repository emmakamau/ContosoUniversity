using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public EditModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; } = default!;

        //Code changed from here from FirstOrDefaultAsync to FindAsync
        /*
         FirstOrDefaultAsync has been replaced with FindAsync.
         When you don't have to include related data, FindAsync is more efficient.

         */
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await _context.Students.FindAsync(id);

            if (Student == null)
            {
                return NotFound();
            }
            return Page();
            //if (id == null || _context.Students == null)
            //{
            //    return NotFound();
            //}

            //var student =  await _context.Students.FirstOrDefaultAsync(m => m.ID == id);
            //if (student == null)
            //{
            //    return NotFound();
            //}
            //Student = student;
            //return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            //Add an id parameter to OnPostAsync.
            //The current student is fetched from the database, rather than creating an empty student.
            var studentToUpdate = await _context.Students.FindAsync(id);

            if (studentToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Student>(
                studentToUpdate,
                "student",
                s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //_context.Attach(Student).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!StudentExists(Student.ID))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return RedirectToPage("./Index");
        }

        private bool StudentExists(int id)
        {
          return (_context.Students?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
