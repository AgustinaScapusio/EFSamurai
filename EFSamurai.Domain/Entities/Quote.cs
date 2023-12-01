﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFSamurai.Domain.Entities
{
    public class Quote
    {
        public int Id { get; set; }
        public string Text{ get; set; } = string.Empty;
        public QuoteStyle QuoteStyle { get; set; }
    }
}
