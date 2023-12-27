using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Models
{
    public class Producer
    {
        [Key]
        public int ID { get; set; }

        public string ProfilePictureURl { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }
    }
}
