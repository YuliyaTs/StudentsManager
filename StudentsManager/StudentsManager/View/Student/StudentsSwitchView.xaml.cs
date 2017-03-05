using System.Windows;
using System.Windows.Controls;
using StudentsManager.Services.Interaction;
using StudentsManager.ViewModel.Students;

namespace StudentsManager.View.Student
{
    /// <summary>
    /// Interaction logic for StudentsSwitchView.xaml
    /// </summary>
    public partial class StudentsSwitchView
    {
        public StudentsSwitchView(InteractionService<Model.Student> interactionService)
        {
            InitializeComponent();
            DataContext = new StudentsSwitchViewModel(interactionService);
        }

        private void StudentsTreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedStudent = e.NewValue as Model.Student;
            if(selectedStudent != null)
                ((StudentsSwitchViewModel)DataContext).SelectedStudent = selectedStudent;
        }
    }
}
