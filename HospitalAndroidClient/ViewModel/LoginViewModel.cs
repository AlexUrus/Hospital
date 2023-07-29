using HospitalLib;
using HospitalLib.Record;
using HospitalLib.Tcp;
using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace HospitalAndroidClient.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        public string UserName 
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
        }

        private async Task SendMessage()
        {
            await Task.Run(() =>
            {
                AbstractRecord record = new ClientLoginRecord()
                {
                    Username = _username,
                    Password = _password
                };
                string json = JsonConvert.SerializeObject(record);
                //_tcpClient.Send(json);
            });
        }
    }
}
