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
    [Route("api/employ")]
    //[ApiVersion(2, Deprecated = true)]
    public class EmployController : ControllerBase {
        private const int maxEmployPageSize = 2;
        private readonly IEmplyInfoRepository _employInfoRepository;
        private readonly IMapper _mapper;

        public EmployController(IEmplyInfoRepository emplyInfoRepository, IMapper mapper) {
            _employInfoRepository = emplyInfoRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployDto>> GetEmploy(int id) {
            var employ = await _employInfoRepository.GetEmployAsync(id);
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

            var (employs, paginationMetadata) = await _employInfoRepository
                .GetEmploysAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<EmployDto>>(employs));
        }

        [HttpPost("create")]
        public async Task<ActionResult<EmployDto>> CreateEmploy(EmployForCreationDto employ) {

            var finalEmploy = _mapper.Map<Employ>(employ);

            await _employInfoRepository.AddEmployAsync(finalEmploy);
            await _employInfoRepository.SaveChangesAsync();

            var employToReturn = _mapper.Map<EmployDto>(finalEmploy);

            return Ok(employToReturn);
        }
    }
}
