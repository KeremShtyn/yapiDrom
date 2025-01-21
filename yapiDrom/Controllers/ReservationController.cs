using Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace yapiDrom.Controllers
{

    [Route("api/reservation")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly ReservationBackgroundService _reservationBackgroundService;

        public ReservationController(IReservationService reservationService, ReservationBackgroundService reservationBackgroundService)
        {
            _reservationService = reservationService;
            _reservationBackgroundService = reservationBackgroundService;
        }

        [HttpPost("/create")]
        public async Task<IActionResult> CreateReservation([FromBody] Reservation reservation)
        {
            try
            {
                var newReservation = await _reservationService.CreateReservationAsync(reservation.UserId, reservation.ScooterId);
                await _reservationBackgroundService.PublishReservationStartMessageAsync(reservation);
                return Ok(newReservation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("/end/{reservationId}")]
        public async Task<IActionResult> EndReservation(int reservationId, [FromBody] string endedBy)
        {
            try
            {
                await _reservationService.EndReservationAsync(reservationId, endedBy);
                return Ok(new { Message = "Reservation ended successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserReservations(int userId)
        {
            var reservations = await _reservationService.GetUserReservationsAsync(userId);
            return Ok(reservations);
        }

    }
}
