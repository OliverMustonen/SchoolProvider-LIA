using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Infrastructure.Providers;
using Microsoft.AspNetCore.Hosting;





public class SchoolProvider : ISchoolProvider
{
    private readonly AppDBContext _context;
    private readonly IWebHostEnvironment _environment;

    public SchoolProvider(AppDBContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<IEnumerable<SchoolEntity>> GetAllSchoolsAsync()
    {
        return await _context.School.Where(s => s.IsActive).ToListAsync();
    }

    public async Task<SchoolEntity?> GetSchoolByIdAsync(int id)
    {
        return await _context.School.FindAsync(id);
    }

    public async Task<SchoolEntity> CreateSchoolAsync(SchoolEntity school)
    {
        school.Created = DateTime.UtcNow;
        school.IsActive = true;
        _context.School.Add(school);
        await _context.SaveChangesAsync();
        return school;
    }

    public async Task<bool> UpdateSchoolAsync(SchoolEntity school)
    {
        var existing = await _context.School.FindAsync(school.Id);
        if (existing == null) return false;

        existing.Name = school.Name;
        existing.Address = school.Address;
        existing.City = school.City;
        existing.PostalCode = school.PostalCode;
        existing.Country = school.Country;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeactivateSchoolAsync(int id)
    {
        var school = await _context.School.FindAsync(id);
        if (school == null) return false;

        school.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UploadSchoolImageAsync(int id, IFormFile image)
    {
        var school = await _context.School.FindAsync(id);
        if (school == null || image == null) return false;

        var filePath = Path.Combine(_environment.WebRootPath, "images", $"{id}_{image.FileName}");
        using var stream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(stream);

        school.ImagePath = $"/images/{id}_{image.FileName}";
        await _context.SaveChangesAsync();
        return true;
    }
}
