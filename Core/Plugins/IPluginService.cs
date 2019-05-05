using System.Threading.Tasks;

namespace HomeCTRL.Backend.Core.Plugins
{
    public interface IPluginService
    {
        /// <summary>
        /// Loads the registered plugins and activates them
        /// </summary>
        /// <returns>true when succesfull; false otherwise</returns>
        Task<bool> LoadPlugins();
    }
}