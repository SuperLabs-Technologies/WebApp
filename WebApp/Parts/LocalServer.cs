using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WebApp
{
    internal class LocalServer
    {
        HttpListener httpListener = new HttpListener();
        public int Port = 0;
        public string Url = "";

        Dictionary<string, string> ContentTypes = new Dictionary<string, string>()
        {
            { "js", "application/javascript" },
            { "svg", "image/svg+xml" }
        };

        public void Start()
        {
            RuntimeLog.WriteLog($"Finding available port");
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] listeners = properties.GetActiveTcpListeners();

            int[] openPorts = listeners.Select(item => item.Port).ToArray<int>();
            int[] allPorts = Enumerable.Range(4000, 65535).ToArray();

            allPorts = allPorts.Where(t => !openPorts.Contains(t)).ToArray();
            if(this.Port == 0)
            {
                this.Port = allPorts.First();
            }
            this.Url = $"http://localhost:{this.Port}/";

            RuntimeLog.WriteLog($"Starting server at port {this.Port}");

            httpListener.Prefixes.Add(this.Url);
            httpListener.Start();
            Thread _responseThread = new Thread(ServerThread);
            _responseThread.Start();

            RuntimeLog.WriteLog($"Server started!");
            RuntimeLog.WriteLog($"Server url: {this.Url}");
        }

        void ServerThread()
        {
            for(; ; )
            {
                HttpListenerContext context = httpListener.GetContext();
                string targetFile = context.Request.RawUrl.Remove(0, 1);

                if(targetFile != "")
                {
                    byte[] _responseArray = Encoding.UTF8.GetBytes("404 File not found");

                    if (File.Exists(targetFile))
                    {
                        _responseArray = File.ReadAllBytes(context.Request.RawUrl.Remove(0, 1));
                        RuntimeLog.WriteLog($"Serving file {context.Request.RawUrl.Remove(0, 1)}");
                    }
                    else
                    {
                        RuntimeLog.WriteLog($"Could not get file at '{targetFile}'");
                        RuntimeLog.WriteLog($"File does not exist at '{targetFile}'");
                    }

                    context.Response.Headers.Add("Vary", "Origin");
                    context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                    context.Response.Headers.Add("Accept-Ranges", "bytes");
                    context.Response.Headers.Add("Cache-Control", "public, max-age=0");
                    context.Response.ContentLength64 = _responseArray.Length;

                    string fileType = Path.GetExtension(context.Request.RawUrl.Remove(0, 1)).Remove(0, 1);
                    if (ContentTypes.ContainsKey(fileType))
                    {
                        string contentType = ContentTypes[fileType];
                        if(contentType != "")
                        {
                            context.Response.ContentType = $"{ContentTypes[fileType]}";
                        }
                    }
                    else
                    {
                        context.Response.ContentType = $"text/{fileType}; charset=utf-8";
                    }
                    context.Response.OutputStream.Write(_responseArray, 0, _responseArray.Length);
                    context.Response.KeepAlive = false;
                    context.Response.Close();
                }
            }
        }
    }
}
