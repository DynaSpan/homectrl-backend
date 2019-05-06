using System;
using System.Threading.Tasks;

namespace HomeCTRL.Plugin
{
    public interface IPlugin : IDisposable
    {
        /// <summary>
        /// The startup function where the plugin can do its things.
        /// Reports true when succesfull; false otherwise
        /// </summary>
        /// <returns></returns>
        Task<bool> Startup();
    }
}
