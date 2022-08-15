using System.ComponentModel.DataAnnotations;

namespace CommandsServ.Dtos
{
    public class CommandCreateDto
    {
        [Required]
        public string ? How { get; set; }
    
        [Required]
        public string ? Commandline { get; set; }
    }
}