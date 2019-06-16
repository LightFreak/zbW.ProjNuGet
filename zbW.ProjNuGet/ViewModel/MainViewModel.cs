
using System;
using System.ComponentModel;
using Prism.Commands;
using Prism.Mvvm;
using zbW.ProjNuGet.Properties;
using zbW.ProjNuGet.Views;
using UserControl = System.Windows.Controls.UserControl;

namespace zbW.ProjNuGet.ViewModel
{
    class MainViewModel : BindableBase, INotifyPropertyChanged
    {
        private UserControl _content;
        private string _server;
        private string _db;
        private string _uid;
        private string _pwd;

        public string Server
        {
            get
            {
#if DEBUG
                if (string.IsNullOrEmpty(_server))
                {
                    return Settings.Default.DefaultServer;
                }
#endif
                return _server;
            }
            set
            {
                _server = value;
                NotifyPropertyChanged("Server");
                
            }
        }
        public string Database
        {
            get
            {
#if DEBUG
                if (string.IsNullOrEmpty(_db))
                {
                    return Settings.Default.DefaultDB;
                }
#endif
                return _db;
            }
            set
            {
                _db = value;
                NotifyPropertyChanged("Database");
                
            }
        }
        public string User
        {
            get
            {
#if DEBUG
                if (string.IsNullOrEmpty(_uid))
                {
                    return Settings.Default.DefaultUser;
                }
#endif
                return _uid;
            }
            set
            {
                _uid = value;
                NotifyPropertyChanged("User");
                

            }
        }
        public string Password
        {
            get
            {
#if DEBUG
                if (string.IsNullOrEmpty(_pwd))
                {
                    return Settings.Default.DefaultPW;
                }
#endif
                return _pwd;
            }
            set
            {
                _pwd = value;
                NotifyPropertyChanged("Password");
               
            }
        }

        public UserControl Content
        {
            get
            {
                return _content;
            }
            set
            {
                SetProperty(ref _content, value);
            }
        }

        public MainViewModel()
        {
            LoadLogEntryView = new DelegateCommand(OnLoadEntryView);
        }
        public DelegateCommand LoadLogEntryView { get; internal set; }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnLoadEntryView()
        {
            ExecuteEntryView();
        }

        private void ExecuteEntryView()
        {
            Content = new Views.UserControl();
        }
    }
}
