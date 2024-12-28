

using DeadlineService.Application.Interfaces.Repostitories;
using DeadlineService.Domain.Models;
using DeadlineService.Domain.Models.DTOs.Role;
using DeadlineService.Domain.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace DeadlineService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _repository;

        public RoleController(IRoleRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<ResponseModel<Role>> Create(Role role)
        {
            var response = await _repository.CreateAsync(role);
            return new ResponseModel<Role>(response);
        }

        [HttpGet]
        public async Task<ResponseModel<IEnumerable<Role>>> GetAll()
        {
            var roles = await _repository.GetAllAsync();
            return new ResponseModel<IEnumerable<Role>>(roles);
        }

        [HttpGet]
        public async Task<ResponseModel<IEnumerable<RoleDTO>>> GetAllWithUser()
        {
            var roles = await _repository.GetAllWithUserAsync();

            var roleDtos = roles.Select(x => new RoleDTO
            {
                Id = x.Id,
                Name = x.Name,
                Users = x.Users.Select(role => role.Username).ToList()

            });

            return new ResponseModel<IEnumerable<RoleDTO>>(roleDtos);
        }

    }
}
