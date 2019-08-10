using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DuplicateCheckerLib;
using Prism.Commands;
using zbW.ProjNuGet.Repository;

namespace zbW.ProjNuGet.ViewModel
{
    internal class EntryViewModel : MainViewModel
    {
        private readonly LoggingRepoMySql logRepo = new LoggingRepoMySql();
        private ObservableCollection<LogEntry> _entrys;
        private LogEntry _selectedLogEntry;
        private DuplicateChecker _dubChecker;
        private IEnumerable<IEntity> _duplicates;
        private Dictionary<string, Object> logDictionary = new Dictionary<string, object>();
        
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
                if (AddCommand != null)
                {
                    AddCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string connection { get; set; }

        public DuplicateChecker dubChecker
        {
            get { return _dubChecker; }
            set { _dubChecker = value; }
        }
        
        public EntryViewModel()
        {
            InitEntrys();
            InitDictionarys();
            
            dubChecker = new DuplicateChecker();
            LoadCommand = new DelegateCommand(LoadExecute, CanExecuteLoadCommand);
            AddCommand = new DelegateCommand(AddExecute, CanExecuteAddCommand);
            ConfirmCommand = new DelegateCommand(ConfirmExecute, CanExecuteConfirmCommand);
            DuplicateCommand = new DelegateCommand(DuplicateExecute, CanExecuteDuplicateCommand);
            DeleteAllDuplicatesCommand = new DelegateCommand(DeleteAllDuplicatesExecute, CanExecuteDeleteCommand);
            ConnectExecute();
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
        }
        
        public DelegateCommand LoadCommand { get; internal set; }

        public DelegateCommand AddCommand { get; internal set; }

        public DelegateCommand ConfirmCommand { get; internal set; }

        public DelegateCommand DuplicateCommand { get; internal set; }

        public DelegateCommand DeleteAllDuplicatesCommand { get; internal set; }

        private bool CanExecuteLoadCommand()
        {
            if(Server != "" && Database != "" && User != "" && Entrys.Count > 0)
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
            //locRepo.ConnectionString = connection;
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
            
            this.Entrys.Clear();
            foreach (LogEntry entry in entries)
            {
                this.Entrys.Add(entry);
            }
            
            if (DuplicateCommand != null)
            {
                DuplicateCommand.RaiseCanExecuteChanged();
            }
            if (ConfirmCommand != null)
            {
                ConfirmCommand.RaiseCanExecuteChanged();
            }

            if (LoadCommand != null)
            {
                LoadCommand.RaiseCanExecuteChanged();
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
                if (DuplicateCommand != null)
                {
                    DuplicateCommand.RaiseCanExecuteChanged();

                }
                if (DeleteAllDuplicatesCommand != null)
                {
                    DeleteAllDuplicatesCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public void DeleteAllDuplicatesExecute()
        {
            foreach (var dupl in _duplicates)
            {
                logRepo.Delete((LogEntry) dupl);
            }
            if (DeleteAllDuplicatesCommand != null)
            {
                DeleteAllDuplicatesCommand.RaiseCanExecuteChanged();

            }
            LoadExecute();
            DuplicateExecute();
            
        }

    }
}
