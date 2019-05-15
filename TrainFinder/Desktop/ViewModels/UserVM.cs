using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Desktop.Model;

namespace Desktop.ViewModels
{
    public class UserVM:BaseViewModelMain
    {
        
        private string _userName;
        //private logp _login;

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                IsValidCheck();
                //Command.RaiseCanExecuteChanged();
            }
        }

        public string Password { get; set; }

        public CommandBase Command{get;set;}


        private void DoLogin()
        {
            //if (user.Login(new user() {Uname = "osura", Password = "osura"}))
            {
                new Main().Show();
              
                
            }
        }

        private  void IsValidCheck()
        {
            
            //var returnBool = Validations.NullEmptyStringValidation(UserName);
            //returnBool = Validations.LengthCheck(UserName, 6);
            //IsValid= returnBool;
        }

        protected override bool CheckValid()
        {
            throw new NotImplementedException();
        }

        

        protected override void Reset()
        {
            throw new NotImplementedException();
        }
    }

}
