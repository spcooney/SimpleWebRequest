using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleWebRequest.WinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = WebGetTsp();
        }

        public static string WebGetTsp()
        {
            String respStr = String.Empty;
            // Get the current service point protocol type
            SecurityProtocolType spt = ServicePointManager.SecurityProtocol;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                DateTime nowDt = DateTime.Now;
                DateTime startDt = nowDt.AddMonths(-1);
                string tspUrl = String.Format(CultureInfo.InvariantCulture, @"https://secure.tsp.gov/components/CORS/getSharePricesRaw.html?startdate={0}&enddate={1}&Lfunds=1&InvFunds=1&download=0", startDt.ToString("yyyyMMdd"), nowDt.ToString("yyyyMMdd"));
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tspUrl);
                request.ClientCertificates.Add(new X509Certificate());
                WebResponse response = request.GetResponse();
                string htmlDocTxt = String.Empty;
                using (Stream receiveStream = response.GetResponseStream())
                {
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    respStr = readStream.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                respStr = ex.Message;
                return respStr;
            }
            finally
            {
                // Reset to the original value
                ServicePointManager.SecurityProtocol = spt;
            }
            return respStr;
        }
    }
}
