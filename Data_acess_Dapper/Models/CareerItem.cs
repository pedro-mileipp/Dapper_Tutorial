using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaltaDataAccess.Models
{
    public class CareerItem
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public Course? Course { get; set; }
    }
}