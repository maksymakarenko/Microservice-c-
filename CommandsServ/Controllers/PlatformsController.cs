using System;
using AutoMapper;
using CommandsServ.Data;
using CommandsServ.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsServ.Controllers{

    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepository _repository;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms");
            
            var plat = _repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(plat));
        }

        [HttpPost]
        public ActionResult TestInConnection()
        {
            Console.WriteLine("--> In Post # Command Service");
        
            return Ok("In Test of from Platform Service");
        }
    }
}