using RegistrationAPI.Models;

namespace RegistrationAPI.Services
{
    public interface IDocumentTypeService
    {
        IQueryable<DocumentType> ListDocumentTypes(string? searchTerm = null);
        Task<DocumentType?> FindDocumentType(int id);
        Task InsertDocumentType(DocumentType entity);
        Task UpdateDocumentType(DocumentType entity);
        Task DeleteDocumentType(DocumentType entity);
        Task<bool> HasRelatedEmployees(int id);
    }
}
