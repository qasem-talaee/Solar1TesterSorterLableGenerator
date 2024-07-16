using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Stimulsoft.Report.StiOptions.Designer;

namespace Solar1TesterSorterLableGenerator.Helper
{
    public class ScannerHelper
    {
        private SerialPort _serialPort;
        public string ComPort;
        public event EventHandler<string> ScanComplated;

        public ScannerHelper()
        {
            ComPort = ConfigurationSettings.AppSettings["ComPort"];
            try
            {
                _serialPort = new SerialPort("COM" + ComPort, 9600, Parity.None, 8, StopBits.One);
                _serialPort.Handshake = Handshake.None;
                _serialPort.ReadTimeout = 1500;
                _serialPort.Encoding = Encoding.ASCII;
                _serialPort.DtrEnable = true;
                _serialPort.RtsEnable = true;
                _serialPort.Open();
                _serialPort.DataReceived += _SerialPort_DataRecieved;
            }
            catch
            {
                MessageBox.Show("پورت کام پیدا نشد.با پشتیبانی نرم افزار تماس بگیرید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void _SerialPort_DataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] data = new byte[1024];
            int bytesRead = _serialPort.Read(data, 0, data.Length);
            var res = Encoding.ASCII.GetString(data, 0, bytesRead);
            if (res.Length > 1)
            {
                ScanComplated.Invoke(this, res);
            }
        }

        public void CloseSerialPort()
        {
            _serialPort.Close();
        }


        
    }
}
