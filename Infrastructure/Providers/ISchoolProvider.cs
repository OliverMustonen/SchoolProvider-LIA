namespace Infrastructure.Providers;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;



public interface ISchoolProvider
{
    Task<IEnumerable<SchoolEntity>> GetAllSchoolsAsync();
    Task<SchoolEntity?> GetSchoolByIdAsync(int id);
    Task<SchoolEntity> CreateSchoolAsync(SchoolEntity school);
    Task<bool> UpdateSchoolAsync(SchoolEntity school);
    Task<bool> DeactivateSchoolAsync(int id);
    Task<bool> UploadSchoolImageAsync(int id, IFormFile image);
}
