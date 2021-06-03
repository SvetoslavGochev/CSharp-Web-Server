namespace MyWebServer
{
    using System.Threading.Tasks;
    using MyWebServer.Server;
    using MyWebServer.Server.Http;
    using MyWebServer.Server2._0.Responses;

    public class StartUp
    {
        public static async Task Main()
      
            => await new HttpServer( routes => routes
                .MapGet("/", new TextResponse("Hello from ss3434"))
                .MapGet("/Cats", new TextResponse("<h1>Helloo from cats<h1>", "text/html"))
                .MapGet("/Dogs", new TextResponse("<h1>Helloo from dogs<h1>", "text/html")))
            .Start();

    }
}

