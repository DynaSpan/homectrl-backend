using System.Collections.Generic;

namespace HomeCTRL.Backend.Core.Models
{
    public class PluginSettings
    {
        public IEnumerable<PluginRegister> RegisteredPlugins { get; set; }
    }
}