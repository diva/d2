namespace NiniTest
{
    using System;
    using Nini.Config;

    public class ConfigValueWrapper
    {
        #region Constants and Fields

        private readonly IConfig m_config;

        private readonly string m_key;

        #endregion

        #region Constructors and Destructors

        public ConfigValueWrapper(IConfig config, string key)
        {
            this.m_config = config;
            this.m_key = key;
        }

        #endregion

        #region Public Methods

        public bool TryClearValue()
        {
            try
            {
                this.m_config.Remove(this.m_key);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TryGetValue(out bool value)
        {
            try
            {
                value = this.m_config.GetBoolean(this.m_key);
                return true;
            }
            catch (Exception)
            {
                value = default(bool);
                return false;
            }
        }

        public bool TryGetValue(out int value)
        {
            try
            {
                value = this.m_config.GetInt(this.m_key);
                return true;
            }
            catch (Exception)
            {
                value = default(int);
                return false;
            }
        }

        public bool TrySetValue(object value)
        {
            try
            {
                this.m_config.Set(this.m_key, value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}