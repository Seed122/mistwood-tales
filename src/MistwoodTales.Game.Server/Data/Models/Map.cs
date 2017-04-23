using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MistwoodTales.Game.Server.Data.Models
{
    public class Map
    {
        [Key]
        public int Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
