using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Clinic.API.Installer
{
    public interface IInstaller
    {
        void InstallServicesAssembly(IServiceCollection services, IConfiguration configuration);
    }
}