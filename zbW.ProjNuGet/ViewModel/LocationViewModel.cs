using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Prism.Commands;
using zbW.ProjNuGet.Model;
using zbW.ProjNuGet.Repository;

namespace zbW.ProjNuGet.ViewModel
{
    class LocationViewModel : MainViewModel
    {
        private readonly LocationRepoMySql locRepo;
        private Dictionary<string, Object> locDictionary = new Dictionary<string, object>();
        private ObservableCollection<Location> _entrys;
        private ObservableCollection<Location> _hiraEntrys;
        private Location _selectedLocationEntry;
        private string _connection;
        public DelegateCommand DeleteCommand { get; internal set; }
        public DelegateCommand LoadCommand { get; internal set; }
        public DelegateCommand AddCommand { get; internal set; }
       
        public ObservableCollection<Location> Entrys
        {
            get { return _entrys; }
            set { _entrys = value; }
        }

        public ObservableCollection<Location> HiraEntrys
        {
            get => _hiraEntrys;
            set => _hiraEntrys = value;
        }

        public Location SelectedEntry
        {
            get => _selectedLocationEntry;
            set => _selectedLocationEntry = value;
        }

        public LocationViewModel()
        {
            locRepo = new LocationRepoMySql(GenerateConnentionString(Server, Database, User, Password));
            InitEntrys();
            InitDictionarys();
            
            DeleteCommand = new DelegateCommand(DeleteExecute,CanDeleteExecute);
            AddCommand = new DelegateCommand(AddExecute);
            LoadCommand = new DelegateCommand(LoadExecute);
            LoadExecute();
        }

        private bool CanDeleteExecute() => true;

        public void ConnectExecute()
        {
            _connection = GenerateConnentionString(Server, Database, User, Password);
            locRepo.ConnectionString = _connection;
            LoadExecute();
        }

        private void InitDictionarys()
        {
            locDictionary.Add("@name1", "Hauptsitz");
            locDictionary.Add("@name2", "Filiale Gossau");
            locDictionary.Add("@pod1", 1);
            locDictionary.Add("@pod2", 2);
        }

        private void InitEntrys()
        {
            var e = new List<Location>();
            e.Add(new Location());
            Entrys = new ObservableCollection<Location>(e);
            HiraEntrys = new ObservableCollection<Location>(e);
            Entrys.Clear();
            HiraEntrys.Clear();
        }
        
        internal void DeleteExecute()
        {
            if (SelectedEntry != null)
            {
                if (SelectedEntry.Name != null && SelectedEntry.Pod_ID != 0)
                {
                    try
                    {
                        locRepo.Delete(SelectedEntry);
                        LoadExecute();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                    
                }
            }
        }

        public void AddExecute()
        {
            if (SelectedEntry != null)
            {
                if (SelectedEntry.Name != null && SelectedEntry.Pod_ID != 0)
                {
                    bool exists = false;
                    var tmp = locRepo.GetAll();
                    foreach(var entity in tmp)
                    {
                        if (SelectedEntry.Id == entity.Id) exists = true; 
                    }
                    try
                    {
                        if (exists)
                        {
                            locRepo.Update(SelectedEntry);
                        }
                        else
                        {
                            locRepo.Add(SelectedEntry);
                        }                        
                        LoadExecute();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
        }

        internal void LoadExecute()
        {
            var entity = new Location();
            List<Location> entries = new List<Location>();
            List<Location> hiraEntries = new List<Location>();
            try
            {
                entries = locRepo.GetAll().ToList<Location>();
                hiraEntries = locRepo.GetAllHirachical(0,entries).ToList<Location>();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            HiraEntrys.Clear();
            Entrys.Clear();
            foreach (Location hiraEntry in hiraEntries)
            {
                HiraEntrys.Add(hiraEntry);
            }
            foreach (Location entry in entries)
            {
                this.Entrys.Add(entry);
            }
        }

    }
}
