using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ap.Model
{
    public class ClientSingelton
    {
        static IClient iClient;

        private ClientSingelton() { }

        public static IClient GetClient()
        {
            if (iClient == null)
            {
                iClient = new Client();
            }
            return iClient;
        }

        private class Client : IClient
        {
            private TcpClient tcpClient;

            // constructor
            public Client()
            {
                tcpClient = new TcpClient();
            }

            public bool CheckConnection()
            {
                if (tcpClient != null)
                {
                    return tcpClient.Connected;
                }
                return false;

            }

            async Task<bool> IClient.Connect(string ip, int port)
            {
                // connection might fail
                try
                {
                    // async connection task
                    var nonSyncConnection = tcpClient.ConnectAsync(ip, port);
                    // delay task
                    var delayPrompt = Task.Delay(300);
                    // wait until one task finishs
                    var isComplete = await Task.WhenAny(new[] { delayPrompt, nonSyncConnection });
                    tcpClient.ReceiveTimeout = 10000;
                    return isComplete == nonSyncConnection;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Connection Failed: " + e.ToString());
                    return false;
                }
            }

            void IClient.Disconnect()
            {
                if (tcpClient != null)
                {
                    tcpClient.Close();
                    // connect again
                    tcpClient = new TcpClient();
                }
            }

            string IClient.Read()
            {
                try
                {
                    StreamReader reader = new StreamReader(tcpClient.GetStream());
                    return reader.ReadLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Read Failed: " + e.ToString());
                    return "";
                }

            }

            void IClient.Write(string data)
            {
                try
                {
                    data += "\r\n";
                    //Debug.Write(data);
                    byte[] byteData = Encoding.ASCII.GetBytes(data);
                    tcpClient.GetStream().Write(byteData, 0, byteData.Length);
                    tcpClient.GetStream().Flush();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Write Failed: " + e.ToString());
                }
            }
        }
    }
}

