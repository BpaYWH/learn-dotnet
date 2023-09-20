using Microsoft.AspNetCore.Mvc;
using FiveInARow.Services.FiveInARow;
using AutoMapper;
using FiveInARow.Models;
using FiveInARow.Dto;

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
        public IActionResult CreateGameRecord([FromBody] GameRecordDto gameRecordCreate)
        {
            if (gameRecordCreate == null)
                return BadRequest(ModelState);

            var gameRecord = _gameRecordService.GetGameRecords()
                .FirstOrDefault(gr => gr.StartedAt == gameRecordCreate.StartedAt && gr.EndedAt == gameRecordCreate.EndedAt);

            if (gameRecord != null)
                return BadRequest("Game record already exists");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var gameRecordMap = _mapper.Map<GameRecord>(gameRecordCreate);

            if (!_gameRecordService.CreateGameRecord(gameRecordMap))
            {
                ModelState.AddModelError("", "Something went wrong while creating game record");
                return StatusCode(500, ModelState);
            }
            
            return Ok("Successfully created");
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(GameRecord))]
        [ProducesResponseType(404)]
        public IActionResult GetGameRecord(int id)
        {
            if (!_gameRecordService.GameRecordExists(id))
                return NotFound();

            GameRecord gameRecord = _mapper.Map<GameRecord>(_gameRecordService.GetGameRecord(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(gameRecord);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GameRecord>))]
        public IActionResult GetGameRecords()
        {
            var gameRecords = _mapper.Map<List<GameRecord>>(_gameRecordService.GetGameRecords());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(gameRecords);
        }
    }
}