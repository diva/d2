namespace Diva.Uncle.Windows.StartupTab
{
    using System;
    using System.Windows.Forms;
    using Diva.Windows;
    using Framework;
    using Framework.Windows;
    using NiniTest;

    public partial class StartupTab : UserControl, IWindowsAddin
    {
        #region Constructors and Destructors

        public StartupTab()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Implemented Interfaces

        #region IWindowsAddin

        public void Initialize(IWindowsAddinHost host)
        {
            TabPage page = new TabPage("Startup");
            page.Controls.Add(this);

            host.AddTab(page);
        }

        #endregion

        #endregion

        #region Methods

        private void saveButton_Click(object sender, EventArgs e)
        {
            Configs.OpenSim.ConfigSource.Save();
        }

        private void StartupTab_Load(object sender, EventArgs e)
        {
            new CheckBoxController(this.m_saveCrashesCheckBox, Configs.OpenSim.Startup["save_crashes"]);
            new IntFieldController(this.m_nonPhysicalPrimMax, Configs.OpenSim.Startup["NonPhysicalPrimMax"]);
        }

        #endregion
    }
}