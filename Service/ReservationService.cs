using Entity.Models;
using Persistence.Abstracts;
using Util.Error;

namespace Service;

public interface IReservationService
{
    Task<Reservation?> CreateReservationAsync(int userId, int scooterId);
    Task EndReservationAsync(int reservationId, string endedBy);
    Task<IEnumerable<Reservation>> GetAllReservationsAsync();
    Task<IEnumerable<Reservation>> GetUserReservationsAsync(int userId);
    Task<List<Reservation>> EndExpiredReservationsAsync();
}

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IScooterService _scooterService;
    private readonly IUserService _userService;

    public ReservationService(
        IReservationRepository reservationRepository,
        IScooterService scooterService,
        IUserService userService)
    {
        _reservationRepository = reservationRepository;
        _scooterService = scooterService;
        _userService = userService;
    }

    public async Task<Reservation?> CreateReservationAsync(int userId, int scooterId)
    {

        var hasSufficientBalance = await _userService.HasSufficientBalanceAsync(userId);
        if (!hasSufficientBalance)
        {
            throw new YapidromException(ErrorCodes.InsufficientBalance);
        }

        var scooter = await _scooterService.GetByIdAsync(scooterId) ?? throw new YapidromException(ErrorCodes.ScooterNotFound);
        if (!scooter.IsAvailable)
        {
            throw new YapidromException(ErrorCodes.ScooterNotAvailable);
        }

        var user = await _userService.GetUserByIdAsync(userId) ?? throw new YapidromException(ErrorCodes.UserNotFound);
        if (user.Reservations.Any(reservation => reservation.Status == ScooterStatus.Active))
        {
            throw new YapidromException(ErrorCodes.UserHasReservation);
        }
       

        var reservation = new Reservation
        {
            UserId = userId,
            ScooterId = scooterId,
            StartTime = DateTime.UtcNow,
            Status = ScooterStatus.Active
        };

        scooter.IsAvailable = false;
        await _scooterService.UpdateAsync(scooter);

        await _reservationRepository.AddAsync(reservation);

        return reservation;
    }

    public async Task EndReservationAsync(int reservationId, string endedBy)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation == null)
        {
            throw new YapidromException(ErrorCodes.DataNotFound);
        }

        reservation.EndTime = DateTime.UtcNow;
        var durationMinutes = Math.Ceiling((reservation.EndTime.Value - reservation.StartTime).TotalMinutes);

        reservation.AmountCharged = (decimal)durationMinutes;
        reservation.Status = ScooterStatus.Completed;

        var scooter = await _scooterService.GetByIdAsync(reservation.ScooterId) ?? throw new YapidromException(ErrorCodes.ScooterNotFound);
        var user = await _userService.GetUserByIdAsync(reservation.UserId) ?? throw new YapidromException(ErrorCodes.UserNotFound);
        if (user.Balance < reservation.AmountCharged)
        {
            throw new YapidromException(ErrorCodes.InsufficientBalance );
        }

        user.Balance = user.Balance - reservation.AmountCharged;

        scooter.IsAvailable = true;
        await _scooterService.UpdateAsync(scooter);

        await _reservationRepository.UpdateAsync(reservation);
    }

    public async Task<List<Reservation>> EndExpiredReservationsAsync()
    {
        var now = DateTime.UtcNow;
        var expiredReservations = await _reservationRepository.GetAllAsync();
        var expiredReservationsList = expiredReservations
            .Where(reservation => reservation.Status == ScooterStatus.Active && reservation.StartTime.AddMinutes(15) <= now)
            .ToList();

        foreach (var reservation in expiredReservationsList)
        {
            reservation.EndTime = now;
            reservation.AmountCharged = 15;
            reservation.Status = ScooterStatus.Completed;

            var scooter = await _scooterService.GetByIdAsync(reservation.ScooterId) ?? throw new YapidromException(ErrorCodes.ScooterNotFound);
            
                scooter.IsAvailable = true;
                await _scooterService.UpdateAsync(scooter);

            await _reservationRepository.UpdateAsync(reservation);
        }

        return expiredReservationsList;
    } 


    public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
    {
        return await _reservationRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Reservation>> GetUserReservationsAsync(int userId)
    {
        return await _reservationRepository.GetAllAsync(reservation => reservation.UserId == userId);
    }
}
