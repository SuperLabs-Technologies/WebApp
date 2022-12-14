using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using HtmlAgilityPack;

namespace WebApp
{
    internal class InterfaceConfigurator
    {
        MainWindow mainWindow { get; set; }

        public InterfaceConfigurator(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void Layout(string html)
        {

            mainWindow.Visibility = Visibility.Visible;
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            HtmlNode body = htmlDoc.DocumentNode.ChildNodes["html"].ChildNodes["body"];
            HtmlNode head = htmlDoc.DocumentNode.ChildNodes["html"].ChildNodes["head"];

            /* Read and assign Window */
            if (body.ChildNodes.Select(t => t.Name == "window").Count() > 0)
            {
                HtmlNode windowConfig = body.ChildNodes["window"];

                /* Set window width */
                if (windowConfig.Attributes.Contains("width"))
                {
                    if (int.TryParse(windowConfig.Attributes["width"].Value, out int widthNumber))
                    {
                        mainWindow.Width = widthNumber;
                        RuntimeLog.WriteLog($"Configured window width: {widthNumber}px");
                    }
                    else
                    {
                        throw new Exception("Window.width is not a valid number");
                    }
                }

                /* Set window height */
                if (windowConfig.Attributes.Contains("height"))
                {
                    if (int.TryParse(windowConfig.Attributes["height"].Value, out int heightNumber))
                    {
                        mainWindow.Height = heightNumber;
                        RuntimeLog.WriteLog($"Configured window height: {heightNumber}px");
                    }
                    else
                    {
                        throw new Exception("Window.height is not a valid number");
                    }
                }
            }

            /* Set favicon */
            IEnumerable<HtmlNode> headLinks = head.ChildNodes.Where(t => t.Name == "link" && t.Attributes.Contains("rel"));
            if (headLinks.Count() > 0)
            {
                HtmlNode iconLink = headLinks.First();
                if (iconLink.Attributes.Contains("rel") && iconLink.Attributes["rel"].Value == "icon")
                {
                    if (iconLink.Attributes.Contains("href") && File.Exists(iconLink.Attributes["href"].Value.Replace("/", "\\")))
                    {
                        string path = iconLink.Attributes["href"].Value.Replace("/", "\\");

                        MemoryStream ms = new MemoryStream();
                        ((System.Drawing.Bitmap)new Bitmap(path)).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        ms.Seek(0, SeekOrigin.Begin);
                        image.StreamSource = ms;
                        image.EndInit();

                        mainWindow.Icon = image;
                        RuntimeLog.WriteLog($"Configured window favicon");
                    }
                }
            }


            /* Assign application title */
            if (head.ChildNodes.Select(t => t.Name == "title").Count() > 0)
            {
                mainWindow.Title = head.ChildNodes["title"].InnerText;
            }
        }
    }
}