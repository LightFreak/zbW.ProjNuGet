using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zbW.ProjNuGet.ViewModel
{
    interface IViewModelBase<M>
    {
        ObservableCollection<M> Entrys { get; set; }
        M SelectedEntry { get; set; }
        void InitDictionarys();
        void InitEntrys();

    }
}
