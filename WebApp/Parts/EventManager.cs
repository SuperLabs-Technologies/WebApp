using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebApp
{
    internal class EventManager
    {
        public static List<Element> Elements = new List<Element>();
        WebView2 webView { get; set; }

        public EventManager(WebView2 webView2)
        {
            this.webView = webView2;
            this.webView.WebMessageReceived += WebRenderer_WebMessageReceived;
        }

        private void WebRenderer_WebMessageReceived(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
        {
            string messageJson = JSONUtilities.Deserialize(e.WebMessageAsJson);
            JSONReader json = JSONReader.Parse(messageJson);
            Element targetElement = GetElementById(json["id"]);

            if (targetElement.Events.ContainsKey(json["eventName"]))
            {
                targetElement.Events[json["eventName"]].Invoke(new EventArguments() { args = json });
            }
        }

        private Element GetElementById(string id)
        {
            return Elements.Where(t => t.Id == id).First();
        }
    }
}
