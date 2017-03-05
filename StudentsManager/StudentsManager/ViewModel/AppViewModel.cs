using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using StudentsManager.Helper;
using StudentsManager.Model;
using StudentsManager.Repositories;
using StudentsManager.Services.Interaction;
using StudentsManager.View;
using StudentsManager.View.Base;
using StudentsManager.View.NewFeatures;
using StudentsManager.View.Student;
using StudentsManager.ViewModel.Students;
using StudentsManager.ViewModel.Universities;

namespace StudentsManager.ViewModel
{
    public class AppViewModel : INotifyPropertyChanged
    {
        #region Fields

        private const string StudentsViewName = "Students";
        private const string UniversitiesViewName = "Universities";
        private const string EditStudentViewName = "Editing student";
        private const string EditUniversityViewName = "Editing university";
        private const string EditFacultyViewName = "Editing faculty";
        private const string AddStudentsViewName = "New student";
        private const string AddUniversityViewName = "New university";
        private const string AddFacultyViewName = "New faculty";
        private const string StudentsSwitchViewName = "Students TreeView";
        private readonly ObservableCollection<BaseView> _viewsInList;
        private readonly IList<BaseView> _studentsViews;
        private BaseView _selectedView;
        private ObservableCollection<ClosableTab> _viewsInTab;
        private ClosableTab _selectedTab;
        private ClosableTab _studentViewTab;

        #endregion

        #region Constructors

        public AppViewModel()
        {
            _viewsInTab = new ObservableCollection<ClosableTab>();

            var studentInteractionservice = new InteractionService<Student>(_viewsInTab);

            studentInteractionservice.ClosingView += () =>
            {
                IsActionButtonEnabled = true;
                OnPropertyChanged("IsActionButtonEnabled"); ;
            };

            studentInteractionservice.SelectionChanged += (s, view) =>
            {
                SelectedView = view;
            };

            _studentsViews = new List<BaseView>()
            {
                new StudentsView(studentInteractionservice) {Header = StudentsViewName, ViewName = StudentsViewName},
                new StudentsSwitchView(studentInteractionservice) {Header = StudentsViewName, ViewName = StudentsSwitchViewName}
            };

            var universityInteractionService = new InteractionService<University>(_viewsInTab);
            var facultyInteractionService = new InteractionService<Model.Faculty>(_viewsInTab);

            universityInteractionService.ClosingView += () =>
            {
                IsActionButtonEnabled = true;
                OnPropertyChanged("IsActionButtonEnabled"); ;
            };

            universityInteractionService.SelectionChanged += (s, view) =>
            {
                SelectedView = view;
            };

            facultyInteractionService.ClosingView += () =>
            {
                IsActionButtonEnabled = true;
                OnPropertyChanged("IsActionButtonEnabled"); ;
            };

            facultyInteractionService.SelectionChanged += (s, view) =>
            {
                SelectedView = view;
            };

            _viewsInList = new ObservableCollection<BaseView>
            {
                _studentsViews.First(),
                new UniversitiesView(universityInteractionService, facultyInteractionService) {Header = UniversitiesViewName, ViewName = UniversitiesViewName}
            };

            SwitchViewRibbonGroupVisibility = Visibility.Hidden;

            SwitchStudentViewCommand = new DelegateCommand<object>(ExecuteSwitchStudentViewCommand, CanExecuteSwitchStudentViewCommand, true);
            CloseAppCommand = new DelegateCommand<object>(ExecuteCloseAppCommand, CanExecuteCloseAppCommand, true);

            SelectedView = _viewsInList.First();
        }

        #endregion

        #region Properties

        public BaseView SelectedView
        {
            get { return _selectedView; }
            set
            {
                IsActionButtonEnabled = false;
                OnPropertyChanged("IsActionButtonEnabled");

                if (value == null) return;

                if (!DoViewExistInTabs(value.Header))
                {
                    AddViewToTabs(value, value.Header);
                }

                _selectedView = value;

                _selectedTab = _viewsInTab.FirstOrDefault(tab => tab.Content.Equals(value));
                OnPropertyChanged("SelectedTab");

                if (!(value.ViewName.Equals(EditStudentViewName)
                      || value.ViewName.Equals(EditUniversityViewName)
                      || value.ViewName.Equals(EditFacultyViewName)
                      || value.ViewName.Equals(AddStudentsViewName)
                      || value.ViewName.Equals(AddUniversityViewName)
                      || value.ViewName.Equals(AddFacultyViewName)))

                {
                    OnPropertyChanged("SelectedViewModel");

                    IsActionButtonEnabled = true;
                    OnPropertyChanged("IsActionButtonEnabled");
                }

                SwitchViewRibbonGroupVisibility = value.Header != UniversitiesViewName
                    ? Visibility.Visible
                    : Visibility.Hidden;
                OnPropertyChanged("SwitchViewRibbonGroupVisibility");

                OnPropertyChanged("IsActionButtonEnabled");

                OnPropertyChanged();
            }
        }

        public BaseViewModel SelectedViewModel
        {
            get { return SelectedView == null ? null : (BaseViewModel)SelectedView.DataContext; }
        }

        public ObservableCollection<BaseView> ViewsInList
        {
            get
            {
                return _viewsInList;
            }
        }

        public ObservableCollection<ClosableTab> ViewsInTab
        {
            get { return _viewsInTab; }
            set
            {
                _viewsInTab = value;

                if(_viewsInTab.Count == 0)
                    IsActionButtonEnabled = false;
                OnPropertyChanged("IsActionButtonEnabled");

                OnPropertyChanged();
            }
        }

        public ClosableTab SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                var newValue = value;

                if (value == null)
                {
                    var selectedViewName = SelectedView.ViewName;

                    if (selectedViewName.Equals(EditStudentViewName) || selectedViewName.Equals(AddStudentsViewName))
                        newValue = _viewsInTab.FirstOrDefault(tab => ((BaseView) tab.Content).ViewName.Equals(StudentsViewName));

                    else if (selectedViewName.Equals(EditUniversityViewName) || selectedViewName.Equals(AddUniversityViewName))
                        newValue = _viewsInTab.FirstOrDefault(tab => ((BaseView)tab.Content).ViewName.Equals(UniversitiesViewName));
                }

                _selectedTab = newValue;
                OnPropertyChanged();

                var viewName = ((ClosableHeader) _selectedTab.Header).View.ViewName;

                if (!(viewName.Equals(EditStudentViewName)
                      || viewName.Equals(EditUniversityViewName)
                      || viewName.Equals(EditFacultyViewName)
                      || viewName.Equals(AddStudentsViewName)
                      || viewName.Equals(AddUniversityViewName)
                      || viewName.Equals(AddFacultyViewName)))
                {
                    IsActionButtonEnabled = true;
                    OnPropertyChanged("IsActionButtonEnabled");
                }

                SelectedView = newValue != null ? (BaseView)newValue.Content : null ;
                OnPropertyChanged("SelectedView");
            }
        }

        public ICommand SwitchStudentViewCommand
        {
            get;
            private set;
        }

        public ICommand CloseAppCommand
        {
            get;
            private set;
        }

        public Visibility SwitchViewRibbonGroupVisibility { get; set; }

        public bool IsActionButtonEnabled { get; set; }

        #endregion

        #region Methods

        public bool DoViewExistInTabs(string header)
        {
            return _viewsInTab.Any(tab => ((BaseView)tab.Content).Header.Equals(header));
        }

        public void AddViewToTabs(BaseView view, string header)
        {
            ClosableTab newTab;

            if (header == StudentsViewName)
            {
                newTab = view.ViewName == StudentsViewName ?
                                                            new ClosableTab(view, _viewsInTab, ((StudentsViewModel)view.DataContext).Students)
                                                          : new ClosableTab(view, _viewsInTab, ((StudentsSwitchViewModel)view.DataContext).Universities);


                _studentViewTab = newTab;
            }

            else if (header == UniversitiesViewName)
                newTab = new ClosableTab(view, _viewsInTab, ((UniversitiesViewModel)view.DataContext).Universities);

            else
                newTab = new ClosableTab(view, _viewsInTab);

            newTab.Content = view;
            _viewsInTab.Add(newTab);

        }

        private bool CanExecuteSwitchStudentViewCommand(object arg)
        {
            return SelectedView != null && SelectedView.Header == StudentsViewName;
        }

        private void ExecuteSwitchStudentViewCommand(object obj)
        {
            var indexOfStudentView =
                _viewsInList.IndexOf(_viewsInList.FirstOrDefault(view => view.Header.Equals(StudentsViewName) || view.Header.Equals(StudentsSwitchViewName)));

            _viewsInList[indexOfStudentView] = _studentsViews.FirstOrDefault(studentView => !studentView.Equals(_viewsInList[indexOfStudentView]));

            _selectedView = _viewsInList[indexOfStudentView];
            OnPropertyChanged("SelectedView");

            _studentViewTab.Content = _viewsInList[indexOfStudentView];
            _selectedTab = _studentViewTab;
            OnPropertyChanged("SelectedTab");
        }

        private bool CanExecuteCloseAppCommand(object arg)
        {
            return true;
        }

        private void ExecuteCloseAppCommand(object obj)
        {
            if (ViewsInTab != null && ViewsInTab.Contains(_studentViewTab) && !((StudentsViewModel)_studentsViews.First().DataContext).CanClose)
            {
                var result = MessageBox.Show("Do You want to save changes ?", "Students Manager",
                       MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                
                switch (result)
                {
                    case MessageBoxResult.Cancel:
                        return;

                    case MessageBoxResult.Yes:
                        var repository = new XmlStudentRepository();
                        repository.Save(((StudentsViewModel)_studentsViews.First().DataContext).Students);
                        break;
                }
            }

            Application.Current.Shutdown();
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
