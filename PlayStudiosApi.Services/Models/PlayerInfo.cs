using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlayStudiosApi.Services.Models
{
    public class PlayerInfo
    {
        public string PlayerId { get; set; }
        public int PlayerLevel { get; set; }
        public int ChipAmountBet { get; set; }
    }
}