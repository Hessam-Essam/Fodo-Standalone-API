using Fodo.Application.Implementation.Interfaces;
using Fodo.Contracts.DTOS;
using Fodo.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fodo.API.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IBranchesService _service;
        public ClientsController(IBranchesService service) => _service = service;

        // GET api/clients/id/by-code/{clientCode}
        [HttpGet("id/by-code/{clientCode}")]
        public async Task<ActionResult<ClientsDto>> GetIdByCode(string clientCode, CancellationToken ct)
        {
            var result = await _service.GetByClientCodeAsync(clientCode, ct);
            return result is null ? NotFound() : Ok(result);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
