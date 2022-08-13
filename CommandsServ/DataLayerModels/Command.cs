using System.ComponentModel.DataAnnotations;

namespace CommandsServ.DataLayerModels
{
    public class Command
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string ? How { get; set; }
    
        [Required]
        public string ? Commandline { get; set; }

        [Required]    
        public int PlatformId { get; set; }

        public Platform ? Platform { get; set; }
    }
}