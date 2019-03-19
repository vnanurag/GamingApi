using PlayStudiosApi.Domain.Models;
using PlayStudiosApi.Services.Models;
using PlayStudiosApi.Services.Services.Interfaces;
using Serilog;
using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace PlayStudiosApi.Controllers
{
    [RoutePrefix("api")]
    public class QuestController : ApiController
    {
        private readonly IQuestService _questService;
        private readonly ILogger _logger;

        public QuestController(
            IQuestService questService,
            ILogger logger)
        {
            _questService = questService;
            _logger = logger;
        }

        // api/progress

        /// <summary>
        /// Gets the Quest progress
        /// </summary>
        /// <param name="playerInfo">Player information</param>
        /// <returns>Quest progress thus far</returns>
        [HttpPost]
        [Route("progress")]
        [ResponseType(typeof(QuestProgress))]
        public IHttpActionResult GetQuestProgress([FromBody]PlayerInfo playerInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(playerInfo.PlayerId))
                {
                    _logger.Error("Bad Request. Player Id is not valid.");
                    return BadRequest("Please provide valid player information to see the quest progress.");
                }

                var response = _questService.GetQuestProgress(playerInfo);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger
                    .Error($"Request failed with the following message: {ex.Message} (StackTrace: {ex.StackTrace})");

                return InternalServerError(ex);
            }
        }

        // api/state?playerId=4
        // Use [HttpGet("state/{playerId}")] for api/state/4

        /// <summary>
        /// Gets the Quest state for a player
        /// </summary>
        /// <param name="playerId">Id of the player</param>
        /// <returns>Quest state</returns>
        [HttpGet]
        [Route("state")]
        [ResponseType(typeof(QuestState))]
        public IHttpActionResult GetQuestState([FromUri]string playerId)
        {
            try
            {
                if (string.IsNullOrEmpty(playerId))
                {
                    _logger.Error("Bad Request. Player Id is not valid");
                    return BadRequest("Player Id is not valid.");
                }

                var response = _questService.GetQuestState(playerId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger
                    .Error($"Request failed with the following message: {ex.Message} (StackTrace: {ex.StackTrace})");

                return InternalServerError(ex);
            }
        }
    }
}
