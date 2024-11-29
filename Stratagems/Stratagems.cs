using Newtonsoft.Json;

namespace Oxide.Plugins
{
    [Info("Stratagems", "Monarc Games", "1.0.0")]
    [Description("Call in a wide variety of powerful ordnance and war materials")]
    public class Stratagems : RustPlugin
    {
        #region Fields

        

        #endregion
        
        #region Initialisation

        

        #endregion
        
        #region Hooks

        

        #endregion
        
        #region Commands

        

        #endregion

        #region Stratagems Pod Entity

        

        #endregion

        #region Animator / Curve

        

        #endregion
        
        #region User Interface

        

        #endregion
        
        #region Data Management

        

        #endregion
        
        #region Localisation

        

        #endregion
        
        #region Configuration

        private ConfigData config;

        private class ConfigData
        {
            [JsonProperty(PropertyName = "Debug")]
            public bool debug { get; set; }
            
            [JsonProperty(PropertyName = "General Settings")]
            public GeneralSettings generalSettings { get; set; }
            
            [JsonProperty(PropertyName = "Chat Settings")]
            public ChatSettings chatSettings { get; set; }
        }

        private class GeneralSettings
        {
            [JsonProperty(PropertyName = "Main command")]
            public string mainCommand { get; set; }
        }

        private class ChatSettings
        {
            [JsonProperty(PropertyName = "Chat icon (Steam ID)")]
            public ulong iconID { get; set; }

            [JsonProperty(PropertyName = "Chat prefix")]
            public string chatPrefix { get; set; }

            [JsonProperty(PropertyName = "Chat prefix colour")]
            public string chatPrefixColour { get; set; }
        }

        private class UISettings
        {
            
        }

        private class PodSettings
        {
            [JsonProperty(PropertyName = "Allow stratagem pod to take damage")]
            public bool canTakeDamage { get; set; }
            
            [JsonProperty(PropertyName = "Stratagem pod health")]
            public float podHealth { get; set; }
        }
        
        protected override void LoadConfig()
        {
            base.LoadConfig();
            try
            {
                config = Config.ReadObject<ConfigData>();
                if (config == null)
                {
                    LoadDefaultConfig();
                }
            }
            catch
            {
                PrintError($"{Name}.json is corrupted! Recreating a new configuration");
                LoadDefaultConfig();
                return;
            }
            SaveConfig();
        }

        protected override void LoadDefaultConfig()
        {
            config = new ConfigData
            {
                debug = false,
                generalSettings = new GeneralSettings()
                {
                    mainCommand = "stratagems"
                },
                
                chatSettings = new ChatSettings()
                {
                    iconID = 0,
                    chatPrefix = "STRATAGEMS: ",
                    chatPrefixColour = "#8B4343"
                },
            };
        }


        protected override void SaveConfig() => Config.WriteObject(config);

        #endregion

    }
}