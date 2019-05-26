using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using zbW.ProjNuGet.Connection;
using System.Linq;
using DuplicateCheckerLib;
using zbW.ProjNuGet.Properties;

namespace zbW.ProjNuGet.ViewModel
{
    class EntryViewModel : INotifyPropertyChanged
    {
        private readonly IEntryServices repo = new EntryServices();
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Entry> _entrys;
        private String _server;
        private String _db;
        private String _uid;
        private String _pwd;
        private Entry _selectedEntry;
        private DuplicateChecker _dubChecker;
        private Boolean _canExecuteLoad;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public ObservableCollection<Entry> Entrys
        {
            get { return _entrys;}
            set { _entrys = value; }
        }
        public Entry SelectedEntry
        {
            get { return _selectedEntry; }
            set
            {
                _selectedEntry = value;
                NotifyPropertyChanged("SelectedEntry");
                if (AddCommand != null)
                {
                    ((RelayCommand)AddCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public String connection { get; set; }
        private List<int> _duplicateInts = new List<int>();

        public List<int> duplicatesInt
        {
            get { return _duplicateInts; }
            set
            {
                
                _duplicateInts = value;
                NotifyPropertyChanged("DuplicateInts");
                if (DeleteCommand != null)
                {
                    ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
                }

            }
        }


        public String Server
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
                if (LoadCommand != null)
                {
                    ((RelayCommand) LoadCommand).RaiseCanExecuteChanged();
                }
                if (ConnectCommand != null)
                {
                    ((RelayCommand)ConnectCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public String Database
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
                if (LoadCommand != null)
                {
                    ((RelayCommand) LoadCommand).RaiseCanExecuteChanged();
                }
                if (ConnectCommand != null)
                {
                    ((RelayCommand)ConnectCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public String User
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
                if (LoadCommand != null)
                {
                    ((RelayCommand)LoadCommand).RaiseCanExecuteChanged();
                }
                if (ConnectCommand != null)
                {
                    ((RelayCommand)ConnectCommand).RaiseCanExecuteChanged();
                }

            }
        }
        public String Password
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
                if (LoadCommand != null)
                {
                    ((RelayCommand)LoadCommand).RaiseCanExecuteChanged();
                }
                if (ConnectCommand != null)
                {
                    ((RelayCommand)ConnectCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public DuplicateChecker dubChecker
        {
            get { return _dubChecker; }
            set { _dubChecker = value; }
        }
        
        public EntryViewModel()
        {
            InitEntrys();
            InitCred();
            
            dubChecker = new DuplicateChecker();
            LoadCommand = new RelayCommand(LoadExecute, CanExecuteLoadCommand);
            AddCommand = new RelayCommand(AddExecute, CanExecuteAddCommand);
            ConfirmCommand = new RelayCommand(ConfirmExecute, CanExecuteConfirmCommand);
            DuplicateCommand = new RelayCommand(DuplicateExecute, CanExecuteDuplicateCommand);
            DeleteCommand = new RelayCommand(DeleteExecute, CanExecuteDeleteCommand);
            ConnectCommand = new RelayCommand(ConnectExecute, CanExecuteConnectCommand);

        }

        public String GenerateConnentionString(String server, String db, String uid, String pwd)
        {
            return "Server = " + Server + "; Database = " + Database + "; Uid = " + User + ";Pwd = " + Password + ";";
        }

        private void InitCred()
        {
            Server = "";
            Database = "";
            User = "";
            Password = "";
        }
        private void InitEntrys()
        {
            var e = new List<Entry>();
            e.Add(new Entry());
            Entrys = new ObservableCollection<Entry>(e);
            this.Entrys.Clear();
        }

        public ICommand LoadCommand { get; internal set; }
        public ICommand AddCommand { get; internal set; }
        public ICommand ConfirmCommand { get; internal set; }
        public ICommand DuplicateCommand { get; internal set; }
        public ICommand DeleteCommand { get; internal set; }
        public ICommand ConnectCommand { get; internal set; }



        private bool CanExecuteLoadCommand()
        {
            if(Server != "" && Database != "" && User != "" && Entrys.Count > 0)
            {
                return true;
            }
            return false;
        }

        private bool CanExecuteConnectCommand()
        {
            if (Server != "" && Database != "" && User != "")
            {
                return true;
            }
            return false;
        }

        private bool CanExecuteAddCommand()
        {
            if (SelectedEntry != null) return true;
            return false;
        }

        private bool CanExecuteConfirmCommand()
        {
            if (Entrys.Count > 0) return true;
            return false;
        }

        private bool CanExecuteDuplicateCommand()
        {
            if (Entrys.Count > 1) return true;
            return false;
        }

        private bool CanExecuteDeleteCommand()
        {
            if (duplicatesInt.Count > 0) return true;
            return false;
        }

        public void ConnectExecute()
        {
            LoadExecute();
        }

        public void LoadExecute()
        {
            connection = GenerateConnentionString(Server, Database, User, Password);
            var entries = repo.GetEntrys(connection);
           
            this.Entrys.Clear();
            foreach (Entry entry in entries)
            {
                this.Entrys.Add(entry);
            }
            NotifyPropertyChanged("Entrys");
            if (DuplicateCommand != null)
            {
                ((RelayCommand)DuplicateCommand).RaiseCanExecuteChanged();
            }
            if (ConfirmCommand != null)
            {
                ((RelayCommand)ConfirmCommand).RaiseCanExecuteChanged();
            }
            if (LoadCommand != null)
            {
                ((RelayCommand)LoadCommand).RaiseCanExecuteChanged();
            }

        }

        public void AddExecute()
        {
            if(SelectedEntry != null)
            {
                if(SelectedEntry.timestamp != null && SelectedEntry.severity != 0)
                {
                    connection = GenerateConnentionString(Server, Database, User, Password);
                    repo.AddEntry(SelectedEntry, connection);
                    LoadExecute();
                }
            }
            
        }

        public void ConfirmExecute()
        {
            var toConfirm = new List<int>();

            foreach (Entry entry in Entrys)
            {
                if (entry.confirm == true)
                {
                    toConfirm.Add(entry.id);
                }
            }

            if (toConfirm.Count != 0)
            {
                connection = GenerateConnentionString(Server, Database, User, Password);
                repo.ConfirmEntry(toConfirm,connection);
            }

            LoadExecute();
        }

        public void DuplicateExecute()
        {
            var duplicates = dubChecker.FindDuplicates(Entrys);

            if (duplicates.Count() != 0)
            {
                var i = new List<int>();
                var l = Entrys.ToList();
                this.Entrys.Clear();
                foreach (Entry e in l)
                {
                    foreach (var dup in duplicates)
                    {
                        if (e.Equals(dup))
                        {
                            e.duplicate = true;
                            e.confirm = true;
                            i.Add(e.id);
                        }
                    }
                    
                }
                
                this.Entrys.Clear();
                foreach (Entry entry in l)
                {
                    this.Entrys.Add(entry);
                }

                NotifyPropertyChanged("Entrys");
                if (DuplicateCommand != null)
                {
                    ((RelayCommand)DuplicateCommand).RaiseCanExecuteChanged();

                }

                duplicatesInt = i;
            }
        }

        public void DeleteExecute()
        {
            connection = GenerateConnentionString(Server, Database, User, Password);
            repo.ConfirmEntry(duplicatesInt, connection);
            duplicatesInt.Clear();

            NotifyPropertyChanged("DuplicateInts");
            
            if (DeleteCommand != null)
            {
                ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();

            }
            LoadExecute();
            
        }

        /*
        public void CloseExecute(ICloseable window)
        {
            if (window != null)
            {
                window.Close();
            }
        }*/
    }

}
