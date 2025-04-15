using Microsoft.AspNetCore.Mvc;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace School_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolController : ControllerBase
    {
        private readonly AppDBContext _context;

        public SchoolController(AppDBContext context)
        {
            _context = context;
        }

        // 1. Create a new school
        [HttpPost("create")]
        public async Task<IActionResult> CreateSchool([FromBody] SchoolEntity school)
        {
            school.Created = DateTime.UtcNow;
            _context.School.Add(school);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSchoolProfile), new { id = school.Id }, school);
        }

        // 2. Get school profile by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchoolProfile(int id)
        {
            var school = await _context.School.FindAsync(id);
            if (school == null) return NotFound();
            return Ok(school);
        }

        // 3. Update school info
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchool(int id, [FromBody] SchoolEntity updatedSchool)
        {
            if (id != updatedSchool.Id) return BadRequest();

            var existingSchool = await _context.School.FindAsync(id);
            if (existingSchool == null) return NotFound();

            // Update properties
            existingSchool.Name = updatedSchool.Name;
            existingSchool.Address = updatedSchool.Address;
            existingSchool.PostalCode = updatedSchool.PostalCode;
            existingSchool.City = updatedSchool.City;
            existingSchool.Country = updatedSchool.Country;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 4. Upload school profile picture
        [HttpPost("{id}/upload-picture")]
        public async Task<IActionResult> UploadPicture(int id, IFormFile file)
        {
            var school = await _context.School.FindAsync(id);
            if (school == null) return NotFound();

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/schools");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            school.ImagePath = $"/images/schools/{fileName}";
            await _context.SaveChangesAsync();

            return Ok(new { school.Id, school.ImagePath });
        }

        // 5. View all schools
        [HttpGet]
        public async Task<IActionResult> GetAllSchools()
        {
            var schools = await _context.School.ToListAsync();
            return Ok(schools);
        }

        // 6. Deactivate a school
        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> DeactivateSchool(int id)
        {
            var school = await _context.School.FindAsync(id);
            if (school == null) return NotFound();

            school.IsActive = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = "School deactivated." });
        }
    }
}