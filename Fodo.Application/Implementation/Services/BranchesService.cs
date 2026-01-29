using Fodo.Application.Implementation.Interfaces;
using Fodo.Application.Implementation.IRepositories;
using Fodo.Contracts.DTOS;

namespace Fodo.Application.Implementation.Services
{
    public class BranchesService : IBranchesService
    {
        private readonly IClientRepository _clientRepo;
        private readonly IBranchesRepository _branchRepo;
        public BranchesService(IClientRepository clientRepo, IBranchesRepository branchRepo)
        {
            _clientRepo = clientRepo;
            _branchRepo = branchRepo;
        }

        public async Task<ClientsDto> GetByClientCodeAsync(string clientCode, CancellationToken ct)
        {
            var clientId = await _clientRepo.GetClientIdByCodeAsync(clientCode, ct);
            if (clientId is null) return null;

            var branches = await _branchRepo.GetByClientIdAsync(clientId.Value, ct);
            return new ClientsDto(clientId.Value, branches);
        }
    }
}
