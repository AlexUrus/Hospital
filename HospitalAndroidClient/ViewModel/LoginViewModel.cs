using HospitalAndroidClient.View;
using HospitalLib;
using HospitalLib.Model;
using HospitalLib.Record;
using HospitalLib.Tcp;
using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
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
        public ReactiveCommand<Unit, Unit> LoginCommand
        {
            get => _sendMessageCommand;
            set => this.RaiseAndSetIfChanged(ref _sendMessageCommand, value);
        }
        private ReactiveCommand<Unit, Unit> _registrationUserCommand;
        public ReactiveCommand<Unit, Unit> RegistrationUserCommand
        {
            get => _registrationUserCommand;
            set => this.RaiseAndSetIfChanged(ref _registrationUserCommand, value);
        }

        public LoginViewModel()
        {
            RegistrationUserCommand = ReactiveCommand.CreateFromTask(RegistrationUser);
            LoginCommand = ReactiveCommand.Create(Login);
            TcpClient tcpClient = new TcpClient("192.168.0.106", 7000);
            _tcpClient = new HospitalTcpClient(tcpClient, 60 * 1000);
        }

        private async Task RegistrationUser()
        {
            await SendRegistrationRequest();
            /*
            Patient patient = _dataRepository.GetPatient(Username);
            if (patient == null)
            {
                SendRegistrationRequest();
                Application.Current.MainPage = new NavigationPage(new ChoiceDoctorPage());
            }
            else
            {
                ShowMessage("Ошибка", "Пользователь уже существует.");
            }*/
        }

        private async Task SendRegistrationRequest()
        {
            await _tcpClient.SendAsync(new PatientRecord(new Patient() { Username = Username, Password = Password }));
        }

        private void Login()
        {/*
            Patient patient = _dataRepository.GetPatient(Username);
            if (patient == null)
            {
                ShowMessage("Ошибка", "Логин неверен.");
            }
            else if (patient.Password != Password)
            {
                ShowMessage("Ошибка", "Пароль неверен.");
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(new ChoiceDoctorPage());
            }*/
        }

        private async void ShowMessage(string title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }
    }
}
