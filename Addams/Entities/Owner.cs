﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addams.Entities
{
    public class Owner
    {
        public string? display_name { get; set; }
        public ExternalUrls? external_urls { get; set; }
        public string? href { get; set; } 
        public string? id { get; set; }
        public string? type { get; set; }
        public string? uri { get; set; }
    }
}
