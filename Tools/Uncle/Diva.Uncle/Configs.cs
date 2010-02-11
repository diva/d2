namespace Framework
{
    using System.Collections.Generic;
    using Nini.Config;
    using NiniTest;

    public static class Configs
    {
        public static class OpenSim
        {
            #region Constants and Fields

            public static IniConfigSource ConfigSource;

            public static StartupCollection Startup;

            #endregion

            #region Constructors and Destructors

            public static void Load()
            {
                Load("OpenSim.ini");
            }

            public static void Load(string openSimFilename)
            {
                ConfigSource = new IniConfigSource(openSimFilename);
                Startup = new StartupCollection();
            }

            #endregion

            public class StartupCollection : Dictionary<string, ConfigValueWrapper>
            {                
                #region Constants and Fields

                public readonly IConfig Config;

                #endregion

                #region Constructors and Destructors

                public StartupCollection()
                {
                    this.Config = ConfigSource.Configs["Startup"];

                    this.Add("gridmode");
                    this.Add("save_crashes");
                    this.Add("NonPhysicalPrimMax");
                }

                #endregion

                #region Public Methods

                public ConfigValueWrapper Add(string key)
                {
                    ConfigValueWrapper value = new ConfigValueWrapper(this.Config, key);
                    this.Add(key, value);
                    return value;
                }

                #endregion
            }
        }
    }
}