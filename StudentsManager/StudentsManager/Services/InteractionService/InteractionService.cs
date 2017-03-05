using System;
using System.Collections.ObjectModel;
using System.Linq;
using StudentsManager.Helper;
using StudentsManager.Model;
using StudentsManager.Repositories;
using StudentsManager.View.Base;
using StudentsManager.View.NewFeatures;
using StudentsManager.ViewModel;

namespace StudentsManager.Services.Interaction
{
    public class InteractionService<T> where T : class
    {
        #region Fields
        
        private ObservableCollection<ClosableTab> _viewsInTabs;
        private uint _counter;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes new instance of the <see cref="InteractionService &lt;T&gt;"/> class.
        /// </summary>
        /// <param name="tabs"></param>
        public InteractionService(ObservableCollection<ClosableTab> tabs)
        {
            _viewsInTabs = tabs;
            _counter = 1;
        }

        #endregion

        #region Delegates

        public delegate void PropertyChangedHandler();

        #endregion

        #region Events

        public event BaseViewModel.CanCloseHandler CanCloseView;
        public event PropertyChangedHandler CollectionChanged = delegate { };
        public event EventHandler<BaseView> SelectionChanged;
        public event ClosableHeader.ClosingViewHandler ClosingView = delegate { };

        #endregion

        #region Methods
        /// <summary>
        /// Shows view for adding new student.
        /// </summary>
        /// <param name="baseViewModel"></param>
        /// <param name="collectionOfInstances"></param>
        /// <param name="facultiesRepository"></param>
        /// <param name="universitiesRepository"></param>
        public void ShowAddView(BaseViewModel baseViewModel, ObservableCollection<T> collectionOfInstances, IRepository<University> universitiesRepository = null,
                                IRepository<Faculty> facultiesRepository = null)
        {
            //if (collectionOfInstances.First().GetType() == typeof(Student))
            //{
            //    var studentAddView = new StudentEditView(collectionOfInstances as ObservableCollection<Student>, facultiesRepository, universitiesRepository)
            //    {
            //        Header = "New student " + _counter++,
            //        ViewName = "New student"
            //    };
            //    var studentAddViewTab = new ClosableTab(studentAddView, _viewsInTabs)
            //    {
            //        Content = studentAddView
            //    };

            //    ((ClosableHeader) studentAddViewTab.Header).ClosingEditView += () =>
            //    {
            //        ClosingView();
            //    };

            //    _viewsInTabs.Add(studentAddViewTab);

            //    if (SelectionChanged != null)
            //    {
            //        SelectionChanged(this, studentAddView);
            //    }

            //    ((StudentEditViewModel)studentAddView.DataContext).StudentsCollectionChanged += () =>
            //    {

            //        CollectionChanged();
            //    };

            //    ((StudentEditViewModel)studentAddView.DataContext).CanCloseChanged += canCloseNewValue =>
            //    {
            //        if (CanCloseView != null)
            //            CanCloseView(canCloseNewValue);
            //    };
            //}

            //else if (collectionOfInstances.First().GetType() == typeof(University))
            //{
            //    var universityAddView = new UniversityEditView(collectionOfInstances as ObservableCollection<University>)
            //    {
            //        Header = "New university " + _counter++,
            //        ViewName = "New university"
            //    };
            //    var universityAddViewTab = new ClosableTab(universityAddView, _viewsInTabs)
            //    {
            //        Content = universityAddView
            //    };

            //    ((ClosableHeader)universityAddViewTab.Header).ClosingEditView += () =>
            //    {
            //        ClosingView();
            //    };

            //    _viewsInTabs.Add(universityAddViewTab);

            //    if (SelectionChanged != null)
            //    {
            //        SelectionChanged(this, universityAddView);
            //    }

            //    ((UniversitiesEditViewModel)universityAddView.DataContext).UniversitiesCollectionChanged += () =>
            //    {

            //        CollectionChanged();
            //    };

            //    ((UniversitiesEditViewModel)universityAddView.DataContext).CanCloseChanged += canCloseNewValue =>
            //    {
            //        if (CanCloseView != null)
            //            CanCloseView(canCloseNewValue);
            //    };
            //}

            //else if (collectionOfInstances.First().GetType() == typeof(Faculty))
            //{
            //    var facultyAddView = new FacultyEditView(collectionOfInstances as ObservableCollection<Faculty>)
            //    {
            //        Header = "New faculty " + _counter++,
            //        ViewName = "New faculty"
            //    };
            //    var facultyAddViewTab = new ClosableTab(facultyAddView, _viewsInTabs)
            //    {
            //        Content = facultyAddView
            //    };

            //    ((ClosableHeader)facultyAddViewTab.Header).ClosingEditView += () =>
            //    {
            //        ClosingView();
            //    };

            //    _viewsInTabs.Add(facultyAddViewTab);

            //    if (SelectionChanged != null)
            //    {
            //        SelectionChanged(this, facultyAddView);
            //    }

            //    ((FacultyEditViewModel)facultyAddView.DataContext).FacultiesCollectionChanged += () =>
            //    {

            //        CollectionChanged();
            //    };

            //    ((FacultyEditViewModel)facultyAddView.DataContext).CanCloseChanged += canCloseNewValue =>
            //    {
            //        if (CanCloseView != null)
            //            CanCloseView(canCloseNewValue);
            //    };
            //}

        }

        /// <summary>
        /// Shows view for editing selected student.
        /// </summary>
        /// <param name="baseViewModel"></param>
        /// <param name="editedInstance"></param>
        /// <param name="collectionOfInstances"></param>
        /// <param name="facultiesRepository"></param>
        /// <param name="universitiesRepository"></param>
        public void ShowEditView(BaseViewModel baseViewModel, T editedInstance, ObservableCollection<T> collectionOfInstances, IRepository<University> universitiesRepository = null,
                                 IRepository<Faculty> facultiesRepository = null)
        {
            //if (editedInstance.GetType() == typeof(Student))
            //{
            //    var editedStudent = editedInstance as Student;

            //    if (editedStudent == null)
            //        return;

            //    var header = "Editing ";
            //    header += editedStudent.Name;
            //    header += editedStudent.Surname;

            //    var studentEditView = new StudentEditView(editedStudent, collectionOfInstances as ObservableCollection<Student>, 
            //                                              facultiesRepository, universitiesRepository)
            //    {
            //        Header = header,
            //        ViewName = "Editing student"
            //    };
            //    var studentEditViewTab = new ClosableTab(studentEditView, _viewsInTabs)
            //    {
            //        Content = studentEditView
            //    };

            //    ((ClosableHeader)studentEditViewTab.Header).ClosingEditView += () =>
            //    {
            //        ClosingView();
            //    };

            //    _viewsInTabs.Add(studentEditViewTab);

            //    if (SelectionChanged != null)
            //    {
            //        SelectionChanged(this, studentEditView);
            //    }

            //    ((StudentEditViewModel)studentEditView.DataContext).StudentsCollectionChanged += () =>
            //    {
            //        CollectionChanged();
            //    };

            //    ((StudentEditViewModel)studentEditView.DataContext).CanCloseChanged += canCloseNewValue =>
            //    {
            //        if (CanCloseView != null)
            //            CanCloseView(canCloseNewValue);
            //    };
            //}

            //else if (editedInstance.GetType() == typeof(University))
            //{
            //    var editedUniversity = editedInstance as University;

            //    if (editedUniversity == null)
            //        return;

            //    var header = "Editing ";
            //    header += editedUniversity.Name;

            //    var universityEditView = new UniversityEditView(editedUniversity, collectionOfInstances as ObservableCollection<University>)
            //    {
            //        Header = header,
            //        ViewName = "Editing university"
            //    };

            //    var universityEditViewTab = new ClosableTab(universityEditView, _viewsInTabs)
            //    {
            //        Content = universityEditView
            //    };

            //    ((ClosableHeader)universityEditViewTab.Header).ClosingEditView += () =>
            //    {
            //        ClosingView();
            //    };

            //    _viewsInTabs.Add(universityEditViewTab);

            //    if (SelectionChanged != null)
            //    {
            //        SelectionChanged(this, universityEditView);
            //    }

            //    ((UniversitiesEditViewModel)universityEditView.DataContext).UniversitiesCollectionChanged += () =>
            //    {
            //        CollectionChanged();
            //    };

            //    ((UniversitiesEditViewModel)universityEditView.DataContext).CanCloseChanged += canCloseNewValue =>
            //    {
            //        if (CanCloseView != null)
            //            CanCloseView(canCloseNewValue);
            //    };
            //}

            //else if (editedInstance.GetType() == typeof(Faculty))
            //{
            //    var editedFaculty = editedInstance as Faculty;

            //    if (editedFaculty == null)
            //        return;

            //    var header = "Editing ";
            //    header += editedFaculty.Name;

            //    var facultyEditView = new FacultyEditView(editedFaculty, collectionOfInstances as ObservableCollection<Faculty>)
            //    {
            //        Header = header,
            //        ViewName = "Editing faculty"
            //    };

            //    var facultyEditViewTab = new ClosableTab(facultyEditView, _viewsInTabs)
            //    {
            //        Content = facultyEditView
            //    };

            //    ((ClosableHeader)facultyEditViewTab.Header).ClosingEditView += () =>
            //    {
            //        ClosingView();
            //    };

            //    _viewsInTabs.Add(facultyEditViewTab);

            //    if (SelectionChanged != null)
            //    {
            //        SelectionChanged(this, facultyEditView);
            //    }

            //    ((FacultyEditViewModel)facultyEditView.DataContext).FacultiesCollectionChanged += () =>
            //    {
            //        CollectionChanged();
            //    };

            //    ((FacultyEditViewModel)facultyEditView.DataContext).CanCloseChanged += canCloseNewValue =>
            //    {
            //        if (CanCloseView != null)
            //            CanCloseView(canCloseNewValue);
            //    };
            //}
        }

        #endregion

    }

}
