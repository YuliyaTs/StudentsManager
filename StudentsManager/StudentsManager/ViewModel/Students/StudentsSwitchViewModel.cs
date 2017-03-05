using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using StudentsManager.Helper;
using StudentsManager.Model;
using StudentsManager.Repositories;
using StudentsManager.Services.Interaction;
using StudentsManager.Services.Local;

namespace StudentsManager.ViewModel.Students
{
    public class StudentsSwitchViewModel : BaseViewModel
    {
        #region Fields

        private readonly IRepository<University> _universityRepository;
        private readonly IRepository<Model.Faculty> _facultyRepository;
        private readonly IRepository<Student> _studentRepository;
        private ObservableCollection<University> _universities;
        private ObservableCollection<Student> _students;
        private IList<Model.Faculty> _faculties;
        private Student _selectedStudent;
        private InteractionService<Student> _interactionService;
        private IEditingService<Student> _localService;

        #endregion

        #region Constructors

        public StudentsSwitchViewModel()
        {
            _universityRepository = new XmlUniversitiesRepository();
            _facultyRepository = new XmlFacultiesRepository();
            _studentRepository = new XmlStudentRepository();

            _universities = new ObservableCollection<University>(_universityRepository.GetAll());
            _faculties = new List<Model.Faculty>(_facultyRepository.GetAll());
            _students = new ObservableCollection<Student>(_studentRepository.GetAll());

            CategorizeStudents();

            _selectedStudent = new Student();
            _localService = new StudentsEditingService();

        }

        public StudentsSwitchViewModel(InteractionService<Student> interactionService)
            : this()
        {
            _interactionService = interactionService;
        }

        #endregion

        #region Properties

        public ObservableCollection<University> Universities
        {
            get { return _universities; }
            set
            {
                _universities = value;
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
            return SelectedStudent != null;
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
            var listOfSelectedStudents = new List<Student>();

            listOfSelectedStudents.Add(SelectedStudent);

            _localService.Delete(_students, listOfSelectedStudents);

            CategorizeStudents();

        }

        private bool CanExecuteDeleteCommand(object arg)
        {
            return SelectedStudent != null;
        }

        #endregion

        #region RefreshCommand

        private void ExecuteRefreshCommand(object obj)
        {
            _students = new ObservableCollection<Student>(_studentRepository.GetAll());
            OnPropertyChanged("Universities");
        }

        private bool CanExecuteRefreshCommand(object arg)
        {
            return true;
        }

        #endregion

        #region SaveCommand

        private void ExecuteSaveCommand(object obj)
        {
            if (!_studentRepository.Save(_students))
                MessageBox.Show("Error while saving data into file", "Students Manager",
                                 MessageBoxButton.OK, MessageBoxImage.Warning);
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

            CategorizeStudents();

            OnPropertyChanged("Universities");

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


        private void FillUniversitiesWithFaculties()
        {
            foreach (var university in _universities)
            {
                if (university.Faculties != null)
                    university.Faculties.Clear();

                var facultiesOfCurrentUniversity = new ObservableCollection<Model.Faculty>();

                foreach (var faculty in _faculties)
                {
                    if (faculty.University != null && faculty.University.Id == university.Id)
                        facultiesOfCurrentUniversity.Add(faculty);
                }

                foreach (var faculty in facultiesOfCurrentUniversity)
                {
                    if (university.Faculties != null)
                        university.Faculties.Add(faculty);
                }
            }
        }

        private void FillFacultiesWithStudents()
        {
            foreach (var university in _universities)
            {
                foreach (var faculty in university.Faculties)
                {
                    if (faculty.Students != null)
                        faculty.Students.Clear();

                    var studentsOfCurrentFaculty = new ObservableCollection<Student>();

                    foreach (var student in _students)
                    {
                        if (student.Faculty != null && student.Faculty.Id == faculty.Id)
                            studentsOfCurrentFaculty.Add(student);
                    }

                    foreach (var student in studentsOfCurrentFaculty)
                    {
                        if (faculty.Students != null)
                            faculty.Students.Add(student);
                    }
                }
            }
        }

        private void CategorizeStudents()
        {
            FillUniversitiesWithFaculties();
            FillFacultiesWithStudents();

            Universities = _universities;
            OnPropertyChanged("Universities");
        }

        #endregion
    }
}
