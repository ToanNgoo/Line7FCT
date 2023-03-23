using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SDIV_FUNCTION_TEST
{
    public class clsDAQ
    {
        SerialPort DAQ;
        clsdataconvert dataconvert;

        private string _COMnum;

        public string COMnum
        {
            get { return _COMnum; }
            set { _COMnum = value; }
        }

        public clsDAQ()
        {
            DAQ = new SerialPort();
            dataconvert = new clsdataconvert();
        }

        public bool ketnoi()
        {
            try
            {
                DAQ.PortName = _COMnum;
                DAQ.BaudRate = 19200;
                DAQ.DataBits = 8;
                DAQ.ReadBufferSize = 1024;
                DAQ.WriteBufferSize = 512;
                DAQ.Handshake = Handshake.None;
                DAQ.Parity = Parity.None;
                DAQ.DtrEnable = true;
                DAQ.DataReceived += DAQ_DataReceived;
                DAQ.Open();
                DAQ.Write("*IDN?\r\n");
                Thread.Sleep(1000);
                
                if (DAQ.ReadExisting().Substring(0, 19) == "HEWLETT-PACKARD,349")
                {
                    return true;
                }
                else return false;
            }
            catch (Exception)
            {
                DAQ.Close();
                return false;
            }

        }

        public void ngatketnoi()
        {
            try
            {
                DAQ.Close();
            }
            catch (Exception)
            {

            }
        }

        void DAQ_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
        }

        public void setupkenhdoDAQ(string str, ref string[] data, float offset)
        {
            try
            {
                string receivedata = "";
                DAQ.Write("*RST\r\n");
                Thread.Sleep(50);
                //DAQ.Write("ROUT:CHAN:DELAY 0,(" + str + ")\r\n");
                //Thread.Sleep(100);

                DAQ.Write("MEASure:VOLtage? AUTO,MAX,(" + str + ")\r\n");
                Thread.Sleep(50);

                receivedata = DAQ.ReadLine();
                string[] mangdulieu = new string[100];
                mangdulieu = receivedata.Split(',');
                for (int i = 0; i < mangdulieu.Length; i++)
                {
                    data[i + 1] = dataconvert.str2num(mangdulieu[i]).ToString();
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
            
        }
        public void setupkenhdoresistor(string str, ref string[] data,float offset)
        {
            string receivedata = "";
            DAQ.Write("*RST\r\n");
            Thread.Sleep(50);
            //DAQ.Write("ROUTe:CHAN:DELAY 0,(" + str + ")\r\n");
            //Thread.Sleep(500);
            DAQ.Write("MEASure:RESistance? AUTO,MAX,(" + str + ")\r\n");
            Thread.Sleep(50);
      
            receivedata = DAQ.ReadLine();
      
            string[] mangdulieu = new string[100];
            mangdulieu = receivedata.Split(',');
            for (int i = 0; i < mangdulieu.Length; i++)
            {
                data[i + 1] = dataconvert.str2numdap(mangdulieu[i]).ToString();
            
            }
            DAQ.Write("*RST\r\n");
            
            
        }
        public void setupkenhdoTEMP(string str, ref string[] data, float offset)
        {
            string receivedata = "";
           
            DAQ.Write("*RST\r\n");
            Thread.Sleep(100);
            DAQ.Write("MEASure:TEMPerature? THERmistor,10000,1,MAX,(" + str + ")\r\n");
            Thread.Sleep(200);
            
            receivedata = DAQ.ReadLine();
            string[] mangdulieu = new string[100];
            mangdulieu = receivedata.Split(',');
            for (int i = 0; i < mangdulieu.Length; i++)
            {
                data[i + 1] = dataconvert.str2numdap(mangdulieu[i]).ToString();

            }
            DAQ.Write("*RST\r\n");
            
        }
    }
}
