using System;
using System.Windows;
using Desktop.Model;

namespace Desktop.ViewModels
{
    public class UserVm:BaseViewModelMain
    {
        private bool flag;
        public string Name { get; set; }
        public string UserName { get; set; }


        public CommandBase CreateCommand { get; set; }
        public CommandBase UpdateCommand { get; set; }
        public CommandBase ResetCommand { get; set; }

        public UserVm()
        {
            CreateCommand = new CommandBase((data)=>AddUser(data),()=>true);
            UpdateCommand = new CommandBase((data)=>UpdateUser(data),()=>true);
            ResetCommand = new CommandBase(ResetView, () => true);
        }

        private void UpdateUser(object data)
        {
            flag = false;
            AddUpdate(data);
        }

        private void ResetView()
        {
            Name = UserName="";
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(UserName));
        }

        private void AddUser(object data)
        {
            flag = true;
            AddUpdate(data);
        }

        private void AddUpdate(object data)
        {
            var havePassword = data as IHavePassword;
            if (havePassword.SecureString.ComparePassword(havePassword.SecureString1, out string password))
            {
                var response = WebConnect.PostData("User/Create", new User { Name = Name, Uname = UserName, Password = password, UserId = flag ? 0 : LogInFor.User.UserId });
                DialogDisplayHelper.DisplayMessageBox(flag?"Add Complete":"Update Complete", "Informative");
            }
            else
            {
                DialogDisplayHelper.DisplayMessageBox("Confirm Password And Password Not Matched", "Informative", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
