using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace Cloud_MR
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int listenPort = 11000;
        UdpClient listener;
        IPEndPoint groupEP;
        CancellationTokenSource cts;
        DeserializingDataFromSensors deserializing;

        public MainWindow()
        {
            InitializeComponent();
            listener = new UdpClient(listenPort);
            groupEP = new IPEndPoint(IPAddress.Any, listenPort);
            StatusNetwork.Content = new AccessPointConfig().StatusNetwork;
        }
        private async Task GetMessages(CancellationToken token)
        {
            string json = "";
            string result = "";
            deserializing = new DeserializingDataFromSensors();
            while (!token.IsCancellationRequested) {
                json = await GetJSON();
                result = json.Replace("<", "").Replace(">", "");
                TextBox1.Text = json;
                deserializing = deserializing.FromJson(result);
            }
        }
        private async Task<string> GetJSON()
        {
            string incomingJSON = "";
            return await Task.Run(() => {
                byte[] bytes = listener.Receive(ref groupEP);
                incomingJSON = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                return incomingJSON;
            });
        }
        private async void WriteDataButtonClickAsync(object sender, RoutedEventArgs e)
        {
            WriteData.Click -= new RoutedEventHandler(WriteDataButtonClickAsync);
            WriteData.Click += new RoutedEventHandler(CancelWriteDataButtonClickAsync);
            cts = new CancellationTokenSource();
            WriteData.Content = "Отмена";
            await GetMessages(cts.Token);
        }
        private void CancelWriteDataButtonClickAsync(object sender, RoutedEventArgs e)
        {
            WriteData.Click += new RoutedEventHandler(WriteDataButtonClickAsync);
            WriteData.Click -= new RoutedEventHandler(CancelWriteDataButtonClickAsync);
            WriteData.Content = "Получить данные";
            if (cts != null) {
                cts.Cancel();
            }
        }
    }
}
