using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using Prism.Commands;
using zbW.ProjNuGet.Model;
using zbW.ProjNuGet.Service;

[assembly: InternalsVisibleTo("zbW.ProjNuGetTest")]
namespace zbW.ProjNuGet.ViewModel
{
    internal class CustomerViewModel : MainViewModel
    {
        public string CustomerId
        {
            get { return _customer.Customer_Id;}
            set
            {
                if (Check_Customer_Id(value))
                {
                    _customer.Customer_Id = value;
                    RaisePropertyChanged("CustomerId");
                    
                }
                else
                {
                    MessageBox.Show("Kundennummer: Entspricht nicht den Vorgaben !");
                    
                   
                }
            }
        }

        public string Phone
        {
            get { return _customer.Phone; }
            set
            {
                if (Check_Phone(value, "ch"))
                {
                    _customer.Phone = value;
                    RaisePropertyChanged("Phone");
                }
                else
                {
                    MessageBox.Show("Phone: Entspricht nicht den Vorgaben !");
                }
            }
        }

        public string Website
        {
            get { return _customer.Website; }
            set
            {
                if (Check_Website(value))
                {
                    _customer.Website = value;
                    RaisePropertyChanged("Website");
                }
                else
                {
                    MessageBox.Show("Website: Entspricht nicht den Vorgaben !");
                }
            }
        }

        public string Email
        {
            get { return _customer.EMail; }
            set
            {
                if (Check_Mail(value))
                {
                    _customer.EMail = value;
                    RaisePropertyChanged("Email");
                }
                else
                {
                    MessageBox.Show("E-Mail: Entspricht nicht den Vorgaben !");
                }
            }
        }

        public string CuPassword
        {
            get
            {
                
                if (_customer.Password != null && _customer.Password.Equals(""))
                {
                    return "";
                }
                else if (_customer.Password == null)
                {
                    return "";
                }

                return "*************";
            }
            set
            {
                if (Check_Password(value))
                {
                    _customer.Password = pwHasher.Hash(value);
                    RaisePropertyChanged("CuPassword");
                }
                else
                {
                    MessageBox.Show("Password: Entspricht nicht den Vorgaben !");
                    
                }
            }
        }

        public string Name
        {
            get { return _customer.Name; }
            set
            {
                _customer.Name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string Prename
        {
            get { return _customer.Prename; }
            set
            {
                _customer.Prename = value;
                RaisePropertyChanged("Prename");
            }
        }

        public ObservableCollection<Customer> Entrys
        {
            get { return _loadedCustomer; }
            set { _loadedCustomer = value; }
        }

        public Customer SelectedCustomer
        {
            get { return _customer; }
            set
            {
                _customer = value;
                RaisePropertyChanged("SelectedCustomer");
            }
        }
        
        private readonly CustomerRepoMySQL cusRepo;
        PasswordHasher pwHasher = new PasswordHasher();
        private Customer _customer;
        private ObservableCollection<Customer> _loadedCustomer;
        public DelegateCommand AddCommand { internal set; get; }
        public DelegateCommand DeleteCommand { internal set; get; }
        public DelegateCommand SearchCommand { internal set; get; }
        public DelegateCommand UpddateCommand { internal set; get; }
        public DelegateCommand DetailCommand { internal set; get; }

        


        public CustomerViewModel()
        {
            cusRepo = new CustomerRepoMySQL(GenerateConnentionString(Server, Database, User, Password));
            _customer = new Customer();
            InitEntrys();
            AddCommand = new DelegateCommand(AddExecute);
            //DeleteCommand = new DelegateCommand();
            SearchCommand = new DelegateCommand(SearchExecute);
            //UpddateCommand = new DelegateCommand();
            //DetailCommand = new DelegateCommand();
            LoadExecute();
        }

        internal  void InitEntrys()
        {
            var e = new List<Customer>();
            e.Add(new Customer());
            Entrys = new ObservableCollection<Customer>(e);
            this.Entrys.Clear();
        }

        internal void AddExecute()
        {
            if (Check_Customer_Id(_customer.Customer_Id) && Check_Mail(_customer.EMail) && Check_Password(_customer.Password) &&
                Check_Phone(_customer.Phone,"ch") && Check_Website(_customer.Website))
            {
                try
                {
                    cusRepo.Add(_customer);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        internal void LoadExecute()
        {
            var entity = new Customer();
            List<Customer> entries = new List<Customer>();

            try
            {
                entries = cusRepo.GetAll().ToList<Customer>();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            this.Entrys.Clear();
            foreach (Customer entry in entries)
            {
                this.Entrys.Add(entry);
            }
        }

        internal void SearchExecute()
        {
            if (_customer.Customer_Id != "")
            {
                foreach (Customer customer in Entrys)
                {
                    if (_customer.Customer_Id == customer.Customer_Id)
                    {
                        UpdateSelectedCustomer(customer);
                        return;
                    }
                }
            }

            if (_customer.Name != "" && _customer.Prename != "")
            {
                foreach (Customer customer in Entrys)
                {
                    if (customer.Name.Contains(_customer.Name))
                }
            }
        }

        internal void UpdateSelectedCustomer(Customer value)
        {
            CustomerId = value.Customer_Id;
            Name = value.Name;
            Prename = value.Prename;
            Phone = value.Phone;
            Email = value.EMail;
            Website = value.Website;
            Password = value.Password;
        }

        internal bool Check_Customer_Id(String value)
        {
            if (value == null)
            {
                return false;
            }

            var customerRegex = new Regex(@"CU\d{5}$",RegexOptions.IgnoreCase);
            var customerMatches = customerRegex.Matches(value);

            foreach(Match match in customerMatches)
            {
                if(match.Value != null)
                {
                    return true;
                }
            }
            
            return false;
        }

        internal bool Check_Phone(String phone, String land)
        {
            if (phone == null)
            {
                return false;
            }

            Dictionary<String,String> pattern = new Dictionary<string, string>();
            pattern.Add("ch", @"^((((\+|00)4\d)\s?((\d{2})|\(0\)\d{2})\s?\d{3}(\s?\d{2}){2})|0\d{2}\s?((\/\s?\d{3})|(\d{3}))(\s?\d{2}){2})$");
            pattern.Add("de", @"^\(?(\+|00)\(?49\)?[ ()]?([- ()]?\d[- ()]?){10}$");
            pattern.Add("li", @"^((((\+|00)423)\s?\d{3}(\s?\d{2}){2})|0\d{2}\s?((\/\s?\d{3})|(\d{3}))(\s?\d{2}){2})$");
            
            
            
            var phoneRegex = new Regex(pattern[land]);
            var phoneMatches = phoneRegex.Matches(phone);

            foreach (Match match in phoneMatches)
            {
                if (match.Value != null)
                {
                    return true;
                }
                
            }

            return false;
        }

        internal bool Check_Website(string website)
        {
            if (website == null)
            {
                return false;
            }

            var webRegex = new Regex(@"^(https?:\/\/)?(www\.)?([a-zA-Z0-9]+(-?[a-zA-Z0-9])*\.)+[\w]{2,}(\/\S*)?$",RegexOptions.IgnoreCase);
            var webMatches = webRegex.Matches(website);

            foreach (Match webMatch in webMatches)
            {
                if (webMatch.Value != null)
                {
                    return true;
                }
            }
            return false;
        }

        internal bool Check_Mail(String mail)
        {
            if (mail == null)
            {
                return false;
            }

            var mailRegex = new Regex(@"^([\w\d\-\.]+)@{1}(([\w\d\-]{1,67})|([\w\d\-]+\.[\w\d\-]{1,67}))\.(([a-zA-Z\d]{2,4})(\.[a-zA-Z\d]{2})?)$");
            var mailMatches = mailRegex.Matches(mail);

            foreach (Match mailMatch in mailMatches)
            {
                if (mailMatch.Value != null)
                {
                    return true;
                }
            }

            return false;
        }

        internal bool Check_Password(String password)
        {
            if (password == null)
            {
                return false;
            }

            var pwRegex = new Regex(@"^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8,}$");
            var pwMatches = pwRegex.Matches(password);

            foreach (Match pwMatch in pwMatches)
            {
                if (pwMatch.Value != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
