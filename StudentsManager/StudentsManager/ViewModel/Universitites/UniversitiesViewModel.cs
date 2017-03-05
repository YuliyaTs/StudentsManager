using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace StudentsManager.ViewModel.Universities
{
    public class UniversitiesViewModel : BaseViewModel
    {
        #region Fields

        private readonly XmlUniversitiesRepository _universityRepository;
        private readonly XmlFacultiesRepository _facultyRepository;
        private readonly InteractionService<University> _universityInteractionService;
        private readonly InteractionService<Model.Faculty> _facultyInteractionService;
        private ObservableCollection<University> _universities;
        private ObservableCollection<Model.Faculty> _faculties;
        private IList _selectedUniversities;
        private IList _selectedFaculties;
        private University _selectedUniversity;
        private ObservableCollection<Model.Faculty> _facultiesOfSelectedUniversity;
        private IEditingService<University> _universityEditingService;
        private IEditingService<Model.Faculty> _facultyEditingService;

        #endregion

        #region Constructors

        public UniversitiesViewModel()
        {
            _universityRepository = new XmlUniversitiesRepository();
            _facultyRepository = new XmlFacultiesRepository();

            _universities = new ObservableCollection<University>(_universityRepository.GetAll());
            _faculties = new ObservableCollection<Model.Faculty>(_facultyRepository.GetAll());
            _facultiesOfSelectedUniversity = new ObservableCollection<Model.Faculty>();
            _selectedUniversities = new ObservableCollection<University>();
            _selectedUniversity = new University();

            _universityEditingService = new UniversitiesEditingService();

            CanClose = true;
        }

        public UniversitiesViewModel(InteractionService<University> universityInteractionService, InteractionService<Model.Faculty> facultyInteractionService, IList selectedUniversities, IList selectedFaculties)
            : this()
        {
            _universityInteractionService = universityInteractionService;
            _facultyInteractionService = facultyInteractionService;

            _selectedUniversities = selectedUniversities;
            _selectedFaculties = selectedFaculties;

            _universityInteractionService.CanCloseView += canCloseNewValue =>
            {
                CanClose = canCloseNewValue;
                OnPropertyChanged("CanClose");
            };

            _universityInteractionService.CollectionChanged += () =>
            {
                OnPropertyChanged("Universities");
            };

            _facultyInteractionService.CanCloseView += canCloseNewValue =>
            {
                CanClose = canCloseNewValue;
                OnPropertyChanged("CanClose");
            };

            _facultyInteractionService.CollectionChanged += () =>
            {
                OnPropertyChanged("Faculties");
            };
        }

        #endregion

        #region Properties

        public ObservableCollection<University> Universities
        {
            get { return _universities; }
            set
            {
                _universities = value;
                OnPropertyChanged(Universities.ToString());
            }
        }

        public ObservableCollection<Model.Faculty> Faculties
        {
            get { return _faculties; }
            set
            {
                _faculties = value;
                OnPropertyChanged(Faculties.ToString());
            }
        }

        public University SelectedUniversity
        {
            get { return _selectedUniversity; }
            set
            {
                _selectedUniversity = value;

                if (SelectedUniversity != null)
                {
                    FacultiesOfSelectedUniversity = new ObservableCollection<Model.Faculty>(_facultyRepository.GetAll()
                                                                                                        .Where(faculty => faculty.University.Name == SelectedUniversity.Name));
                    OnPropertyChanged("FacultiesOfSelectedUniversity");
                }

                OnPropertyChanged();
            }
        }

        public ObservableCollection<Model.Faculty> FacultiesOfSelectedUniversity
        {
            get { return _facultiesOfSelectedUniversity; }
            set
            {
                _facultiesOfSelectedUniversity = value;
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
            return new DelegateCommand<object>(ExecuteRefreshCommand, CanExecuteRefreshCommand, true); ;
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

        #region ExecuteCommand

        private void ExecuteEditCommand(object obj)
        {
            _universityInteractionService.ShowEditView(this, _selectedUniversity, _universities);
        }

        private bool CanExecuteEditCommand(object arg)
        {
            return SelectedUniversity != null && _selectedUniversities.Count <= 1;
        }

        #endregion

        #region AddCommand

        private void ExecuteAddCommand(object obj)
        {
            if (obj != null)
            {
                var type = ((List) obj).ListItems.FirstListItem.GetType();

                if (type == typeof(Model.Faculty))
                    _universityInteractionService.ShowAddView(this, _universities);
            }
            _universityInteractionService.ShowAddView(this, _universities);
            
        }

        private bool CanExecuteAddCommand(object arg)
        {
            return true;
        }

        #endregion

        #region DeleteCommand

        private void ExecuteDeleteCommand(object obj)
        {
            var listOfSelectedUniversities = new List<University>();

            foreach (var university in _selectedUniversities)
            {
                listOfSelectedUniversities.Add(university as University);
            }

            _universityEditingService.Delete(_universities, listOfSelectedUniversities);

            OnPropertyChanged("Universities");
            CanClose = false;
        }

        private bool CanExecuteDeleteCommand(object arg)
        {
            return SelectedUniversity != null;
        }

        #endregion

        #region RefreshCommand

        private void ExecuteRefreshCommand(object obj)
        {
            _universities = _universityRepository.GetAll() as ObservableCollection<University>;
            OnPropertyChanged("Universities");

            _faculties = _facultyRepository.GetAll() as ObservableCollection<Model.Faculty>;
            OnPropertyChanged("Faculties");
        }

        private bool CanExecuteRefreshCommand(object arg)
        {
            return true;
        }

        #endregion

        #region SaveCommand

        private void ExecuteSaveCommand(object obj)
        {
            if (!_universityRepository.Save(_universities))
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
                _universities = new ObservableCollection<University>(_universityRepository.GetAll(openDialog.FileName));

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
                _universityRepository.Save(_universities, saveDialog.FileName);
        }

        private bool CanExecuteExportCommand(object arg)
        {
            return true;
        }

        #endregion


        #endregion
    }
}
