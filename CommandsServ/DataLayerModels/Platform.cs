using System.ComponentModel.DataAnnotations;

namespace CommandsServ.DataLayerModels
{
    public class Platform
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string ? Name { get; set; }
        
        [Required]
        public int ExternalId { get; set; }

        public ICollection<Command> Commands {get; set; } = new List<Command>();
    }
}