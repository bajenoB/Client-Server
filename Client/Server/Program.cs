
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Desk
{
    class Program
    {
        static int port = 8000;
        static string address = "127.0.0.1";
        static void Main(string[] args)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
            Socket socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Start server...");
            try
            {
                socketClient.Bind(ipPoint);
                socketClient.Listen(10);




                while (true)
                {
                    socketClient.Send(Encoding.Unicode.GetBytes("Welcome on server!"));
                    Socket handler = socketClient.Accept();
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    byte[] data = new byte[256];

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    Console.WriteLine("Message recived: " + builder.ToString());


                    Dictionary<string, int> keywords = new Dictionary<string, int>();
                    builder.ToString().Split(' ', ',', '.', '!', '?', '-').ToList().ForEach((word) => {
                        if (!keywords.ContainsKey(word))
                        {
                            keywords.Add(word, 1);
                        }
                        else
                        {
                            keywords[word]++;
                        }
                    });

                    string message = string.Empty;
                    keywords.ToList().ForEach((keyword) => {
                        message += keyword.Key + " - " + keyword.Value + "\n";
                    });
                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }


            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Server Closed!");
        }
    }
}