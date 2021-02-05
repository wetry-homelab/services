using Infrastructure.Contracts.Request;
using Infrastructure.Contracts.Response;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITemplateBusiness
    {
        Task<TemplateResponse[]> GetTemplatesAsync();
        Task<bool> CreateTemplateAsync(TemplateCreateRequest request);
        Task<(bool found, bool success)> UpdateTemplateAsync(int id, TemplateUpdateRequest request);
        Task<(bool found, bool success)> DeleteTemplateAsync(int id);
    }
}
