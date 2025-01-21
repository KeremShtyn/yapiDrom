using Entity.Models;
using Persistence.Abstracts;
using Persistence.Repositories;
using Util.Error;

namespace Service
{

    public interface IScooterService
    {
        Task<IEnumerable<Scooter>> GetAvailableScootersAsync();
        Task<Scooter> GetByIdAsync(int scooterId);
        Task<Scooter> UpdateAsync(Scooter scooter);
    }
    public class ScooterService : IScooterService
    {
        private readonly IScooterRepository _scooterRepository;

        public ScooterService(IScooterRepository scooterRepository)
        {
            _scooterRepository = scooterRepository;
        }

        public async Task<IEnumerable<Scooter>> GetAvailableScootersAsync()
        {
            return (await _scooterRepository.GetAllAsync()).Where(scooter => scooter.IsAvailable);
        }

        public async Task<Scooter> GetByIdAsync(int scooterId)
        {
            return await _scooterRepository.GetAsync(scooter => scooter.Id == scooterId) ?? throw new YapidromException(ErrorCodes.ScooterNotFound);
        }


        public async Task<Scooter> UpdateAsync(Scooter scooter)
        {
            var s = await _scooterRepository.GetAsync(s => s.Id == scooter.Id) ?? throw new YapidromException(ErrorCodes.ScooterNotFound);
            s.IsAvailable = scooter.IsAvailable;
            s.Location = scooter.Location;
            return s;
        }
    }
}
