using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using HomeCTRL.Backend.Core.Logging;
using HomeCTRL.Backend.Core.Models;
using HomeCTRL.Plugin;

namespace HomeCTRL.Backend.Core.Plugins
{
    public class PluginService : IPluginService
    {
        private readonly string TAG = "PluginService";
        private readonly PluginSettings pluginSettings;

        public PluginService(PluginSettings pluginSettings)
        {
            this.pluginSettings = pluginSettings;
        }

        public async Task<bool> LoadPlugins()
        {
            Log.Info(this.TAG, "Loading plugins...");

            PluginRegister currentPlugin = null; // store for debugging purposes

            try
            {
                foreach (PluginRegister plugin in this.pluginSettings.RegisteredPlugins)
                {
                    currentPlugin = plugin;

                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.GetFullPath(plugin.Path));
                    var pluginClass = assembly.GetType(string.Format("{0}.Plugin", plugin.RootNamespace));
                    var pluginInstance = Activator.CreateInstance(pluginClass) as IPlugin;
        
                    // Call the startup function
                    var pluginStatus = await pluginInstance.Startup();

                    if (pluginStatus)
                    {
                        Log.Info(this.TAG, string.Format("Successfully registered plugin {0}", plugin.Name));
                    } else {
                        Log.Error(this.TAG, string.Format("Plugin startup of {0} did not complete properly", plugin.Name));
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                if (currentPlugin != null)
                {
                    Log.Error(this.TAG, $"Error happened during loading of plugin {currentPlugin.Name}");
                } 
                else 
                {
                    Log.Error(this.TAG, "Error occurred during plugin loading");
                }

                Log.Error(this.TAG, e.Message);
                Log.Error(this.TAG, e.StackTrace);

                return false;
            }
        }
    }
}