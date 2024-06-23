using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : Controller
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandClient _commandClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformController(IPlatformRepo repository, IMapper mapper,ICommandClient commandClient,IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandClient = commandClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting platforms...");
            var platformItem = _repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItem));

        }

        
        [HttpGet("{id}",Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            Console.WriteLine("--> Getting Platform by id");
            var platform=_repository.GetPlatformById(id);
            if(platform == null) { return NotFound(); }
            return Ok(_mapper.Map<PlatformReadDto>(platform));
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel=_mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();
            var platformDto=_mapper.Map<PlatformReadDto>(platformModel);

            //Send Sync message
            try
            {
                await _commandClient.SendPlatformToCommand(platformDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> could not send synchronously {ex.Message}");
            }

            //Send Async Message
            try
            {
                var platformPublishedDto=_mapper.Map<PlatformPublishedDto>(platformDto);
                platformPublishedDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> could not send Asynchronously {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById),new {Id=platformDto.Id }, platformDto);

        }
    }
}
