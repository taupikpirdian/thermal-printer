using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Threading;
using System.Xml;
using System.IO;
using Newtonsoft.Json;
using ESC_POS_USB_NET.Printer;
using ESC_POS_USB_NET.Enums;
using printerAPI.Properties;
using System.Globalization;
using System.Drawing.Printing;
using System.Printing;
using System.Security.Cryptography;
using System.Net.Http;
using System.Security.Policy;
using System.Net;

namespace printerAPI
{
    public partial class MainForm : Form
    {
        private IMqttClient MqttClient;
        private IMqttClientOptions options;

        private string MQTT_SERVER = "test.mosquitto.org";
        private string MQTT_CLIENT = "Printer_Desktop";
        MqttFactory factory = new MqttFactory();
        string topict = "printer";
        string topics = "";
        string printer_name = "";
        int font_size;
        int offsi;

        public MainForm()
        {
            InitializeComponent();
            InitMQTTServer();
            readXml();
        }

        public void readXml()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(Directory.GetCurrentDirectory(), "setting.xml"));
            XmlNode node = doc.DocumentElement.SelectSingleNode("id_merchant");
            XmlNode name = doc.DocumentElement.SelectSingleNode("name_merchant");
            XmlNode printer = doc.DocumentElement.SelectSingleNode("printer_name");
            XmlNode size = doc.DocumentElement.SelectSingleNode("size");
            XmlNode offsite = doc.DocumentElement.SelectSingleNode("offsite");
            string text = node.InnerText;
            labelName.Text = ": " + name.InnerText.ToString();
            printer_name = printer.InnerText;
            labelPrinterName.Text = ": " + printer_name;
            topics = topict + text;
            font_size = Int32.Parse(size.InnerText);
            offsi = Int32.Parse(offsite.InnerText);
            labelStatus.Text += "Welcome!  " + name.InnerText.ToString();

            Console.WriteLine(topics);
        }

        public void InitMQTTServer()
        {
            var factory = new MqttFactory();

            MqttClient = factory.CreateMqttClient();
            MqttClient.UseDisconnectedHandler(async e =>
            {
                //labelConnectionServer.Text = ": no Connection";

                await Task.Delay(TimeSpan.FromSeconds(5));
                try
                {
                    ConnectMqtt();
                }
                catch
                {
                    labelConnectionServer.Text = ": Connecting";

                    Console.WriteLine(": Connecting");
                    //if (labelConnectionServer.InvokeRequired)
                    //    labelConnectionServer.Invoke(new Action(() => labelConnectionServer.Text = ": Connecting"));
                }
            });
            options = new MqttClientOptionsBuilder()
                .WithClientId(MQTT_CLIENT)
                .WithTcpServer(MQTT_SERVER)
                .Build();
            _ = MqttClient.UseConnectedHandler(async e =>
            {
                await MqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(topics).Build());
            });
            MqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                if (e != null)
                {
                    string topic = e.ApplicationMessage.Topic.ToString();
                    String payload = e.ApplicationMessage.Payload != null ? Encoding.UTF8.GetString(e.ApplicationMessage.Payload) : "";
                    Console.WriteLine(topic);
                    //Console.WriteLine(payload);

                    if (topic == topics)
                    {
                        try
                        {
                            jsonDeseriliaze(payload);
                        }
                        catch (Exception error)
                        {
                            //Alert(alertError, "Error when parsing method::dataGps() \n" + error.Message);
                            //MessageBox.Show(error.Message, "Error when parsing method::dataGps() \n");
                            labelStatus.Invoke(new Action(() => labelStatus.Text += Environment.NewLine + error.Message + "Error when parsing method::data() \n"));

                        }
                    }
                }
            });
            ConnectMqtt();
        }

        public async void ConnectMqtt()
        {
            try
            {
                await MqttClient.ConnectAsync(options, CancellationToken.None);
                labelConnectionServer.Text = ": Connected";
                
            }
            catch (Exception e)
            {
                labelConnectionServer.Text = ": No Connection";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 fr = new Form1(this);
            fr.Show();
        }

        PrintDocument pd = new PrintDocument();

        private async void jsonDeseriliaze(string payload)
        {
            Console.WriteLine("Payload: " + payload);

            var getResponse = JsonConvert.DeserializeObject<Rootobject>(payload);

            string logoPath = @"C:\Users\manual\OneDrive\Documents\Code\1.Project\printer_csharp\bin\Debug\downloaded_image.jpg";

            PrintDocument printDocument = new PrintDocument();

            printDocument.PrinterSettings.PrinterName = "EPSON TM-U220 Receipt";

            //printDocument.PrintPage += (sender, e) =>
            //{
            //    Image logo = Image.FromFile(logoPath);

            //    int targetWidth = 150;
            //    int targetHeight = (int)((double)logo.Height / logo.Width * targetWidth);
            //    Image resizedLogo = new Bitmap(logo, new Size(targetWidth, targetHeight));

            //    int paperWidth = (int)e.PageBounds.Width; // Lebar kertas
            //    int centerX = (paperWidth - targetWidth) / 2; // Posisi X di tengah
            //    int centerY = 10; // Posisi Y (offset dari atas)

            //    e.Graphics.DrawImage(resizedLogo, new Point(0, 0));

            //    // Cetak logo di bagian atas
            //    //e.Graphics.DrawImage(resizedLogo, new Point(0, 0));
            //};

            printDocument.Print();

            print(getResponse);
        }


        static void PrintInvoiceItem(Printer printer, int paperWidth, string title, string value)
        {
            int valueWidth = paperWidth - title.Length - 1; 

            string indent = new string(' ', title.Length - title.Length + 3);

            while (value.Length > valueWidth)
            {
                string lineValue = value.Substring(0, valueWidth);
                value = value.Substring(valueWidth);
                printer.Append($"{title.PadRight(paperWidth - valueWidth - 1)} {lineValue}");
                title = indent; 
            }

            //printer.BoldMode($"{paperWidth - value.Length - 3} {value}");
            printer.Append($"{title.PadRight(paperWidth - value.Length - 1)} {value}");
            //printer.NewLine();
        }

        NumberFormatInfo nfi = new CultureInfo("id-ID", false).NumberFormat;

        void print(Rootobject payload)
        {
            Printer printer = new Printer("EPSON TM-U220 Receipt");
            try
            {
                int paperWidth = offsi;

                string separator = new string('-', paperWidth);

                printer.Clear();
                printer.ExpandedMode(PrinterModeState.On);
                printer.NormalWidth();

                printer.NewLines(2);
                printer.AlignCenter();
                printer.BoldMode(payload.header.title);

                printer.Append(payload.merchant_name);

                printer.DoubleWidth3();
                //printer.BoldMode(payload.header.address);
                if (payload.advance_header)
                {
                    PrintLeftAlignedText(printer, paperWidth, payload.header.address.Replace("<br>", "\n"));
                    printer.Append(payload.header.phone);
                }

                printer.NewLines(2);

                PrintInvoiceItem(printer, paperWidth, "No. Transaksi    :", payload.no_order);
                PrintInvoiceItem(printer, paperWidth, "Tgl. Transaksi   :", payload.tanggal);
                PrintInvoiceItem(printer, paperWidth, "Layanan          :", payload.servis);
                PrintInvoiceItem(printer, paperWidth, "Nama Kasir       :", payload.kasir_name);
                PrintInvoiceItem(printer, paperWidth, "ID Pelanggan     :", payload.customer_id);
                PrintInvoiceItem(printer, paperWidth, "Nama Pelanggan   :", payload.customer_name);
                PrintInvoiceItem(printer, paperWidth, "No. Pelanggan    :", payload.customer_phone);
                printer.AlignLeft();
                printer.Append("Alamat Pelanggan :");
                PrintLeftAlignedText(printer, paperWidth, payload.customer_address);
                PrintInvoiceItem(printer, paperWidth, "Catatan          :", payload.notes);
                printer.NewLines(2);

                printer.AlignLeft();
                printer.DoubleWidth2();
                printer.BoldMode("Detail Pesanan");
                printer.Append(separator);

                foreach (var detail in payload.detail_order)
                {
                    PrintInvoiceItem(printer, paperWidth, "Currency :", detail.currency);
                    PrintInvoiceItem(printer, paperWidth, "Amount   :", detail.amount.ToString("N0", nfi));
                    PrintInvoiceItem(printer, paperWidth, "Rate     :", String.Format("{0:0}", detail.rate));
                    PrintInvoiceItem(printer, paperWidth, "Total    :", "Rp " + detail.total.ToString("N0", nfi));
                    printer.Append(separator);
                }
                printer.NewLine();

                PrintInvoiceItem(printer, paperWidth, "Sub Total :", "Rp " + payload.sub_total.ToString("N0", nfi));

                if (payload.ongkir != 0)
                {
                    PrintInvoiceItem(printer, paperWidth, "Ongkir    :", "Rp " + payload.ongkir.ToString("N0", nfi));
                }

                if (payload.pajak != 0)
                {
                    PrintInvoiceItem(printer, paperWidth, "Pajak     :", "Rp " + payload.pajak.ToString("N0", nfi));
                }

                if (payload.diskon != 0)
                {
                    PrintInvoiceItem(printer, paperWidth, "Diskon    :", "Rp " + payload.diskon.ToString("N0", nfi));
                }

                PrintInvoiceItem(printer, paperWidth, "Total     :", "Rp " + payload.total.ToString("N0", nfi));
                printer.Append(separator);
                printer.NewLine();

                printer.BoldMode("Pembayaran");
                printer.AlignLeft();
                printer.Append("Metode Pembayaran :");
                PrintLeftAlignedText(printer, paperWidth, payload.metode_pembayaran);
                //PrintInvoiceItem(printer, paperWidth, "Metode Pembayaran : ", payload.metode_pembayaran);
                printer.NewLine();
                foreach (var summary in payload.summary_payment)
                {
                    PrintInvoiceItem(printer, paperWidth, summary.metode, "Rp " + summary.amount.ToString("N0", nfi));
                }

                printer.Append(separator);

                PrintInvoiceItem(printer, paperWidth, "Pembayaran :", "Rp " + payload.money_received.ToString("N0", nfi));
                PrintInvoiceItem(printer, paperWidth, "Kembalian  :", "Rp " + payload.money_back.ToString("N0", nfi));

                printer.NewLines(2);
                //printer.Append(payload.footer.statement);
                PrintLeftAlignedText(printer, paperWidth, payload.footer.statement.Replace("<br>", "\n"));
                printer.NewLines(2);

                PrintInvoiceItem(printer, paperWidth, "    Served By :", "Customer    ");
                printer.NewLines(6);
                string kasir = ProcessName(payload.kasir_name);
                string customer = ProcessName(payload.customer_name);
                PrintInvoiceItem(printer, paperWidth, kasir, customer);

                printer.NewLines(2);
                printer.BoldMode("Perhatian / Note");
                //printer.Append(payload.footer.note_id.Replace("<br>", "\n"));
                PrintLeftAlignedText(printer, paperWidth, payload.footer.note_id.Replace("<br>", "\n"));
                printer.NewLine();
                //printer.Append(payload.footer.note_en.Replace("<br>", "\n"));
                PrintLeftAlignedText(printer, paperWidth, payload.footer.note_en.Replace("<br>", "\n"));
                printer.NewLines(2);

                printer.ExpandedMode(PrinterModeState.Off);
                printer.FullPaperCut();
                printer.PrintDocument();
                labelStatus.Invoke(new Action(() => labelStatus.Text += Environment.NewLine + "Pencetakan Selesai"));

            }
            catch (Exception ex)
            {
                Console.WriteLine("Terjadi kesalahan: " + ex.Message);
                labelStatus.Invoke(new Action(() => labelStatus.Text += Environment.NewLine + "Terjadi kesalahan: " + ex.Message));
            }
        }

        static string ProcessName(string name)
        {
            int maxLength = 16;

            if (name.Length > maxLength)
            {
                name = TruncateToMaxLength(name, maxLength);
            }

            return CenterText(name, maxLength);
        }

        static string TruncateToMaxLength(string text, int maxLength)
        {
            string[] words = text.Split(' ');
            string result = "";

            foreach (var word in words)
            {
                if ((result + word).Length > maxLength)
                {
                    break;
                }

                if (result.Length > 0)
                {
                    result += " ";
                }

                result += word;
            }

            return result.TrimEnd();
        }

        static string CenterText(string text, int totalWidth)
        {
            if (text.Length >= totalWidth)
            {
                return text;
            }

            int padding = (totalWidth - text.Length) / 2;
            return text.PadLeft(text.Length + padding).PadRight(totalWidth);
        }

        static void PrintRightAlignedText(Printer printer, int paperWidth, string text)
        {
            string[] lines = GetWrappedLiness(text, paperWidth);

            foreach (string line in lines)
            {
                printer.AlignRight();
                printer.Append(line);
            }
        }

        static void PrintLeftMenjorokAlignedText(Printer printer, int paperWidth, string text)
        {
            string[] lines = GetWrappedLiness(text, paperWidth);

            foreach (string line in lines)
            {
                printer.DoubleWidth3();
                printer.Append(line);
                //printer.NewLine();
            }
        }

        static void PrintLeftAlignedText(Printer printer, int paperWidth, string text)
        {
            string[] lines = GetWrappedLiness(text, paperWidth);

            foreach (string line in lines)
            {
                printer.Append(line);
                //printer.NewLine();
            }
        }

        static string[] GetWrappedLiness(string text, int paperWidth)
        {
            string[] words = text.Split(' ');

            string currentLine = "";
            int lineWidth = 0;
            int spaceWidth = 1; 
            var lines = new System.Collections.Generic.List<string>();

            foreach (string word in words)
            {
                if (lineWidth + word.Length + spaceWidth > paperWidth)
                {
                    lines.Add(currentLine.Trim());

                    currentLine = "";
                    lineWidth = 0;
                }

                currentLine += word + " ";
                lineWidth += word.Length + spaceWidth;
            }

            lines.Add(currentLine.Trim());

            return lines.ToArray();
        }

        static string GetPrinterStatus(PrintQueueStatus status)
        {
            if ((status & PrintQueueStatus.PaperOut) != 0)
            {
                return "Out of Paper";
            }
            else if ((status & PrintQueueStatus.PaperJam) != 0)
            {
                return "Paper Jam";
            }
            else if ((status & PrintQueueStatus.Offline) != 0)
            {
                return "Offline";
            }
            else if ((status & PrintQueueStatus.Error) != 0)
            {
                return "Error";
            }
            else if ((status & PrintQueueStatus.Paused) != 0)
            {
                return "Paused";
            }
            else if ((status & PrintQueueStatus.Printing) != 0)
            {
                return "Printing";
            }
            else if ((status & PrintQueueStatus.PendingDeletion) != 0)
            {
                return "Pending Deletion";
            }
            //else if ((status & PrintQueueStatus.PendingInitialization) != 0)
            //{
            //    return "Pending Initialization";
            //}
            //else if ((status & PrintQueueStatus.Unknown) != 0)
            //{
            //    return "Unknown";
            //}
            else
            {
                return "Available";
            }
        }
    }


    public class Rootobject
    {
        public string merchant_name { get; set; }
        public string no_order { get; set; }
        public string tanggal { get; set; }
        public string type_transaction { get; set; }
        public string servis { get; set; }
        public string kasir_name { get; set; }
        public string customer_id { get; set; }
        public string customer_name { get; set; }
        public string customer_phone { get; set; }
        public string customer_address { get; set; }
        public string metode_pembayaran { get; set; }
        public double sub_total { get; set; }
        public double ongkir { get; set; }
        public double pajak { get; set; }
        public double diskon { get; set; }
        public double total { get; set; }
        public double money_received { get; set; }
        public double money_back { get; set; }
        public Detail_Order[] detail_order { get; set; }
        public string logo { get; set; }
        public Summary_Payment[] summary_payment { get; set; }
        public Header header { get; set; }
        public Footer footer { get; set; }
    }

    public class Header
    {
        public string title { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
    }

    public class Footer
    {
        public string statement { get; set; }
        public string note_id { get; set; }
        public string note_en { get; set; }
    }

    public class Detail_Order
    {
        public string currency { get; set; }
        public double amount { get; set; }
        public string rate { get; set; }
        public double total { get; set; }
    }

    public class Summary_Payment
    {
        public string metode { get; set; }
        public double amount { get; set; }
    }
}
