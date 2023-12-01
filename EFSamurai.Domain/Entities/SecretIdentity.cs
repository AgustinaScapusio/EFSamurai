using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFSamurai.Domain.Entities
{
    public class SecretIdentity
    {
        public int Id { get; set; }
        public string? RealName { get; set; }
        public int SamuraiID { get; set; }
    }
}
