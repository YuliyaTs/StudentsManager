using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using StudentsManager.Helper;
using StudentsManager.Repositories;
using StudentsManager.View.Base;
using StudentsManager.ViewModel;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace StudentsManager.View.NewFeatures
{
    /// <summary>
    /// Interaction logic for CloseableHeader.xaml
    /// </summary>
    public partial class ClosableHeader : UserControl
    {
        #region Fields

        private const string StudentsViewName = "Students";
        private const string StudentsSwitchViewName = "Students TreeView";
        private readonly ObservableCollection<ClosableTab> _viewsInTab;
        private readonly ClosableTab _tab;
        private IList _listOfInstances;
        private BaseView _view;

        #endregion

        #region Constructor

        public ClosableHeader(BaseView view, ObservableCollection<ClosableTab> viewsInTab, ClosableTab tab, IList listOfInstances)
        {
            InitializeComponent();
            DataContext = this;

            _view = view;
            _viewsInTab = viewsInTab;
            _tab = tab;
            _listOfInstances = listOfInstances;

            //if (_view.GetType() == typeof(StudentEditView))
            //    ((StudentEditViewModel) _view.DataContext).CloseView += () =>
            //    {
            //        CloseCommand.Execute(null);
            //    };

            //else if (_view.GetType() == typeof(UniversityEditView))
            //    ((UniversitiesEditViewModel)_view.DataContext).CloseView += () =>
            //    {
            //        CloseCommand.Execute(null);
            //    };

            CloseCommand = new DelegateCommand<object>(ExecuteCloseCommand, CanExecuteCloseCommand, true);
        }

        #endregion

        #region Delegates

        public delegate void ClosingViewHandler();

        #endregion

        #region Event

        public event ClosingViewHandler ClosingEditView = delegate { };

        #endregion

        #region Properties

        public string ViewHeader
        {
            get { return _view.Header; } 
            set { _view.Header = value; }
        }

        public BaseView View
        {
            get { return _view; }
        }

        public ICommand CloseCommand { get; set; }

        #endregion

        #region Methods

        private bool CanExecuteCloseCommand(object arg)
        {
            return true;
        }

        private void ExecuteCloseCommand(object obj)
        {
            if (_listOfInstances != null)
            {
                var canCloseView = ((BaseViewModel)_view.DataContext).CanClose;
                
                if (!canCloseView)
                {
                    ((BaseViewModel) _view.DataContext).CanClose = true;
                    
                    var result = MessageBox.Show("Do You want to save changes ?", "Students Manager",
                        MessageBoxButton.YesNoCancel, MessageBoxImage.Question);


                    switch (result)
                    {
                        case MessageBoxResult.Cancel:
                            return;
                        case MessageBoxResult.Yes:

                            var typeOfInstanceInList = _listOfInstances[0].GetType();

                            if (typeOfInstanceInList == typeof(Model.Student))
                            {
                                var repository = new XmlStudentRepository();
                                var typedList = new List<Model.Student>();

                                if(_view.ViewName == StudentsViewName)
                                    typedList.AddRange(from object student in _listOfInstances select student as Model.Student);

                                else
                                    foreach (var university in _listOfInstances)
                                    {
                                        foreach (var faculty in ((Model.University)university).Faculties)
                                        {
                                            typedList.AddRange(faculty.Students.Select(student => student));
                                        }
                                    }


                                repository.Save(typedList);
                            }

                            break;
                    }
                }
            }

            else
                ClosingEditView();

            _viewsInTab.Remove(_tab);
            
        }

        #endregion
    }
}
