using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace OPEN_FNB_Android.Client
{
    public class Client
    {
        void connect()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            TcpClient tcpClient = new TcpClient();
            //tcpClient.Connect("192.168.1.10", 5000);
            socket.Connect("192.168.1.11", 5000);

            if (tcpClient.Connected || socket.Connected)
            {
                //MessageBox.Show("Connected");
            }
            else
            {
                //MessageBox.Show("ko ther Connected");
            }

            var buffer = new byte[1000];
            while (tcpClient.Connected)
            {

                socket.Receive(buffer);
                var packet = Packet.fromBytes(buffer);
                //MessageBox.Show(JsonConvert.DeserializeObject<User>(packet.mainObject.ToString()).name);

            }


        }
    }
}
