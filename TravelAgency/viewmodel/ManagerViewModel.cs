using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.model;

namespace TravelAgency.viewmodel
{
    public class ManagerViewModel : ViewModelBase
    {
        public Manager CurrentManager;
        public string FirstName
        {
            get => CurrentManager.FirstName;
            set
            {
                CurrentManager.FirstName = value;
                OnPropertyChanged("GreetingInfo");
                OnPropertyChanged("GetFullName");
            }
        }
        public string LastName
        {
            get => CurrentManager.LastName;
            set
            {
                CurrentManager.LastName = value;
                OnPropertyChanged("GreetingInfo");
                OnPropertyChanged("GetFullName");
            }
        }
        public string PatronymicName
        {
            get => CurrentManager.PatronymicName;
            set
            {
                CurrentManager.PatronymicName = value;
                OnPropertyChanged("GreetingInfo");
                OnPropertyChanged("GetFullName");
            }
        }
        public string PhoneNumber
        {
            get => CurrentManager.OfficePhone;
            set => CurrentManager.OfficePhone = value;
        }
        public string Login
        {
            get => CurrentManager.login;
            set => CurrentManager.login = value;
        }
        public string Password
        {
            get => CurrentManager.password;
            set => CurrentManager.password = value;
        }
        public bool Admin
        {
            get => CurrentManager.Admin;
            set => CurrentManager.Admin = value;
        }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName + " " + PatronymicName;
            }
        }

        public string GreetingInfo
        {
            get
            {
                string status;
                if (CurrentManager.Admin == true)
                    status = "адміністратор";

                else
                    status = "менеджер";
                return "Вітаємо, " + FullName + "!\nВи авторизувалися як " + status;
            }
        }

        public ManagerViewModel(Manager currentManager)
        {
            CurrentManager = currentManager;
        }
    }
}
