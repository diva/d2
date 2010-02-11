namespace NiniTest
{
    using System;
    using System.Windows.Forms;

    public class IntFieldController
    {
        #region Constants and Fields

        private readonly ConfigValueWrapper m_valueWrapper;

        private readonly TextBox m_view;

        #endregion

        #region Constructors and Destructors

        public IntFieldController(TextBox view, ConfigValueWrapper valueWrapper)
        {
            this.m_view = view;
            this.m_valueWrapper = valueWrapper;

            int value;

            if (this.m_valueWrapper.TryGetValue(out value))
            {
                this.m_view.Text = value.ToString();
            }

            view.TextChanged += this.ViewCheckStateChanged;
        }

        #endregion

        #region Methods

        private void ViewCheckStateChanged(object sender, EventArgs e)
        {
            this.m_valueWrapper.TrySetValue(this.m_view.Text);
        }

        #endregion
    }
}