using StudentsManager.Helper;

namespace StudentsManager.View.Base
{
    interface IBaseView
    {
        #region Properties

        string ViewName { get; set; }
        string Header { get; set; }

        #endregion
    }
}
