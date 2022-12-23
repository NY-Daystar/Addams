using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addams.Entities
{
    public class Image
    {
        public int? height { get; set; }
        public string? url { get; set; }
        public int? width { get; set; }
    }
}
