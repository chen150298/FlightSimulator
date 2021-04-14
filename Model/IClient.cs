using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ap.Model
{
    public interface IClient
    {
        Task<bool> Connect(string ip, int port);
        void Write(string data);
        string Read(); // blocking call
        void Disconnect();
        /*
        *  A function to check the connection to the server
        */
        bool CheckConnection();
    }
}
