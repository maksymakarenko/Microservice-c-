using AutoMapper;
using Commands;
using CommandsServ.DataLayerModels;
using Grpc.Net.Client;

namespace CommandsServ.SyncDS.Grpc
{
    public class PlatformDC : IPlatformDC
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PlatformDC(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public IEnumerable<Platform> ReturnPlatforms()
        {
            var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"]);
            var client = new CommandsService.CommandsServiceClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllCommandss(request);
                return _mapper.Map<IEnumerable<Platform>>(reply.Commands);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"--> Couldn't Call Grpc {ex.Message}");
                return null;
            }
        }
    }
}