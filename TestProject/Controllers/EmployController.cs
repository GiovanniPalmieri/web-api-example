using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TestProject.Entities;
using TestProject.Models;
using TestProject.Services;

namespace TestProject.Controllers {
    [ApiController]
    //per testare il versionamento
    //[Authorize(Policy = "MustBeOverTheAgeOf30")]
    [Route("employee")]
    //[ApiVersion(2, Deprecated = true)]
    public class EmployController : ControllerBase {
        private const int maxEmployPageSize = 2;
        private readonly IGiraRepository _giraRepository;
        private readonly IMapper _mapper;

        public EmployController(IGiraRepository emplyInfoRepository, IMapper mapper) {
            _giraRepository = emplyInfoRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployDto>> GetEmploy(int id) {
            var employ = await _giraRepository.GetEmployAsync(id);
            if (employ == null) {
                return NotFound();
            }

            return Ok(_mapper.Map<EmployDto>(employ));
        }

        /// <summary>
        /// Restituisce la lista degli impiegati
        /// </summary>
        /// <param name="name">filtra in base al nome</param>
        /// <param name="searchQuery">ricerca testuale su nome e descrizione</param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployDto>>> GetEmploys(
            [FromQuery(Name = "name")] string? name, [FromQuery(Name = "searchQuery")] string? searchQuery,
            int pageNumber = 1, int pageSize = 10) {

            if (pageSize > maxEmployPageSize) {
                pageSize = maxEmployPageSize;
            }

            var (employs, paginationMetadata) = await _giraRepository
                .GetEmploysAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<EmployDto>>(employs));
        }

        [HttpPost("create")]
        public async Task<ActionResult<EmployDto>> CreateEmploy(EmployForCreationDto employ) {

            var finalEmploy = _mapper.Map<Employee>(employ);

            await _giraRepository.AddEmployAsync(finalEmploy);
            await _giraRepository.SaveChangesAsync();

            var employToReturn = _mapper.Map<EmployDto>(finalEmploy);

            return Ok(employToReturn);
        }
    }
}
