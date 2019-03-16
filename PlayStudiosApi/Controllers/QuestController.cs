using PlayStudiosApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace PlayStudiosApi.Controllers
{
    [RoutePrefix("api")]
    public class QuestController : ApiController
    {
        /// <summary>
        /// Posts the player's progress in the quest
        /// </summary>
        /// <param name="playerInfo">Player information</param>
        /// <returns>Player's progress in the quest thus far</returns>
        [HttpPost]
        [Route("progress")]
        [ResponseType(typeof(QuestProgress))]
        public IHttpActionResult PostPlayerProgress([FromBody]PlayerInfo playerInfo)
        {
            try
            {
                var player = playerInfo;
                var response = new QuestProgress();
                return Ok(response);
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Gets a player's state in the quest
        /// </summary>
        /// <param name="playerId">Id of the player</param>
        /// <returns>Player's overall progress</returns>
        [HttpGet]
        [Route("state")]
        [ResponseType(typeof(PlayerProgress))]
        public IHttpActionResult GetPlayerState([FromUri]string playerId)
        {
            try
            {
                var id = playerId;
                var response = new PlayerProgress();
                return Ok(response);
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }
        }
    }
}
