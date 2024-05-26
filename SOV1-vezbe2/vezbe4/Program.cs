using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace vezbe4
{
    internal class Program
    {

        public static List<Klub> Klubovi = new List<Klub>();

        static void Main(string[] args)
        {
            Klubovi.Add(new Klub("FK ludilo", "Bosna", true, 23));
            Klubovi.Add(new Klub("FK bihac 93", "Boulan", true, 14));

            IPAddress iPAddress = IPAddress.Loopback;
            IPEndPoint localEndPoint = new IPEndPoint(iPAddress, 9500);

            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                serverSocket.Bind(localEndPoint);
                serverSocket.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    Socket socket = serverSocket.Accept();

                    Task task = Task.Factory.StartNew(() => Run(socket));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void Run(Socket socket)
        {
            NetworkStream stream = new NetworkStream(socket);
            StreamReader sr = new StreamReader(stream);
            StreamWriter sw = new StreamWriter(stream) { NewLine = "\r\n", AutoFlush = true };

            string resource = GetResource(sr);

            if (resource == null)
            {
                sr.Close();
                sw.Close();
                stream.Close();

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                return;
            }
            
            if (resource.Equals(""))
            {
                resource = "index.html";
            }

            resource = Uri.UnescapeDataString(resource);
            if (resource.Contains("dodaj?"))
            {
                string[] vars = resource.Split('?')[1].Split('&');
                string naziv = vars[0].Split('=')[1];
                naziv.Replace('+',' ');
                string grad = vars[1].Split('=')[1];
                grad.Replace('+', ' ');
                bool aktivan = vars[2].Split('=')[1].Equals("on");

                Klubovi.Add(new Klub(naziv, grad, aktivan, 0));

                sw.WriteLine("HTTP/1.0 200 OK\n\r\n\r");

                sw.WriteLine("<html><body>");

                sw.WriteLine(PrikaziTabelu());

                sw.WriteLine("<a href=\"http://localhost:9500/\"><h3>Dodaj novi klub</h3></a>");

                sw.WriteLine("<a href=\"http://localhost:9500/vodeci\"><h3>Prikazi klub</h3></a>");

                sw.WriteLine(PrikaziFormuZaUnosBodova());

                sw.WriteLine("</body></html>");

            }
            else if (resource.Contains("unesi?"))
            {
                string[] vars = resource.Split('?')[1].Split('&');
                string naziv = vars[0].Split('=')[1];
                int bodovi = int.Parse(vars[1].Split('=')[1]);
                

                sw.WriteLine("HTTP/1.0 200 OK\n\r\n\r");

                sw.WriteLine("<html><body>");

                bool found = false;
                foreach (Klub k in Klubovi)
                {
                    if (k.Ime.Equals(naziv))
                    {
                        found = true;
                        k.BrojBodova = bodovi;
                    }
                }

                sw.WriteLine(PrikaziTabelu());

                sw.WriteLine("<a href=\"http://localhost:9500/\"><h3>Dodaj novi klub</h3></a>");

                sw.WriteLine("<a href=\"http://localhost:9500/vodeci\"><h3>Prikazi klub</h3></a>");

                sw.WriteLine(PrikaziFormuZaUnosBodova());

                if (!found)
                {
                    sw.WriteLine("<h3>NEPOSTOJECI KLUB!</h3>");
                }

                sw.WriteLine("</body></html>");
            }
            else if (resource.Contains("izmeni?"))
            {
                resource = Uri.UnescapeDataString(resource);
                int index = int.Parse(resource.Split('?')[1].Split('=')[1]);
                Klub k = Klubovi[index];

                // Generate HTML response
                StringBuilder responseBuilder = new StringBuilder();
                responseBuilder.AppendLine("HTTP/1.0 200 OK");
                responseBuilder.AppendLine("Content-Type: text/html; charset=UTF-8");
                responseBuilder.AppendLine();
                responseBuilder.AppendLine("<html><body>");
                responseBuilder.AppendLine("<h1>Izmena kluba:</h1>");
                responseBuilder.AppendLine("<form accept-charset=\"UTF-8\" action=\"http://localhost:9500/sacuvaj\">");
                responseBuilder.AppendLine("<table>");

                // Populate the form fields with the club's information
                responseBuilder.AppendLine("<tr>");
                responseBuilder.AppendLine("<td>Naziv:</td>");
                responseBuilder.AppendLine("<td><input type=\"text\" name=\"naziv\" value=\"" + k.Ime + "\"/></td>");
                responseBuilder.AppendLine("</tr>");

                responseBuilder.AppendLine("<tr>");
                responseBuilder.AppendLine("<td>Grad:</td>");
                responseBuilder.AppendLine("<td><select name=\"grad\">");
                responseBuilder.AppendLine("<option" + (k.Grad == "Novi Sad" ? " selected" : "") + ">Novi Sad</option>");
                responseBuilder.AppendLine("<option" + (k.Grad == "Beograd" ? " selected" : "") + ">Beograd</option>");
                responseBuilder.AppendLine("<option" + (k.Grad == "Nis" ? " selected" : "") + ">Nis</option>");
                responseBuilder.AppendLine("</select></td>");
                responseBuilder.AppendLine("</tr>");

                responseBuilder.AppendLine("<tr>");
                responseBuilder.AppendLine("<td>Aktivan:</td>");
                responseBuilder.AppendLine("<td><input type=\"checkbox\" name=\"aktivan\"" + (k.Aktivan ? " checked" : "") + "/></td>");
                responseBuilder.AppendLine("</tr>");

                responseBuilder.AppendLine("<tr>");
                responseBuilder.AppendLine("<td><input type=\"hidden\" name=\"index\" value=\"" + index + "\"/></td>");
                responseBuilder.AppendLine("<td><input type=\"submit\" value=\"sacuvaj\" /></td>");
                responseBuilder.AppendLine("</tr>");

                responseBuilder.AppendLine("</table>");
                responseBuilder.AppendLine("</form>");
                responseBuilder.AppendLine("</body></html>");

                sw.Write(responseBuilder.ToString());

            }
            else if (resource.Contains("sacuvaj"))
            {
                string[] vars = resource.Split('?')[1].Split('&');
                string naziv = vars[0].Split('=')[1];
                naziv.Replace('+', ' ');
                string grad = vars[1].Split('=')[1];
                grad.Replace('+', ' ');
                bool aktivan = vars[2].Split('=')[1].Equals("on");
                int index = int.Parse(vars[3].Split('=')[1]);
                Klubovi[index].Ime = naziv;
                Klubovi[index].Grad = grad;
                Klubovi[index].Aktivan = aktivan;

                sw.WriteLine("HTTP/1.0 200 OK\n\r\n\r");

                sw.WriteLine("<html><body>");

                sw.WriteLine(PrikaziTabelu());

                sw.WriteLine("<a href=\"http://localhost:9500/\"><h3>Dodaj novi klub</h3></a>");

                sw.WriteLine("<a href=\"http://localhost:9500/vodeci\"><h3>Prikazi klub</h3></a>");

                sw.WriteLine(PrikaziFormuZaUnosBodova());

                sw.WriteLine("</body></html>");
            }

            else if (resource.Contains("vodeci"))
            {

                int max = 0;
                string najjaci = "";
                foreach(Klub k in Klubovi)
                {
                    if (max < k.BrojBodova)
                    {
                        najjaci = k.Ime;
                        max = k.BrojBodova;
                    }  
                }
                sw.WriteLine("HTTP/1.0 200 OK\n\r\n\r");
                sw.WriteLine("<html><body>");
                sw.WriteLine("<h1>Vodeci klub je: " + najjaci + " sa " + max + " bodova.</h1>");
                sw.WriteLine("</body></html>");
            }
            else
            {
                resource = "../../../" + resource;
                FileInfo fi = new FileInfo(resource);

                string response;
                if (!fi.Exists)
                {
                    response = "HTTP/1.0 404 File not found\r\n\r\n<b>404 Did not find the page requested.</b>";
                    sw.Write(response);
                    Console.WriteLine("Could not find the resource: " + resource);
                    sr.Close();
                    sw.Close();
                    stream.Close();

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    return;
                }
                response = "HTTP/1.0 200 OK\r\nContent-type: text/html; charset=UTF-8\r\n\r\n";
                sw.Write(response);
                socket.SendFile(resource);
            }
            sr.Close();
            sw.Close();
            stream.Close();

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        private static StringBuilder PrikaziFormuZaUnosBodova()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<h1 style=\"color:blue\">Upis bodova</h1>");


            stringBuilder.AppendLine("<table><form action=\"unesi\">");

            stringBuilder.AppendLine("<tr>");

            stringBuilder.AppendLine("<td>Klub:</td>");
            stringBuilder.AppendLine("<td><select name=\"klub\">");
            foreach(Klub k in Klubovi)
            {
                stringBuilder.AppendLine("<option>" + k.Ime +"</option>");
            }
            stringBuilder.AppendLine("</select></td>");
            stringBuilder.AppendLine("</tr>");

            stringBuilder.AppendLine("<tr>");
            stringBuilder.AppendLine("<td>Bodovi:</td>");
            stringBuilder.AppendLine("<td><input type=\"number\" name=\"bodovi\"></td>");
            stringBuilder.AppendLine("</tr>");

            stringBuilder.AppendLine("<tr>");
            stringBuilder.AppendLine("<td><input type=\"submit\" value=\"Unesi\"></td>");
            stringBuilder.AppendLine("</tr>");

            stringBuilder.AppendLine("</form></table>");
            return stringBuilder;
        }

        private static StringBuilder PrikaziTabelu()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<h1 style=\"color:blue\">Tabela</h1>");
            stringBuilder.AppendLine("<table>" +
                "<th>#</th>" +
                "<th><b>Klub</b></th>" +
                "<th><b>Bodovi</b></th>" +
                "<th><b>Akcije</b></th>");
            for (int i = 0; i < Klubovi.Count; i++)
            {
                stringBuilder.AppendLine("<tr>");
                stringBuilder.AppendLine("<td>" + (i + 1) + "</td>");
                stringBuilder.AppendLine("<td>" + Klubovi[i].Ime + "</td>");
                stringBuilder.AppendLine("<td>" + Klubovi[i].BrojBodova + "</td>");
                stringBuilder.AppendLine("<td><a href=\"izmeni?id=" + i + "\">Izmena podataka</td>");
                stringBuilder.AppendLine("</tr>");
            }

            stringBuilder.AppendLine("</table>");
            return stringBuilder;
        }


        private static string GetResource(StreamReader sr)
        {
            string incoming = sr.ReadLine();
            if (incoming == null)
                return null;

            string[] tokens = incoming.Split(' ');
            if (!tokens[0].Equals("GET"))
            {
                return null;
            }
            return tokens[1].TrimStart('/');
        }
    }
}
