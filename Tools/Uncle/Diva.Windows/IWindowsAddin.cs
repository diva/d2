namespace Diva.Windows
{
    using Framework.Windows;

    public interface IWindowsAddin
    {
        #region Public Methods

        void Initialize(IWindowsAddinHost host);

        #endregion
    }
}