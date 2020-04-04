using ExpensesManagment.Common.Models;
using System.Threading.Tasks;

namespace ExpensesManagment.Common.Services
{
    public interface IApiService
    {
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);

        Task<Response> GetTripAsync(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken);

        Task<bool> CheckConnectionAsync(string url);

        Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);

        Task<Response> GetUserByEmail(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, EmailRequest request);

        Task<Response> RegisterUserAsync(string urlBase, string servicePrefix, string controller, UserRequest userRequest);

        Task<Response> GetTripsByUser(string urlBase, string servicePrefix, string controller, string tokenType,string accessToken, MyTripsRequest myTripsRequest);

        Task<Response> AddTrip(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, TripRequest tripRequest);

        Task<Response> AddExpense(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, ExpenseRequest expenseRequest);

        Task<Response> RecoverPasswordAsync(string urlBase, string servicePrefix, string controller, EmailRequest emailRequest);

        Task<Response> PutAsync<T>(string urlBase, string servicePrefix, string controller, T model, string tokenType, string accessToken);

        Task<Response> ChangePasswordAsync(string urlBase, string servicePrefix, string controller, ChangePasswordRequest changePasswordRequest, string tokenType, string accessToken);

        Task<Response> DeleteTripAsync(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken);

        Task<Response> DeleteExpenseAsync(string urlBase, string servicePrefix, string controller, ExpenseRequest expenseRequest, string tokenType, string accessToken);

    }
}
