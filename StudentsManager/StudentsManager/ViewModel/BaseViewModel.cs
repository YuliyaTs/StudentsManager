using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using StudentsManager.Annotations;

namespace StudentsManager.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region Constructor

        protected BaseViewModel()
        {
            InitCommands();
        }

        #endregion

        #region Delegates

        public delegate void CanCloseHandler(bool canCloseNewValue);
        public delegate void PropertyChangedHandler();
        public delegate void CloseViewHandler();

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public ICommand AddCommand
        {
            get;
            private set;
        }

        public ICommand EditCommand
        {
            get;
            private set;
        }

        public ICommand DeleteCommand
        {
            get;
            private set;
        }

        public ICommand RefreshCommand
        {
            get;
            private set;
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public ICommand ImportCommand
        {
            get;
            private set;
        }

        public ICommand ExportCommand
        {
            get;
            private set;
        }

        public bool CanClose { get; set; }

        #endregion

        #region Commands

        protected abstract ICommand GetAddCommand();
        protected abstract ICommand GetEditCommand();
        protected abstract ICommand GetDeleteCommand();
        protected abstract ICommand GetRefreshCommand();
        protected abstract ICommand GetSaveCommand();
        protected abstract ICommand GetImportCommand();
        protected abstract ICommand GetExportCommand();

        #endregion

        #region Methods

        private void InitCommands()
        {
            AddCommand = GetAddCommand();
            EditCommand = GetEditCommand();
            DeleteCommand = GetDeleteCommand();
            RefreshCommand = GetRefreshCommand();
            SaveCommand = GetSaveCommand();
            ImportCommand = GetImportCommand();
            ExportCommand = GetExportCommand();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
