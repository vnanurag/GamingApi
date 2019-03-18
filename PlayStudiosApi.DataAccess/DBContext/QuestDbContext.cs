using PlayStudiosApi.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayStudiosApi.DataAccess.DBContext
{
    public class QuestDbContext : DbContext
    {
        public DbSet<Quest> Quests { get; set; }
    }
}
