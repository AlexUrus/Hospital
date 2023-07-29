using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalLib.Event
{
    public class ConnectionChangedEventArgs : EventArgs
    {
        public string IpAddress { get; private set; }
        public bool IsConnected { get; private set; }

        public ConnectionChangedEventArgs(string ipAddress, bool isConnected) 
        {
            IpAddress = ipAddress;
            IsConnected = isConnected;
        }
    }
}
