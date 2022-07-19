using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ContosoUniversity.Pages.Students
{
    /*
    New Code:
        -Adds Logging.
        -Adds the optional parameter saveChangesError to the OnGetAsync method signature. 
        saveChangesError indicates whether the method was called after a failure to delete the student object. 

    The delete operation might fail because of transient network problems. Transient network errors are more 
    likely when the database is in the cloud. The saveChangesError parameter is false when the Delete page 
    OnGetAsync is called from the UI. When OnGetAsync is called by OnPostAsync because the delete operation failed,
    the saveChangesError parameter is true.

    The OnPostAsync method retrieves the selected entity, then calls the Remove method to set the entity's status to Deleted. When SaveChanges is called, a SQL DELETE command is generated. If Remove fails:

    The database exception is caught.
    The Delete pages OnGetAsync method is called with saveChangesError=true.
     */
    public class DeleteModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;
        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(ContosoUniversity.Data.SchoolContext context,
                           ILogger<DeleteModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public Student Student { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);

            if (Student == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = String.Format("Delete {ID} failed. Try again", id);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            try
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, ErrorMessage);

                return RedirectToAction("./Delete",
                                     new { id, saveChangesError = true });
            }
        }
    }
}



//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using ContosoUniversity.Data;
//using ContosoUniversity.Models;

//namespace ContosoUniversity.Pages.Students
//{
//    public class DeleteModel : PageModel
//    {
//        private readonly ContosoUniversity.Data.SchoolContext _context;

//        public DeleteModel(ContosoUniversity.Data.SchoolContext context)
//        {
//            _context = context;
//        }

//        [BindProperty]
//      public Student Student { get; set; } = default!;

//        public async Task<IActionResult> OnGetAsync(int? id)
//        {
//            if (id == null || _context.Students == null)
//            {
//                return NotFound();
//            }

//            var student = await _context.Students.FirstOrDefaultAsync(m => m.ID == id);

//            if (student == null)
//            {
//                return NotFound();
//            }
//            else 
//            {
//                Student = student;
//            }
//            return Page();
//        }

//        public async Task<IActionResult> OnPostAsync(int? id)
//        {
//            if (id == null || _context.Students == null)
//            {
//                return NotFound();
//            }
//            var student = await _context.Students.FindAsync(id);

//            if (student != null)
//            {
//                Student = student;
//                _context.Students.Remove(Student);
//                await _context.SaveChangesAsync();
//            }

//            return RedirectToPage("./Index");
//        }
//    }
//}
