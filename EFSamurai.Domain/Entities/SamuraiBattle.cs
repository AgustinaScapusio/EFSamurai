using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFSamurai.Domain.Entities
{
    public class SamuraiBattle
    { 
        public int SamuraiId { get; set; }
        public int BattleId { get; set; }
        public Samurai? Samurai { get; set; }
        public Battle? Battle { get; set; }

    }
}
