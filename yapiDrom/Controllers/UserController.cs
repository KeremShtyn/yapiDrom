using Microsoft.AspNetCore.Mvc;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yapiDrom.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
    {

    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return Ok(user);
    }

    [HttpGet("{id}/balance-check")]
    public async Task<IActionResult> GetUserBalance(int id)
    {
        var balance = await _userService.HasSufficientBalanceAsync(id);
        return Ok(balance);
    }

    [HttpGet("{id}/reservations")]
    public async Task<IActionResult> GetUserReservations(int id)
    {
        var reservations = await _userService.GetUserReservationsAsync(id);
        return Ok(reservations);
    }
}

