using System.Security;
using System.Text.Json;

using App.RestApi.CommandQueries;
using App.RestApi.Model;
using App.Store.Model;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace App.RestApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Produces("application/json")]
    public class WordController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<WordController> logger;

        public WordController(IMediator mediator, ILogger<WordController> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }


        /// <summary>
        /// Список слов
        /// </summary>
        /// <response code="200">список слов</response>
        /// <response code="400">описание логической ошибки</response>
        /// <response code="500">описание ошибки сервера</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<WordData>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            try
            {
                var wordList = await mediator.Send(new WordListQuery());
                return Ok(new WordListResponse(wordList));
            }
            catch (Exception ex) { return Ok(new ApiResponse(1, ex.Message)); }
        }

        /// <summary>
        /// Регистрация слова
        /// </summary>
        /// <param name="wordCreateRequest">параметры запроса</param>
        /// <response code="200">empty</response>
        /// <response code="400">описание логической ошибки</response>
        /// <response code="500">описание ошибки сервера</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [HttpPost]
        public async Task<IActionResult> Create(WordCreateRequest wordCreateRequest)
        {
            try
            {
                await mediator.Send(new WordCreateCommand(wordCreateRequest.Prefix, wordCreateRequest.Root, wordCreateRequest.Sufix, wordCreateRequest.Full));
                return Ok(new ApiResponse());
            }
            catch (Exception ex) { return Ok(new ApiResponse(1, ex.Message)); }
        }


        /// <summary>
        /// Список слов
        /// </summary>
        /// <response code="200">список слов</response>
        /// <response code="400">описание логической ошибки</response>
        /// <response code="500">описание ошибки сервера</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<WordData>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [HttpPost]
        public async Task<IActionResult> Search(WordSearchRequest searchRequest)
        {
            try
            {
                var wordList = await mediator.Send(new WordSearchQuery(searchRequest.Root));
                return Ok(new WordSearchResponse(wordList));
            }
            catch (Exception ex) { return Ok(new ApiResponse(1, ex.Message)); }
        }


        private ObjectResult Problem(Exception ex, string name, object? args = null)
        {
            logger.LogError(ex, "{0}: {1}", name, args == null ? "" : JsonSerializer.Serialize(args));

            if (ex is SecurityException || ex is ArgumentException)
                return Problem(ex.Message, statusCode: StatusCodes.Status400BadRequest);

            return Problem("oops, server inner exception");

        }

    }
}
