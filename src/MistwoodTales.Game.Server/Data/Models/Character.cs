using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MistwoodTales.Game.Server.Data.Models
{
    public class Character
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public int Level { get; set; }
        [Required]
        public int XP { get; set; }

        [Required]
        public int HP { get; set; }
        [Required]
        public int MaxHP { get; set; }

        [Required]
        public int Gold { get; set; }

        public int? MapId { get; set; }
        [ForeignKey(nameof(MapId))]
        public Map Map { get; set; }

        public int AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }
    }
}
