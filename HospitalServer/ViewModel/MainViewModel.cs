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
using HospitalLib.DataStorage;

namespace HospitalServer.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<Patient> Patients { get; set; }

        HospitalTcpServer tcpServer;
        DataRepository repository;

        public MainViewModel()
        {
            repository = DataRepository.Instance;
            Patients = new ObservableCollection<Patient>();
            tcpServer = HospitalTcpServer.Instance;
            tcpServer.MessageReceived += OnTcpMessageReceived;
            tcpServer.NotificationSent += OnNotificationSent;
            tcpServer.RunAsync(7000, 500000);

        }

        private void OnNotificationSent(object sender, NotificationEventArgs e)
        {
            // В этом методе можно обработать уведомление, например, обновить пользовательский интерфейс
            // на основе полученного результата операции.
        }

        private async void OnTcpMessageReceived(object sender, ReceivedMessageEventArgs e)
        {
            switch (e.TypeRecord)
            {
                case TypeRecord.ClientLogin:
                    var result = CreatePatient((PatientRecord)e.Record);
                    await SendStatusAsync(e.IdClient, result);
                    break;
                default:
                    break;
            }
        }

        private ResultOperation CreatePatient(PatientRecord record)
        {
            if (record != null && !repository.PatientExists(record.Value.Username))
            {
                var patient = new Patient { Username = record.Value.Username, Password = record.Value.Password };
                repository.SetPatient(patient);
                Patients.Add(patient);
                return new ResultOperation(ResultOperation.E_OK, "");
            }
            else
            {
                return new ResultOperation(ResultOperation.E_INV_PARAM, "");
            }
            
        }

        private async Task SendStatusAsync(string idClient, ResultOperation resultOperation)
        {
            await tcpServer.SendMessageToClientAsync(idClient, resultOperation);
        }

    }
}
