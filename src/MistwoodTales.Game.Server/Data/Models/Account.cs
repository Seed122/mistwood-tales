using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MistwoodTales.Game.Server.Data.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public virtual List<Character> Characters { get; set; }
    }
}
