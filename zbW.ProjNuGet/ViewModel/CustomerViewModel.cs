using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using zbW.ProjNuGet.Service;

[assembly: InternalsVisibleTo("zbW.ProjNuGetTest")]
namespace zbW.ProjNuGet.ViewModel
{
    internal class CustomerViewModel : MainViewModel
    {
        public string CustomerId
        {
            get { return _customer_id;}
            set
            {
                if (Check_Customer_Id(value))
                {
                    _customer_id = value;
                }
                else
                {
                    MessageBox.Show("Kundennummer: Entspricht nicht den Vorgaben !");
                    _customer_id = value;
                }
            }
        }

        public string Phone
        {
            get { return _phone; }
            set
            {
                if (Check_Phone(value, "ch"))
                {
                    _phone = value;
                }
                else
                {
                    MessageBox.Show("Phone: Entspricht nicht den Vorgaben !");
                }
            }
        }

        public string Website
        {
            get { return _website; }
            set
            {
                if (Check_Website(value))
                {
                    _website = value;
                }
                else
                {
                    MessageBox.Show("Website: Entspricht nicht den Vorgaben !");
                }
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if (Check_Mail(value))
                {
                    _email = value;
                }
                else
                {
                    MessageBox.Show("E-Mail: Entspricht nicht den Vorgaben !");
                }
            }
        }

        public string CuPassword
        {
            get { return _password; }
            set
            {
                if (Check_Password(value))
                {
                    _password = pwHasher.Hash(value);
                }
                else
                {
                    MessageBox.Show("Password: Entspricht nicht den Vorgaben !");
                    
                }
            }
        }

        public string Name { get; set; }
    
        public string Prename { get; set; }

        public string Notification { get; set; }

        private readonly CustomerRepoMySQL cusRepo;
        PasswordHasher pwHasher = new PasswordHasher();
        private string _customer_id;
        private string _name;
        private string _prename;
        private string _phone;
        private string _website;
        private string _email;
        private string _password;
       
        


        public CustomerViewModel()
        {
            cusRepo = new CustomerRepoMySQL(GenerateConnentionString(Server, Database, User, Password));
        }

        public void Add(String Customer_id, String Phone, String Website, String email, String Password)
        {

        }

        internal bool Check_Customer_Id(String value)
        {
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
