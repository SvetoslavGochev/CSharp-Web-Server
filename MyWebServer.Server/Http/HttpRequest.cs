
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWebServer.Server.Http
{
    public class HttpRequest
    {

        private const string NewLine = "/r/n";

        public HttrpMethod Method { get; private set; }

        public string Url { get; private set; }

        public HttpHeaderCollection Headers { get; private set; }

        public string Body { get; private set; }


        public static HttpRequest Parse(string request)
        {
            var lines = request.Split(NewLine);

            var startLine = lines.First().Split(" ");

            var method = ParsehttpMethod(startLine[0]);

            var url = startLine[1];

            var headers = ParseHttpHeaders(lines.Skip(1));

            var bodyLines = lines.Skip(headers.Count + 2).ToArray();

            var body = string.Join(NewLine, bodyLines);

            return new HttpRequest
            {
                Method = method,
                Url = url,
                Headers = headers,
                 Body = body
            };


        }

        private static HttrpMethod ParsehttpMethod(string method)
        {
            return method.ToUpper() switch
            {
                "GET" => HttrpMethod.Get,
                "POST" => HttrpMethod.Post,
                "PUT" => HttrpMethod.Put,
                "DELETE" => HttrpMethod.Delete,
                _ => throw new InvalidOperationException($"Method {method} is not implanet")
            };
        }
        private static HttpHeaderCollection ParseHttpHeaders(IEnumerable<string> headerLines)
        {
            var headertCollection = new HttpHeaderCollection();

            foreach (var headerLine in headerLines)
            {
                if (headerLine == String.Empty)
                {
                    break;
                }

                var headrParts = headerLine.Split(":");

                if (headrParts.Length != 2)
                {
                    throw new InvalidOperationException("request sdfds..");
                }

                var header = new HttpHeader
                {
                    Name = headrParts[0],
                    Value = headrParts[1].Trim()
                };

                headertCollection.Add(header);
            }

            return headertCollection;
        }


    }
    //private static string[] GetStarttLine(string request)
    //{

    //}
}
