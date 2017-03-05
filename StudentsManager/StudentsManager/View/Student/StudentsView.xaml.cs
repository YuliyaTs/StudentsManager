using StudentsManager.Services.Interaction;
using StudentsManager.ViewModel.Students;

namespace StudentsManager.View.Student
{
    /// <summary>
    /// Interaction logic for StudentsView.xaml
    /// </summary>
    public partial class StudentsView
    {

        public StudentsView(InteractionService <Model.Student> interactionService)
        {
            InitializeComponent();
            var dataContext = new StudentsViewModel(interactionService, StudentsGridData.SelectedItems);
            DataContext = dataContext;
        }
    }
}
