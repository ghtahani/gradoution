using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
namespace clinet
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        SocketPermission permission = new SocketPermission(NetworkAccess.Accept, TransportType.Tcp, "", SocketPermission.AllPorts);
            //create socket with the match IpEndPoin establish connection remote host to srever
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect("192.168.1.11", 8001);
            
            //create to varibles that client will be send to server
            string temp = "37.83";
            DateTime DT = DateTime.Now;
            int id = 111;

            
            //take message
            byte[] msgtemp = Encoding.Unicode.GetBytes(temp );
            byte[] msgDT = Encoding.Unicode.GetBytes(DT.ToString());
            byte[] msgid = Encoding.Unicode.GetBytes(id.ToString());
            
            //send message
            int bytesSend = socket.Send(msgtemp);
            int bytesendDT= socket.Send(msgDT);
            int bytesendid = socket.Send(msgid);
        
        }
    }
}
