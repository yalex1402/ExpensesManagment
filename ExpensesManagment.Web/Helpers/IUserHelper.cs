using ExpensesManagment.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Helpers
{
    public interface IUserHelper
    {
        Task<UserEntity> GetUserAsync(string email);

        Task<IdentityResult> AddUserAsync(UserEntity user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(UserEntity user, string roleName);

        Task<bool> IsUserInRoleAsync(UserEntity user, string roleName);

    }
}
