using StudentsManager.Services.Interaction;
using StudentsManager.View.Base;
using StudentsManager.ViewModel.Universities;

namespace StudentsManager.View
{
    /// <summary>
    /// Interaction logic for UniversitiesView.xaml
    /// </summary>
    public partial class UniversitiesView : BaseView
    {
        public UniversitiesView(InteractionService<Model.University> universityInteractionService, InteractionService<Model.Faculty> facultyInteractionService)
        {
            InitializeComponent();
            var dataContext = new UniversitiesViewModel(universityInteractionService, facultyInteractionService, UniversitiesGridData.SelectedItems, FacultiesGridData.SelectedItems);
            DataContext = dataContext;

        }

    }
}
