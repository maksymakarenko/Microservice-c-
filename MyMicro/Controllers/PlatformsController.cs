using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyMicro.AsyncDS;
using MyMicro.Data;
using MyMicro.Dtos;
using MyMicro.Models;
using MyMicro.SyncDS.Http;

namespace MyMicro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataCli _commandDataCli;
        private readonly IMessageBC _messageBC;

        public PlatformsController(IPlatformRepo repository, IMapper mapper, ICommandDataCli commandDataCli, IMessageBC messageBC)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataCli = commandDataCli;
            _messageBC = messageBC;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms");

            var platformItem = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItem));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platformItem = _repository.GetPlatformById(id);

            if(platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            try
            {
                await _commandDataCli.SendPlatformToCommand(platformReadDto);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"--> Could not send sync: {ex}");
            }

            try
            {
                var paltformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
                paltformPublishedDto.Event = "PlatformISPublished";
                _messageBC.PublishNewPlat(paltformPublishedDto);
            }
            catch (System.Exception ex)
            {
                
                Console.WriteLine($"--> Could not send async: {ex}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new {Id = platformReadDto.Id}, platformReadDto);
        }
    }
}