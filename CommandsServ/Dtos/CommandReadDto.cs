namespace CommandsServ.Dtos
{
    public class CommandReadDto
    {
        public int Id { get; set; }
        public string ? How { get; set; }
        public string ? Commandline { get; set; } 
        public int PlatformId { get; set; }
    }
}