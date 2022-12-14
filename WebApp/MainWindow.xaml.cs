using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows;

namespace WebApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        Element buttonElement = new Element("buttonElement");

        public MainWindow()
        {
            InitializeComponent();
        }

        void InitializeEvents()
        {
            buttonElement.AddEventListener("click", (EventArguments args) =>
            {
                MessageBox.Show(args.args["message"]);
            });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            WebAppController.Initialize(this, WebRenderer);
            InitializeEvents();
        }
    }
}
