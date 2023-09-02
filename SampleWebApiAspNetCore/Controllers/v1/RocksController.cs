using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Services;
using SampleWebApiAspNetCore.Models;
using SampleWebApiAspNetCore.Repositories;
using System.Text.Json;

namespace SampleWebApiAspNetCore.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RocksController : ControllerBase
    {
        private readonly IRockRepository _rockRepository;
        private readonly IMapper _mapper;
        private readonly ILinkService<RocksController> _linkService;

        public RocksController(
            IRockRepository rockRepository,
            IMapper mapper,
            ILinkService<RocksController> linkService)
        {
            _rockRepository = rockRepository;
            _mapper = mapper;
            _linkService = linkService;
        }

        [HttpGet(Name = nameof(GetAllRocks))]
        public ActionResult GetAllRocks(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
            List<RockEntity> rockItems = _rockRepository.GetAll(queryParameters).ToList();

            var allItemCount = _rockRepository.Count();

            var paginationMetadata = new
            {
                totalCount = allItemCount,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(allItemCount)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = _linkService.CreateLinksForCollection(queryParameters, allItemCount, version);
            var toReturn = rockItems.Select(x => _linkService.ExpandSingleRockItem(x, x.Id, version));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSingleRock))]
        public ActionResult GetSingleRock(ApiVersion version, int id)
        {
            RockEntity rockItem = _rockRepository.GetSingle(id);

            if (rockItem == null)
            {
                return NotFound();
            }

            RockDto item = _mapper.Map<RockDto>(rockItem);

            return Ok(_linkService.ExpandSingleRockItem(item, item.Id, version));
        }

        [HttpPost(Name = nameof(AddRock))]
        public ActionResult<RockDto> AddRock(ApiVersion version, [FromBody] RockCreateDto rockCreateDto)
        {
            if (rockCreateDto == null)
            {
                return BadRequest();
            }

            RockEntity toAdd = _mapper.Map<RockEntity>(rockCreateDto);

            _rockRepository.Add(toAdd);

            if (!_rockRepository.Save())
            {
                throw new Exception("Creating a rockitem failed on save.");
            }

            RockEntity newRockItem = _rockRepository.GetSingle(toAdd.Id);
            RockDto rockDto = _mapper.Map<RockDto>(newRockItem);

            return CreatedAtRoute(nameof(GetSingleRock),
                new { version = version.ToString(), id = newRockItem.Id },
                _linkService.ExpandSingleRockItem(rockDto, rockDto.Id, version));
        }

        [HttpPatch("{id:int}", Name = nameof(PartiallyUpdateRock))]
        public ActionResult<RockDto> PartiallyUpdateRock(ApiVersion version, int id, [FromBody] JsonPatchDocument<RockUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            RockEntity existingEntity = _rockRepository.GetSingle(id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            RockUpdateDto rockUpdateDto = _mapper.Map<RockUpdateDto>(existingEntity);
            patchDoc.ApplyTo(rockUpdateDto);

            TryValidateModel(rockUpdateDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(rockUpdateDto, existingEntity);
            RockEntity updated = _rockRepository.Update(id, existingEntity);

            if (!_rockRepository.Save())
            {
                throw new Exception("Updating a rockitem failed on save.");
            }

            RockDto rockDto = _mapper.Map<RockDto>(updated);

            return Ok(_linkService.ExpandSingleRockItem(rockDto, rockDto.Id, version));
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(RemoveRock))]
        public ActionResult RemoveRock(int id)
        {
            RockEntity rockItem = _rockRepository.GetSingle(id);

            if (rockItem == null)
            {
                return NotFound();
            }

            _rockRepository.Delete(id);

            if (!_rockRepository.Save())
            {
                throw new Exception("Deleting a rockitem failed on save.");
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}", Name = nameof(UpdateRock))]
        public ActionResult<RockDto> UpdateRock(ApiVersion version, int id, [FromBody] RockUpdateDto rockUpdateDto)
        {
            if (rockUpdateDto == null)
            {
                return BadRequest();
            }

            var existingRockItem = _rockRepository.GetSingle(id);

            if (existingRockItem == null)
            {
                return NotFound();
            }

            _mapper.Map(rockUpdateDto, existingRockItem);

            _rockRepository.Update(id, existingRockItem);

            if (!_rockRepository.Save())
            {
                throw new Exception("Updating a rockitem failed on save.");
            }

            RockDto rockDto = _mapper.Map<RockDto>(existingRockItem);

            return Ok(_linkService.ExpandSingleRockItem(rockDto, rockDto.Id, version));
        }

        [HttpGet("GetRandomMineral", Name = nameof(GetRandomMineral))]
        public ActionResult GetRandomMineral()
        {
            ICollection<RockEntity> rockItems = _rockRepository.GetRandomMineral();

            IEnumerable<RockDto> dtos = rockItems.Select(x => _mapper.Map<RockDto>(x));

            var links = new List<LinkDto>();

            // self 
            links.Add(new LinkDto(Url.Link(nameof(GetRandomMineral), null), "self", "GET"));

            return Ok(new
            {
                value = dtos,
                links = links
            });
        }
    }
}
