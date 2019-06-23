using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zbW.ProjNuGet.ViewModel
{
    public abstract class ViewModelBase<M>:IViewModelBase<M>
    {
        
        public abstract ObservableCollection<M> Entrys { get; set; }
        public abstract M SelectedEntry { get; set; }
        public abstract void InitDictionarys();
        public abstract void InitEntrys();

    }
}
