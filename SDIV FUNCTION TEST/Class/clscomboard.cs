using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SDIV_FUNCTION_TEST
{
    public class clscomboard
    {
        SerialPort SPBoard;
        clsdataconvert dataconvert;

        private string _COMnum;

        public string COMnum
        {
            get { return _COMnum; }
            set { _COMnum = value; }
        }

        public clscomboard()
        {
            SPBoard = new SerialPort();
            dataconvert = new clsdataconvert();
        }

        public bool ketnoi()
        {
            try
            {
                SPBoard.PortName = _COMnum;
                SPBoard.BaudRate = 115200;
                SPBoard.DataBits = 8;
                SPBoard.ReadBufferSize = 1024;
                SPBoard.WriteBufferSize = 512;
                SPBoard.Parity = Parity.Even;
                SPBoard.DtrEnable = true;
                SPBoard.DataReceived += SPBoard_DataReceived;
                SPBoard.Open();
                
                return true;
            }
            catch (Exception)
            {
                SPBoard.Close();
                return false;
            }
        }

        public void ngatketnoi()
        {
            try
            {
                SPBoard.Close();
            }
            catch (Exception)
            {
                
            }
        }

        private void SPBoard_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //string a = SPBoard.ReadLine();
            //MessageBox.Show(a);
        }

        public string ReadBlock_1W(string command, string address, string len)
        {
            try
            {
                string sum_string, sum_data, send_data, RB;
                sum_string = "MA" + command + address + dataconvert.insert_0_left(len, 2);
                sum_data = dataconvert.summake(sum_string);
                send_data = Convert.ToString((char)2) + sum_string + sum_data + Convert.ToString((char)3);
                SPBoard.Write(send_data);
                Thread.Sleep(200);
                RB = SPBoard.ReadExisting();
                try
                {
                    return RB.Substring(2, RB.Length - 6);
                }
                catch (Exception)
                {
                    return "NAK";
                }
            }
            catch (Exception)
            {
                return "NAK";
            }
        }

        public bool WriteBlock_1W(string command, string address, string len,string data)
        {
            try
            {
                string sum_string, sum_data, send_data, WB;
                sum_string = "MC" + command + address + len + data;
                sum_data = dataconvert.summake(sum_string);
                send_data = Convert.ToString((char)2) + sum_string + sum_data + Convert.ToString((char)3);

                SPBoard.Write(send_data);
                Thread.Sleep(200);
                WB = SPBoard.ReadExisting();

                if (WB == Convert.ToString((char)6))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public bool WriteCommand_1W(string command, string address, string len)
        {
            try
            {
                string sum_string, sum_data, send_data, WB;
                sum_string = "MB" + command + len + address;
                sum_data = dataconvert.summake(sum_string);
                send_data = Convert.ToString((char)2) + sum_string + sum_data + Convert.ToString((char)3);

                SPBoard.Write(send_data);
                Thread.Sleep(100);
                WB = SPBoard.ReadExisting();

                if (WB == Convert.ToString((char)6))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            
        }
        public bool WriteBlock_WW(string command, string address, string len, string data)
        {
            try
            {
                string sum_string, sum_data, send_data, WB;
                sum_string = "WW" + command + address + len + data;
                sum_data = dataconvert.summake(sum_string);
                send_data = Convert.ToString((char)2) + sum_string + sum_data + Convert.ToString((char)3);

                SPBoard.Write(send_data);
                Thread.Sleep(50);
                WB = SPBoard.ReadExisting();

                if (WB == Convert.ToString((char)6))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool WriteBlock_SMB(string command, string data)
        {
            try
            {
                string sum_string, sum_data, len, send_data, WB;
                byte pecbyte = 0;
                len = dataconvert.hex_str2((byte)(data.Length / 2)).Substring(0, 1) + dataconvert.hex_str2((byte)(data.Length / 2)).Substring(1, 1);
                sum_string = "BW" + len + "16" + command + data + pecbyte;
                sum_data = dataconvert.summake(sum_string);
                send_data = Convert.ToString((char)2) + sum_string + sum_data + Convert.ToString((char)3);

                SPBoard.Write(send_data);
                Thread.Sleep(50);
                WB = SPBoard.ReadExisting();

                if (WB == Convert.ToString((char)6))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool WriteWord_SMB(string command, string data)
        {
            try
            {
                string sum_string, sum_data, data_HHLL, send_data, WW;
                byte pecbyte = 0;
                data_HHLL = dataconvert.HHLL_LLHH(data);
                sum_string = "WW16" + command + data_HHLL + pecbyte;
                sum_data = dataconvert.summake(sum_string);
                send_data = Convert.ToString((char)2) + sum_string + sum_data + Convert.ToString((char)3);

                SPBoard.Write(send_data);
                Thread.Sleep(50);
                WW = SPBoard.ReadExisting();

                if (WW == Convert.ToString((char)6))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public string ReadWordSMB(string command)
        {
            try
            {
                string sum_string, sum_data, send_data, RW;
                byte pecbyte = 0;
                sum_string = "RW" + "16" + command + pecbyte;
                sum_data = dataconvert.summake(sum_string);
                send_data = Convert.ToString((char)2) + sum_string + sum_data + Convert.ToString((char)3);

                SPBoard.Write(send_data);
                Thread.Sleep(50);
                RW = SPBoard.ReadExisting();

                if (RW.Length >8)
                {
                    return (RW.Substring(2, RW.Length - 8)).Substring(2, 2) + (RW.Substring(2, RW.Length - 8)).Substring(0, 2);
                }
                else
                {
                    return "NAK";
                }
            }
            catch (Exception)
            {
                return "NAK";
            }
            
        }

        public string ReadBlockSMB(string command)
        {
            try
            {
                string sum_string, sum_data, send_data, RB;
                byte pecbyte = 0;

                sum_string = "BR" + "16" + command + pecbyte;
                sum_data = dataconvert.summake(sum_string);
                send_data = Convert.ToString((char)2) + sum_string + sum_data + Convert.ToString((char)3);

                RB = "";
                for (int trycount = 0; trycount < 40; trycount++)
                {
                    SPBoard.Write(send_data);
                    Thread.Sleep(100);
                    RB = SPBoard.ReadExisting();
                    if (RB.Length > 4)
                    {
                        if (RB.Substring(5, 4) != "FFFF") break;
                    }
                }

                if (RB.Length > 4)
                {
                    return RB.Substring(5, RB.Length - 11);
                }
                else
                {
                    return "NAK";
                }
            }
            catch (Exception)
            {
                return "NAK";
            }
            
        }

        public void wakeup(int socell)
        {
            try
            {
                switch (socell)
                {
                        //địa chỉ 4 cổng đầu ra của UC_comboard
                    case 1:
                        SPBoard.Write(Convert.ToString((char)2) + "PT11" + Convert.ToString((char)3));
                        break;
                    case 2:
                        SPBoard.Write(Convert.ToString((char)2) + "PT02" + Convert.ToString((char)3));
                        break;
                    case 3:
                        SPBoard.Write(Convert.ToString((char)2) + "PT04" + Convert.ToString((char)3));
                        break;
                    case 4:
                        SPBoard.Write(Convert.ToString((char)2) + "PT08" + Convert.ToString((char)3));
                        break;
                    case 5:
                        SPBoard.Write(Convert.ToString((char)2) + "PT00" + Convert.ToString((char)3));
                        break;
                }
            }
            catch (Exception)
            {
                ;
            }
            
        }

        public void Offcharge()
        {
           //SPBoard.Write(Convert.ToString((char)2) + "PT00" + Convert.ToString((char)3));
        }
    }
}
