using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Win32;
using StudentsManager.Helper;
using StudentsManager.Model;
using StudentsManager.Repositories;
using StudentsManager.Services.Interaction;
using StudentsManager.Services.Local;
using StudentsManager.View;
using StudentsManager.View.Base;
using MessageBox = System.Windows.MessageBox;

namespace StudentsManager.ViewModel.Students
{
    public class StudentsViewModel : BaseViewModel
    {
        #region Fields

        private readonly XmlStudentRepository _studentRepository;
        private readonly XmlUniversitiesRepository _universityRepository;
        private readonly XmlFacultiesRepository _facultyRepository;
        private ObservableCollection<Student> _students;
        private Student _selectedStudent;
        private IList _selectedStudents;
        private InteractionService<Student> _interactionService;
        private IEditingService<Student> _localService;

        #endregion

        #region Constructors

        public StudentsViewModel()
        {
            _studentRepository = new XmlStudentRepository();
            _universityRepository = new XmlUniversitiesRepository();
            _facultyRepository = new XmlFacultiesRepository();
            _students = new ObservableCollection<Student>(_studentRepository.GetAll());
            _selectedStudent = new Student();
            _localService = new StudentsEditingService();

            CanClose = true;
        }

        public StudentsViewModel(InteractionService<Student> interactionService, IList selectedStudents)
            : this()
        {
            _interactionService = interactionService;
            _selectedStudents = selectedStudents;

            _interactionService.CanCloseView += canCloseNewValue =>
            {
                CanClose = canCloseNewValue;
                OnPropertyChanged("CanClose");
            };

            _interactionService.CollectionChanged += () =>
            {
                OnPropertyChanged("Students");
            };
        }

        #endregion

        #region Properties

        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged();
            }
        }

        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();
            }
        }

        public IList SelectedStudents
        {
            get { return _selectedStudents; }
            set
            {
                _selectedStudents = value;
                OnPropertyChanged();
            }
        }

        public BaseView StudentViewContent { get; set; }

        public Action<BaseView> OnOpenNewTab { get; set; }

        #endregion

        #region Commands

        protected override ICommand GetAddCommand()
        {
            return new DelegateCommand<object>(ExecuteAddCommand, CanExecuteAddCommand, true);
        }

        protected override ICommand GetEditCommand()
        {
            return new DelegateCommand<object>(ExecuteEditCommand, CanExecuteEditCommand, true);
        }

        protected override ICommand GetDeleteCommand()
        {
            return new DelegateCommand<object>(ExecuteDeleteCommand, CanExecuteDeleteCommand, true);
        }

        protected override ICommand GetRefreshCommand()
        {
            return new DelegateCommand<object>(ExecuteRefreshCommand, CanExecuteRefreshCommand, true);
        }

        protected override ICommand GetSaveCommand()
        {
            return new DelegateCommand<object>(ExecuteSaveCommand, CanExecuteSaveCommand, true);
        }

        protected override ICommand GetImportCommand()
        {
            return new DelegateCommand<object>(ExecuteImportCommand, CanExecuteImportCommand, true);
        }

        protected override ICommand GetExportCommand()
        {
            return new DelegateCommand<object>(ExecuteExportCommand, CanExecuteExportCommand, true);
        }

        #endregion

        #region Methods

        #region EditCommand

        private void ExecuteEditCommand(object obj)
        {
            _interactionService.ShowEditView(this, SelectedStudent, _students, _universityRepository, _facultyRepository);
        }

        private bool CanExecuteEditCommand(object arg)
        {
            return SelectedStudents.Count == 1;
        }

        #endregion

        #region AddCommand

        private void ExecuteAddCommand(object obj)
        {
            _interactionService.ShowAddView(this, _students, _universityRepository, _facultyRepository);
        }

        private bool CanExecuteAddCommand(object arg)
        {
            return true;
        }

        #endregion

        #region DeleteCommand

        private void ExecuteDeleteCommand(object obj)
        {
            var listOfSelectedStudents = (from object student in _selectedStudents select student as Student).ToList();

            _localService.Delete(_students, listOfSelectedStudents);

            OnPropertyChanged("Students");
            CanClose = false;
        }

        private bool CanExecuteDeleteCommand(object arg)
        {
            return _selectedStudents.Count >= 1;
        }

        #endregion

        #region RefreshCommand

        private void ExecuteRefreshCommand(object obj)
        {
            _students = new ObservableCollection<Student>(_studentRepository.GetAll());
            OnPropertyChanged("Students");
        }

        private bool CanExecuteRefreshCommand(object arg)
        {
            return true;
        }

        #endregion

        #region SaveCommand

        private void ExecuteSaveCommand(object obj)
        {
            if(!_studentRepository.Save(_students))
                MessageBox.Show("Error while saving data into file", "Students Manager",
                                 MessageBoxButton.OK, MessageBoxImage.Warning);
            CanClose = true;
        }

        private bool CanExecuteSaveCommand(object arg)
        {
            return !CanClose;
        }

        #endregion

        #region ImportCommand

        private void ExecuteImportCommand(object obj)
        {
            var openDialog = new OpenFileDialog()
            {
                Filter = "XML Files(*.xml)|*.xml|All(*.*)|*"
            };

            if (openDialog.ShowDialog() == true)
                _students = new ObservableCollection<Student>(_studentRepository.GetAll(openDialog.FileName));

            OnPropertyChanged("Students");

        }

        private bool CanExecuteImportCommand(object arg)
        {
            return true;
        }

        #endregion

        #region ExportCommand

        private void ExecuteExportCommand(object obj)
        {
            var saveDialog = new SaveFileDialog()
            {
                Filter = "Text Files(*.txt)|*.txt|XML Files(*.xml)|*.xml|All(*.*)|*"
            };

            if (saveDialog.ShowDialog() == true)
                _studentRepository.Save(_students, saveDialog.FileName);
        }

        private bool CanExecuteExportCommand(object arg)
        {
            return true;
        }

        #endregion

        #endregion
    }
}
