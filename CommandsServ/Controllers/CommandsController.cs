using AutoMapper;
using CommandsServ.Data;
using CommandsServ.DataLayerModels;
using CommandsServ.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsServ.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepository _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsFromPlatforms(int platformId)
        {
            Console.WriteLine($"--> GetCommandsForPlatform: {platformId}");

            if(!_repository.platformExist(platformId))
                return NotFound();

            var commands = _repository.GetCommandsFromPlatforms(platformId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandFromPlatforms")]
        public ActionResult<CommandReadDto> GetCommandFromPlatforms(int platformId, int commandId)
        {
            Console.WriteLine($"--> GetCommandForPlatform: {platformId}, {commandId}");

            if(!_repository.platformExist(platformId))
                return NotFound();

            var command = _repository.GetCommand(platformId, commandId);

            if(command == null)
                return NotFound();

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(int platformId, CommandCreateDto commandCreateDto)
        {
             Console.WriteLine($"--> CreateCommand: {platformId}");

            if(!_repository.platformExist(platformId))
                return NotFound();

            var command = _mapper.Map<Command>(commandCreateDto);

            try
            {
                _repository.CreateCommand(platformId, command);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"--> Could not create command {ex}");
            }

            _repository.saveChanges();
            
            var commandreadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommandFromPlatforms), 
                new {platformId = platformId, commandId = commandreadDto.Id}, commandreadDto);
        }
    }
}