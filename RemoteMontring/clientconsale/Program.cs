using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace clientconsale
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketPermission permission = new SocketPermission(NetworkAccess.Accept, TransportType.Tcp, "", SocketPermission.AllPorts);
            //create socket with the match IpEndPoin establish connection remote host to srever
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect("127.168.1.1", 8001);
            Console.WriteLine("Helo/n/t");

            //create to varibles that client will be send to server
            string temp = "37.83";
            DateTime DT = DateTime.Now;
            int id = 111;
            Console.WriteLine("Temprture is "+temp+"/n Date "+DT+"/N");

            //take message
            byte[] msgtemp = Encoding.Unicode.GetBytes(temp);
            byte[] msgDT = Encoding.Unicode.GetBytes(DT.ToString());
            byte[] msgid = Encoding.Unicode.GetBytes(id.ToString());

            //send message
            int bytesSend = socket.Send(msgtemp);
            int bytesendDT = socket.Send(msgDT);
            int bytesendid = socket.Send(msgid);
        
        }
    }
}
