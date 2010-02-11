namespace NiniTest
{
    using System;
    using System.Windows.Forms;

    public class CheckBoxController
    {
        #region Constants and Fields

        private readonly ConfigValueWrapper m_valueWrapper;

        private readonly CheckBox m_view;

        #endregion

        #region Constructors and Destructors

        public CheckBoxController(CheckBox view, ConfigValueWrapper valueWrapper)
        {
            this.m_view = view;
            this.m_valueWrapper = valueWrapper;

            bool saveCrashes;

            if (this.m_valueWrapper.TryGetValue(out saveCrashes))
            {
                this.m_view.Checked = saveCrashes;
            }
            else
            {
                this.m_view.CheckState = CheckState.Indeterminate;
            }

            view.CheckStateChanged += this.ViewCheckStateChanged;
        }

        #endregion

        #region Methods

        private void ViewCheckStateChanged(object sender, EventArgs e)
        {
            if (this.m_view.CheckState == CheckState.Indeterminate)
            {
                this.m_valueWrapper.TryClearValue();
            }
            else
            {
                this.m_valueWrapper.TrySetValue(this.m_view.Checked);
            }
        }

        #endregion
    }
}