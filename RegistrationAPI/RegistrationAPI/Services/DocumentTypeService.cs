using Microsoft.EntityFrameworkCore;
using RegistrationAPI.Data;
using RegistrationAPI.Models;

namespace RegistrationAPI.Services
{
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly EmployeesDbContext _database;

        public DocumentTypeService(EmployeesDbContext database)
        {
            _database = database;
        }

        public IQueryable<DocumentType> ListDocumentTypes(string? searchTerm = null)
        {
            var query = _database.DocumentTypes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(dt => dt.Name.Contains(searchTerm) ||
                                         (dt.Description != null && dt.Description.Contains(searchTerm)));
            }

            return query.OrderBy(dt => dt.Name);
        }

        public async Task<DocumentType?> FindDocumentType(int id)
        {
            return await _database.DocumentTypes.FindAsync(id);
        }

        public async Task InsertDocumentType(DocumentType entity)
        {
            _database.DocumentTypes.Add(entity);
            await _database.SaveChangesAsync();
        }

        public async Task UpdateDocumentType(DocumentType entity)
        {
            _database.DocumentTypes.Update(entity);
            await _database.SaveChangesAsync();
        }

        public async Task DeleteDocumentType(DocumentType entity)
        {
            _database.DocumentTypes.Remove(entity);
            await _database.SaveChangesAsync();
        }

        public async Task<bool> HasRelatedEmployees(int id)
        {
            return await _database.Employees.AnyAsync(e => e.DocumentTypeId == id);
        }
    }
}
