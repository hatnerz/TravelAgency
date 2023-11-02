using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.model;

namespace TravelAgency.viewmodel
{
    public class ServicesListViewModel
    {
        public List<Service> services;
        public ServicesListViewModel(List<Service> services)
        {
            this.services = services;
        }
        public string StringView { get
            {
                string result = "";
                foreach (Service service in services)
                {
                    result += service.Name;
                    result += "; ";
                }
                return result.Substring(0, result.Length - 2);
            }
        }
    }
}
