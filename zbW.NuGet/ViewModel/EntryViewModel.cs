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
                if (LoadCommand != null)
                {
                    ((RelayCommand)AddCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public String connection { get; set; }

       
        public String Server
        {
            get { return _server; }
            set
            {
                _server = value;
                NotifyPropertyChanged("Server");
                if (LoadCommand != null)
                {
                    ((RelayCommand) LoadCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public String Database
        {
            get { return _db; }
            set
            {
                _db = value;
                NotifyPropertyChanged("Database");
                if (LoadCommand != null)
                {
                    ((RelayCommand) LoadCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public String User
        {
            get { return _uid; }
            set
            {
                _uid = value;
                NotifyPropertyChanged("User");
                if (LoadCommand != null)
                {
                    ((RelayCommand)LoadCommand).RaiseCanExecuteChanged();
                }

            }
        }
        public String Password
        {
            get { return _pwd; }
            set
            {
                _pwd = value;
                NotifyPropertyChanged("Password");
                if (LoadCommand != null)
                {
                    ((RelayCommand)LoadCommand).RaiseCanExecuteChanged();
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
            //CloseCommand = new RelayCommand<ICloseable>(this.CloseExecute);

        }

        public String GenerateConnentionString(String server, String db, String uid, String pwd)
        {
            return "Server = " + Server + "; Database = " + Database + "; Uid = " + User + ";Pwd = " + Password + ";";
        }

        private void InitCred()
        {
            Server = "localhost";
            Database = "semesterprojekt";
            User = "root";
            Password = "b91590e1";
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
        //public ICommand CloseCommand { get; internal set; }



        private bool CanExecuteLoadCommand()
        {
            if(Server != "" && Database != "" && User != "" && Password != "")
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

        private bool CanExecuteCloseCommand()
        {
            return true;
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
                foreach (Entry entry in duplicates)
                {
                    i.Add(entry.id);
                }
                connection = GenerateConnentionString(Server, Database, User, Password);
                repo.ConfirmEntry(i, connection);
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
