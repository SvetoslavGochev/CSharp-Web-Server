namespace MyWebServer.Server
{
    using MyWebServer.Server.Http;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    public class HttpServer
    {
        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly TcpListener listener;

        public HttpServer(string ipAddress, int port)
        {
            this.ipAddress = IPAddress.Parse(ipAddress);
            this.port = port;

             listener = new TcpListener(this.ipAddress, port);
        }

        public async Task Start()
        {
            
            this.listener.Start();

            Console.WriteLine($"Server started on port {port}...");
            Console.WriteLine("Listening for request..");


            while (true)
            {
                var connection = await this.listener.AcceptTcpClientAsync();
                //vrazkata s brauzara

                var networkStream = connection.GetStream();//dannite po mrejata

                var requestText = await this.ReadRequest(networkStream);

                Console.WriteLine(requestText);

                var request = HttpRequest.Parse(requestText);

                await WriteResponse(networkStream);
               
                connection.Close();
            }
        }


        private async Task<string> ReadRequest(NetworkStream networkStream)
        {
            var bufferLength = 1024;
            var buffer = new byte[bufferLength];

            var sb = new StringBuilder();

            while (networkStream.DataAvailable)//dokato ima za 4etene ot streama
            {
                var bytesRead = await networkStream.ReadAsync(buffer, 0, bufferLength);
                //shte pro4ete kolkot sa mu pratili  primer 502 byte
                sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }

            
            return sb.ToString();
        }


        private async Task WriteResponse(NetworkStream networkStream)
        {
            var content = @"
<html>
     <head>
          <link rel =""icon"" href=""data:,"">
     <body>
     </body>
      Hello from my server!

     </head>

<html>";
            var contententLength = Encoding.UTF8.GetByteCount(content);

            var response = $@"HTTP/1.1 200 OK
Server: My Web Server
Date: {DateTime.UtcNow:r}
Content-Length: {contententLength}
Content-Type: text/html; charset=UTF-8 

{content}";

            var responseBytes = Encoding.UTF8.GetBytes(response);//ot bytove kym string

            await networkStream.WriteAsync(responseBytes);
        }

    }
}
