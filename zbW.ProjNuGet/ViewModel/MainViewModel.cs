
using System;
using Prism.Commands;
using Prism.Mvvm;
using zbW.ProjNuGet.Properties;
using UserControl = System.Windows.Controls.UserControl;

namespace zbW.ProjNuGet.ViewModel
{
    class MainViewModel : BindableBase
    {
        private UserControl _content;
        private string _server;
        private string _db;
        private string _uid;
        private string _pwd;
        
        public virtual string Server
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
               
            }
        }
        public virtual string Database
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
               
            }
        }
        public virtual string User
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
               
            }
        }
        public virtual string Password
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
            InitCred();
            LoadLogEntryView = new DelegateCommand(OnLoadEntryView);
            LoadLocationEntryView = new DelegateCommand(OnLocationEntryView);
        }

        private void InitCred()
        {
            Server = "";
            Database = "";
            User = "";
            Password = "";
        }

        public DelegateCommand LoadLogEntryView { get; internal set; }

        public DelegateCommand LoadLocationEntryView { get; internal set; }
        
        private void OnLoadEntryView()
        {
            Content = new Views.LoggingView();
        }

        private void OnLocationEntryView()
        {
            Content = new Views.LocationView();
        }
        
        public string GenerateConnentionString(String server, String db, String uid, String pwd)
        {
            return "Server = " + Server + "; Database = " + Database + "; Uid = " + User + ";Pwd = " + Password + ";";
        }

    }
}
