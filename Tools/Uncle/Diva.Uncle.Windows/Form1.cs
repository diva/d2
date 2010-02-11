namespace NiniTest
{
    using System.Windows.Forms;
    using Diva.Windows;
    using Framework.Windows;
    using Mono.Addins;

    public partial class Form1 : Form, IWindowsAddinHost
    {
        #region Constructors and Destructors

        public Form1()
        {
            this.InitializeComponent();

            AddinManager.Initialize();
            AddinManager.Registry.Update(null);

            ExtensionNodeList nodes = AddinManager.GetExtensionNodes("/NiniTest/Modules");

            foreach (TypeExtensionNode node  in nodes)
            {
                IWindowsAddin addin = (IWindowsAddin)node.CreateInstance();

                addin.Initialize(this);
            }

            // StartupTab tab = new StartupTab();
            // tab.Initialize( this );
        }

        #endregion

        #region Implemented Interfaces

        #region IWindowsAddinHost

        public void AddTab(TabPage page)
        {
            this.m_tabControl.TabPages.Add(page);
        }

        #endregion

        #endregion
    }
}