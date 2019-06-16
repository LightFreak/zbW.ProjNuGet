using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Linq;
using System.Windows;
using DuplicateCheckerLib;
using zbW.ProjNuGet.Model;
using zbW.ProjNuGet.Properties;
using zbW.ProjNuGet.Repository;

namespace zbW.ProjNuGet.ViewModel
{
    public class EntryViewModel : INotifyPropertyChanged
    {
        private readonly LocationRepoMySql locRepo = new LocationRepoMySql();
        private readonly LoggingRepoMySql logRepo = new LoggingRepoMySql();
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<LogEntry> _entrys;
        private string _server;
        private string _db;
        private string _uid;
        private string _pwd;
        private LogEntry _selectedLogEntry;
        private DuplicateChecker _dubChecker;
        private IEnumerable<IEntity> _duplicates;

        private Dictionary<string, Object> logDictionary = new Dictionary<string, object>();
        private Dictionary<string, Object> locDictionary = new Dictionary<string, object>();

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public ObservableCollection<LogEntry> Entrys
        {
            get { return _entrys;}
            set { _entrys = value; }
        }
        public LogEntry SelectedLogEntry
        {
            get { return _selectedLogEntry; }
            set
            {
                _selectedLogEntry = value;
                NotifyPropertyChanged("SelectedLogEntry");
                if (AddCommand != null)
                {
                    ((RelayCommand)AddCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string connection { get; set; }
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
            InitDictionarys();
            
            dubChecker = new DuplicateChecker();
            LoadCommand = new RelayCommand(LoadExecute, CanExecuteLoadCommand);
            AddCommand = new RelayCommand(AddExecute, CanExecuteAddCommand);
            ConfirmCommand = new RelayCommand(ConfirmExecute, CanExecuteConfirmCommand);
            DuplicateCommand = new RelayCommand(DuplicateExecute, CanExecuteDuplicateCommand);
            DeleteAllDuplicatesCommand = new RelayCommand(DeleteAllDuplicatesExecute, CanExecuteDeleteCommand);
            ConnectCommand = new RelayCommand(ConnectExecute, CanExecuteConnectCommand);

        }

        public string GenerateConnentionString(String server, String db, String uid, String pwd)
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
            var e = new List<LogEntry>();
            e.Add(new LogEntry());
            Entrys = new ObservableCollection<LogEntry>(e);
            this.Entrys.Clear();
        }

        private void InitDictionarys()
        {
            logDictionary.Add("@pod1", "Nöldi Gmbh");
            logDictionary.Add("@pod2", "Swiss Consulting");
            logDictionary.Add("@hostname", "Ghost002");
            logDictionary.Add("@hostname2", "DVC%");
            logDictionary.Add("@message", "%test%");
            logDictionary.Add("@message2", "%CODE%");
            locDictionary.Add("@name1","Hauptsitz");
            locDictionary.Add("@name2", "Filiale Gossau");
            locDictionary.Add("@pod1", 1);
            locDictionary.Add("@pod2", 2);
        }

        public ICommand LoadCommand { get; internal set; }
        public ICommand AddCommand { get; internal set; }
        public ICommand ConfirmCommand { get; internal set; }
        public ICommand DuplicateCommand { get; internal set; }
        public ICommand DeleteAllDuplicatesCommand { get; internal set; }
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
            if (SelectedLogEntry != null) return true;
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
            if (_duplicates != null && _duplicates.Count() > 0) return true;
            return false;
        }

        public void ConnectExecute()
        {
            connection = GenerateConnentionString(Server, Database, User, Password);
            locRepo.ConnectionString = connection;
            logRepo.ConnectionString = connection;
            LoadExecute();
        }

        public void LoadExecute()
        {
            var entity = new LogEntry();
            List<LogEntry> entries = new List<LogEntry>();
            try
            {
                entries = logRepo.GetAll();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            

            //var test = locRepo.GetAll("name = @name1", locDictionary);
            
            this.Entrys.Clear();
            foreach (LogEntry entry in entries)
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
            if(SelectedLogEntry != null)
            {
                if(SelectedLogEntry.timestamp != null && SelectedLogEntry.severity != 0)
                {
                    logRepo.Add(SelectedLogEntry);
                    LoadExecute();
                }
            }
            
        }

        public void ConfirmExecute()
        {
            var toConfirm = new List<LogEntry>();

            foreach (LogEntry entry in Entrys)
            {
                if (entry.confirm == true)
                {
                    toConfirm.Add(entry);
                }
            }

            if (toConfirm.Count != 0)
            {
                foreach (var entry in toConfirm)
                {
                   logRepo.Delete(entry); 
                }
                
            }

            LoadExecute();
            DuplicateExecute();
        }

        public void DuplicateExecute()
        {
            _duplicates = dubChecker.FindDuplicates(Entrys);

            if (_duplicates.Count() != 0)
            {
                
                var l = Entrys.ToList();
                this.Entrys.Clear();
                foreach (LogEntry e in l)
                {
                    foreach (var dup in _duplicates)
                    {
                        if (e.Equals(dup))
                        {
                            e.duplicate = true;
                            e.confirm = true;
                        }
                    }
                    
                }
                
                this.Entrys.Clear();
                foreach (LogEntry entry in l)
                {
                    this.Entrys.Add(entry);
                }

                NotifyPropertyChanged("Entrys");
                if (DuplicateCommand != null)
                {
                    ((RelayCommand)DuplicateCommand).RaiseCanExecuteChanged();

                }
                if (DeleteAllDuplicatesCommand != null)
                {
                    ((RelayCommand)DeleteAllDuplicatesCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public void DeleteAllDuplicatesExecute()
        {
            foreach (var dupl in _duplicates)
            {
                logRepo.Delete((LogEntry) dupl);
            }
            NotifyPropertyChanged("DuplicateInts");
            
            if (DeleteAllDuplicatesCommand != null)
            {
                ((RelayCommand)DeleteAllDuplicatesCommand).RaiseCanExecuteChanged();

            }
            LoadExecute();
            DuplicateExecute();
            
        }

    }

}
