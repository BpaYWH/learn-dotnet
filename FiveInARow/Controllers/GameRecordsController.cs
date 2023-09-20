using Microsoft.AspNetCore.Mvc;
using FiveInARow.Contracts.FiveInARow;
using FiveInARow.Services.FiveInARow;
using AutoMapper;
using FiveInARow.Models;

namespace FiveInARow.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameRecordsContoller : ControllerBase
    {
        private readonly IGameRecordService _gameRecordService;
        private readonly IMapper _mapper;

        public GameRecordsContoller(IGameRecordService gameRecordService, IMapper mapper)
        {
            _gameRecordService = gameRecordService;
            _mapper = mapper;
        
        }
        [HttpPost()]
        public IActionResult CreateGameRecord(CreateGameRecordRequest request)
        {
            return Ok(request);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetGameRecord(int id)
        {
            if (!_gameRecordService.GameRecordExists(id))
                return NotFound();

            GameRecord gameRecord = _mapper.Map<GameRecord>(_gameRecordService.GetGameRecord(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(gameRecord);
        }
    }
}