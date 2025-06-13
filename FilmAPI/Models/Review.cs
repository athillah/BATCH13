using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FilmAPI.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public float? Rate { get; set; } = null;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public int FilmId { get; set; }
        
        [JsonIgnore]
        public Film? Film { get; set; }
    }
}