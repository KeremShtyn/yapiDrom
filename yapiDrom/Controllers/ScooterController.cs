using Microsoft.AspNetCore.Mvc;
using Service;
using ControllerBase = Microsoft.AspNetCore.Mvc.ControllerBase;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace yapiDrom.Controllers;

[Route("api/scooter")]
[ApiController]
public class ScooterController : ControllerBase
{
    private readonly IScooterService _scooterService;

    public ScooterController(IScooterService scooterService)
    {
        _scooterService = scooterService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailableScooters()
    {
        var scooters = await _scooterService.GetAvailableScootersAsync();
        return Ok(scooters);
    }

}
