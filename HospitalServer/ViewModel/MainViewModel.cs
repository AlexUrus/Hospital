using HospitalLib.Event;
using HospitalLib.Model;
using HospitalLib.Record;
using HospitalLib.Tcp;
using HospitalLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalServer.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<Patient> Patients { get; set; }

        HospitalTcpServer tcpServer;

        public MainViewModel()
        {
            Patients = new ObservableCollection<Patient>();
            tcpServer = HospitalTcpServer.Instance;
            tcpServer.MessageReceived += OnTcpMessageReceived;
            tcpServer.RunAsync(7000, 500000);

        }

        private void OnTcpMessageReceived(object sender, ReceivedMessageEventArgs e)
        {
            switch (e.TypeRecord)
            {
                case TypeRecord.ClientLogin:
                    CreatePatient((PatientRecord)e.Record);
                    break;
                default:
                    break;
            }
        }

        private void CreatePatient(PatientRecord record)
        {
            var patient = new Patient { Username = record.Value.Username, Password = record.Value.Password };
            Patients.Add(patient);
        }
    }
}
