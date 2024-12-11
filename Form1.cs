using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
namespace printerAPI
{
    public partial class Form1 : Form
    {
        MainForm fm = new MainForm();
        public Form1(MainForm form)
        {
            form = fm;
            InitializeComponent();
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(Directory.GetCurrentDirectory(), "setting.xml"));
            XmlNode node = doc.DocumentElement.SelectSingleNode("id_merchant");
            XmlNode name = doc.DocumentElement.SelectSingleNode("name_merchant");
            XmlNode printer = doc.DocumentElement.SelectSingleNode("printer_name");
            XmlNode size = doc.DocumentElement.SelectSingleNode("size");
            XmlNode offsite = doc.DocumentElement.SelectSingleNode("offsite");
            string text = node.InnerText;
            string textName = name.InnerText;
            textBoxIdMerchant.Text = text;
            textBoxName.Text = textName;
            textBoxSize.Text = size.InnerText;
            textBoxOffsite.Text = offsite.InnerText;
            textBoxPrinterName.Text = printer.InnerText.ToString();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(Directory.GetCurrentDirectory(), "setting.xml"));
            XmlNode node = doc.DocumentElement.SelectSingleNode("id_merchant");
            XmlNode name = doc.DocumentElement.SelectSingleNode("name_merchant");
            XmlNode printer = doc.DocumentElement.SelectSingleNode("printer_name");
            XmlNode size = doc.DocumentElement.SelectSingleNode("size");
            XmlNode offsite = doc.DocumentElement.SelectSingleNode("offsite");
            node.InnerText = textBoxIdMerchant.Text;
            name.InnerText = textBoxName.Text;
            size.InnerText = textBoxSize.Text;
            offsite.InnerText = textBoxOffsite.Text;
            printer.InnerText = textBoxPrinterName.Text;
            doc.Save(Path.Combine(Directory.GetCurrentDirectory(), "setting.xml"));

            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBox.Show("Data Saved", "Setting", buttons, MessageBoxIcon.Information);
            //fm.labelStatus.Invoke(new Action(() => fm.labelStatus.Text += Environment.NewLine + "Data Saved"));

            fm.readXml();
            fm.InitMQTTServer();
            this.Close();
        }
    }
}
