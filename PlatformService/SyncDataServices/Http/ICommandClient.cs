using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
    public interface ICommandClient
    {
        Task SendPlatformToCommand(PlatformReadDto plat);
    }
}
