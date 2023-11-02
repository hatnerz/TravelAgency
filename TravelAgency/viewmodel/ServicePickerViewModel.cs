using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.model;

namespace TravelAgency.viewmodel
{
    public class ServicePickerViewModel
    {
        public class ServiceWithStatus
        {
            public Service service { get; set; }
            public bool status { get; set; }
            public ServiceWithStatus(Service service, bool status)
            {
                this.service = service;
                this.status = status;
            }
        }
        public ObservableCollection<ServiceWithStatus> servicesWithStatus;
        public ServicePickerViewModel(List<Service> services)
        {
            servicesWithStatus = new ObservableCollection<ServiceWithStatus>();
            foreach (Service service in services)
            {
                servicesWithStatus.Add(new ServiceWithStatus(service, false));
            }
        }
    }
}
