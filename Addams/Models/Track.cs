using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addams.Models
{
    public class Track
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Track(int id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }
    }
}
