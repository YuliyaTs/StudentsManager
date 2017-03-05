using System.Windows.Controls;
using StudentsManager.Helper;

namespace StudentsManager.View.Base
{
    public class BaseView : UserControl, IBaseView
    {
        #region Constructor

        public BaseView()
        {
        }

        #endregion

        #region Properties

        public string ViewName { get; set; }
        public string Header { get; set; }

        #endregion
    }
}
