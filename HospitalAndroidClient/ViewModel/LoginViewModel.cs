using HospitalAndroidClient.View;
using HospitalLib;
using HospitalLib.Model;
using HospitalLib.Record;
using HospitalLib.Tcp;
using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HospitalAndroidClient.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        public string Username 
        {
            get { return _username; }
            set
            {
                this.RaiseAndSetIfChanged(ref _username, value);
            } 
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                this.RaiseAndSetIfChanged(ref _password, value);
            }
        }
        private HospitalTcpClient _tcpClient;


        private ReactiveCommand<Unit, Unit> _sendMessageCommand;
        public ReactiveCommand<Unit, Unit> SendMessageCommand
        {
            get => _sendMessageCommand;
            set => this.RaiseAndSetIfChanged(ref _sendMessageCommand, value);
        }
        public LoginViewModel()
        {
            SendMessageCommand = ReactiveCommand.CreateFromTask(SendMessage);
            TcpClient tcpClient = new TcpClient("192.168.0.106", 7000 );
            _tcpClient = new HospitalTcpClient(tcpClient, 6000 * 1000);
        }

        private async Task SendMessage()
        {
            await _tcpClient.SendAsync(new PatientRecord(new Patient() { Username = _username, Password = _password } ));
            Application.Current.MainPage = new NavigationPage(new ChoiceDoctorPage());
        }
    }
}
