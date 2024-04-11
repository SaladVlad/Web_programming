using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using vezbe4;

namespace Httpd
{
    class Program
    {
        public static ArrayList books = new ArrayList();

        public static void StartListening()
        {

            IPAddress ipAddress = IPAddress.Loopback;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 5500);

            Socket serverSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                serverSocket.Bind(localEndPoint);
                serverSocket.Listen(10);

                // Start listening for connections.
                while (true)
                {
                    Console.WriteLine("Waiting for requests...");
                    Socket socket = serverSocket.Accept();

                    Task t = Task.Factory.StartNew(() => Run(socket));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        private static void Run(Socket socket)
        {

            NetworkStream stream = new NetworkStream(socket);
            StreamReader sr = new StreamReader(stream);
            StreamWriter sw = new StreamWriter(stream) { NewLine = "\r\n", AutoFlush = true };

            string resource = GetResource(sr);
            if (resource != null)
            {
                if (resource.Equals(""))
                    resource = "index.html";


                if (resource.Contains("add?"))
                {

                    sw.Write("HTTP/1.0 200 OK\r\n\r\n");

                    string[] data = resource.Split('?')[1].Split('&');
                    string name = data[0].Split('=')[1];
                    string author = data[1].Split('=')[1];
                    double price =  Double.Parse(data[2].Split('=')[1]);
                    foreach(Book b in books)
                    {
                        if (b.Name == name)
                        {
                            sw.Write("<h1>This book aleady exists!</h1>");
                            return;
                        }
                    }
                    books.Add(new Book(name, author, price));
                    sw.Write("<h1>Added book: "+ name +"</h1>");
                    GetAllBooks(sw);

                }
                else if (resource.Contains("find?"))
                {
                    sw.Write("HTTP/1.0 200 OK\r\n\r\n");
                    string[] data = resource.Split('?')[1].Split('&');
                    string name = data[0].Split('=')[1];

                    foreach (Book b in books)
                    {
                        if (b.Name.Equals(name))
                        {
                            sw.Write("<h1>Found book: " + b.ToString() + "</h1>");
                            return;
                        }
                    }
                    sw.Write("<h1>This book doesn't exist!</h1>");

                }
                else
                {
                    SendResponse(resource, socket, sw);
                }
            }
            sr.Close();
            sw.Close();
            stream.Close();

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            //return 0;
        }

        private static void GetAllBooks(StreamWriter sw)
        {
            sw.WriteLine("<html>\n<body>\n");
            sw.Write("<h1>List of all books:</h1>");
            sw.WriteLine("<ol>");
            foreach(Book b in books)
            {
                sw.WriteLine("<li>" + b.ToString() + "</li>");
            }
            sw.WriteLine("</ol>");

            sw.WriteLine("\n</body>\n</html>");
        }

        private static string GetResource(StreamReader sr)
        {
            string line = sr.ReadLine();

            if (line == null)
                return null;

            String[] tokens = line.Split(' ');
            string method = tokens[0];

            if (!method.Equals("GET"))
            {
                return null;
            }
            string rsrc = tokens[1].Substring(1);
            return rsrc;
        }

        private static void SendResponse(string resource, Socket socket, StreamWriter sw)
        {
            // ako u resource-u imamo bilo šta što nije slovo ili cifra, možemo da
            // konvertujemo u "normalan" oblik
            //resource = Uri.UnescapeDataString(resource);

            // pripremimo putanju do našeg web root-a
            resource = "../../../" + resource;
            FileInfo fi = new FileInfo(resource);

            string responseText;
            if (!fi.Exists)
            {
                // ako datoteka ne postoji, vratimo kod za gresku
                responseText = "HTTP/1.0 404 File not found\r\n"
                        + "Content-type: text/html; charset=UTF-8\r\n\r\n<b>404 Doesn't exist:"
                        + fi.Name + "</b>";
                sw.Write(responseText);
                Console.WriteLine("Could not find resource: " + fi.Name);
                return;
            }

            // ispisemo zaglavlje HTTP odgovora
            responseText = "HTTP/1.0 200 OK\r\n\r\n";
            sw.Write(responseText);

            // a, zatim datoteku
            socket.SendFile(resource);
        }

        public static int Main(String[] args)
        {
            StartListening();
            return 0;
        }
    }
}
