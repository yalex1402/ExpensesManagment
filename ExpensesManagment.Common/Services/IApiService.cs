using ExpensesManagment.Common.Models;
using System.Threading.Tasks;

namespace ExpensesManagment.Common.Services
{
    public interface IApiService
    {
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);
    }
}
