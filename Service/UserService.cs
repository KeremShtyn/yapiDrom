using Entity.Models;
using Persistence.Abstracts;
using Util.Error;

namespace Service
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> UpdateUser(User user);
        Task<bool> HasSufficientBalanceAsync(int userId);
        Task<List<Reservation>> GetUserReservationsAsync(int userId);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetAsync((user) => user.Id == id);
        }

        public async Task<bool> HasSufficientBalanceAsync(int userId)
        {
            return await _userRepository.GetAsync((user) => user.Id == userId && user.Balance >= 15) != null;
        }

        public async Task<List<Reservation>> GetUserReservationsAsync(int userId)
        {
            User? user = await _userRepository.GetAsync(u => u.Id == userId);
            return user.Reservations;
        }

        public async Task<User?> UpdateUser(User user)
        {
            var u = await _userRepository.GetByIdAsync(user.Id) ?? throw new YapidromException(ErrorCodes.UserNotFound);
            u.Balance = user.Balance;
            return u;
        }
    }
}
