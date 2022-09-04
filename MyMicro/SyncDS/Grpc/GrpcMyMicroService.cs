using AutoMapper;
using Grpc.Core;
using MyMicro.Data;

namespace MyMicro.SyncDS.Grpc
{
    public class GrpcMyMicroService : MyMicroService.MyMicroServiceBase
    {
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;

        public GrpcMyMicroService(IPlatformRepo platformRepo, IMapper mapper)
        {
            _platformRepo = platformRepo;
            _mapper = mapper;
        }

        public override Task<MyMicroResponse> GetAllMyMicros(GetAllRequest request, ServerCallContext context)
        {
            var response = new MyMicroResponse();
            var platforms = _platformRepo.GetAllPlatforms();

            foreach (var platform in platforms)
            {
                response.MyMicro.Add(_mapper.Map<MyMicroModel>(platform));
            }

            return Task.FromResult(response);
        }
    }
}