using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SDIV_FUNCTION_TEST
{
    public class clsSDIVBoard
    {
        SerialPort SDIVBoard;
        clsdataconvert dataconvert;

        private string _COMnum;

        private bool _resetflag;

        public bool Resetflag
        {
            get { return _resetflag; }
            set { _resetflag = value; }
        }
        private bool _startflag;

        public bool Startflag
        {
            get { return _startflag; }
            set { _startflag = value; }
        }
        private string _ketquaLED;
        private string _dongtesttool;
        private string _adcden1, _adcden2, _adcden3, _adcden4;
        private string _ntlden1, _ntlden2, _ntlden3, _ntlden4;
        private bool _clt, _cld;
        public string ketquaLED
        {
            get { return _ketquaLED;}
            set { _ketquaLED = value; }
        }
        public bool Cld
        {
            get { return _cld; }
            set { _cld = value; }
        }

        public bool Clt
        {
            get { return _clt; }
            set { _clt = value; }
        }
        private bool _ofs;

        public bool Ofs
        {
            get { return _ofs; }
            set { _ofs = value; }
        }
        private bool _idn;

        public string COMnum
        {
            get { return _COMnum; }
            set { _COMnum = value; }
        }

        public clsSDIVBoard()
        {
            SDIVBoard = new SerialPort();
            dataconvert = new clsdataconvert();
            _resetflag = false;
            _startflag = false;
            _clt = false;
            _cld = false;
            _idn = false;
            _ketquaLED = "";
            _dongtesttool = "";
            _adcden1 = "";
            _adcden2 = "";
            _adcden3 = "";
            _adcden4 = "";
            _ntlden1 = "";
            _ntlden2 = "";
            _ntlden3 = "";
            _ntlden4 = "";
        }

        public bool ketnoi()
        {
            try
            {
                SDIVBoard.PortName = _COMnum;
                SDIVBoard.BaudRate = 115200;
                SDIVBoard.DataBits = 8;
                SDIVBoard.ReadBufferSize = 1024;
                SDIVBoard.WriteBufferSize = 512;
                SDIVBoard.Handshake = Handshake.None;
                SDIVBoard.Parity = Parity.None;
                SDIVBoard.DtrEnable = true;
                SDIVBoard.DataReceived += SDIVBoard_DataReceived;
                SDIVBoard.Open();
                SDIVBoard.Write("I\n");
                Thread.Sleep(700);
                if (_idn == true)
                {
                    _idn = false;
                    return true;
                }
                else return false;
            }
            catch (Exception)
            {
                SDIVBoard.Close();
                return false;
            }

        }

        void SDIVBoard_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            /*
             * RST: Reset khi NG do board gửi lên
             * STA: Tín hiệu start do board gửi lên
             * VAL: Giá trị kiểm tra đèn (ví dụ: 0000, 0101) (Gửi xuống: S)
             * TOL: Kiểm tra dòng khi test tool (Gửi xuống: L)
             * ADC: Giá trị ADC đọc được (Gửi xuống: R)
             * NTL: Giá trị ADC thiết lập (Gửi xuống: N)
             * CLT: Thông báo hoàn thành hiệu chuẩn ngưỡng trên (Gửi xuống: T)
             * CLD: Thông báo hoàn thành hiệu chuẩn ngưỡng dưới (Gửi xuống: D)
             * OFS: Offset giá trị ADC từng đèn (Gửi xuống: Kênh đèn + Giá trị ADC)
             * LED: Chuỗi nhận dạng thiết bị (Gửi xuống: I)
             */
            string str = "";
            str = SDIVBoard.ReadLine();
            if (str.Trim().Length >= 3 && str.Length <=21)
            {
                switch (str.Substring(0, 3))
                {
                    case "RST":
                        _resetflag = true;
                        break;
                    case "STA":
                        _startflag = true;
                        break;
                    case "VAL":
                        _ketquaLED = str.Substring(3, 4);
                       // MessageBox.Show(_ketquaLED);
                        break;
                    case "TOL":
                        _ketquaLED = str.Substring(3, 4);
                        break;
                    case "ADC":
                        _adcden1 = str.Substring(3, 4);
                        _adcden2 = str.Substring(7, 4);
                        _adcden3 = str.Substring(11, 4);
                        _adcden4 = str.Substring(15, 4);
                        break;
                    case "NTL":
                        _ntlden1 = str.Substring(3, 4);
                        _ntlden2 = str.Substring(7, 4);
                        _ntlden3 = str.Substring(11, 4);
                        _ntlden4 = str.Substring(15, 4);
                        break;
                    case "CLT":
                        _clt = true;
                        break;
                    case "CLD":
                        _cld = true;
                        break;
                    case "OFS":
                        _ofs = true;
                        MessageBox.Show("Bạn đã Offset thành công", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case "LED":
                        _idn = true;
                        break;
                }
            }
        }

        public void ngatketnoi()
        {
            try
            {
                SDIVBoard.Close();
            }
            catch (Exception)
            {

            }
        }

        public void hieuchuanmuctren()
        {
            SDIVBoard.Write("T\r");
        }
        public void hieuchuanmucduoi()
        {
            SDIVBoard.Write("D\r");
        }

        public void thietlapnguong(ref int den1, ref int den2, ref int den3, ref int den4)
        {
            SDIVBoard.Write("N\r");
            Thread.Sleep(200);
            den1 = int.Parse(_ntlden1);
            den2 = int.Parse(_ntlden2);
            den3 = int.Parse(_ntlden3);
            den4 = int.Parse(_ntlden4);

            _ntlden1 = "";
            _ntlden2 = "";
            _ntlden3 = "";
            _ntlden4 = "";
        }

        public string thietlapnguong()
        {
            string str;
            SDIVBoard.Write("N\r");
            Thread.Sleep(200);

            str = _ntlden1 + " - " + _ntlden2 + " - " + _ntlden3 + " - " + _ntlden4;

            _ntlden1 = "";
            _ntlden2 = "";
            _ntlden3 = "";
            _ntlden4 = "";

            return str;
        }

        public void docADC(ref int den1, ref int den2, ref int den3, ref int den4)
        {
            SDIVBoard.Write("R\r");
            Thread.Sleep(200);
            den1 = int.Parse(_adcden1);
            den2 = int.Parse(_adcden2);
            den3 = int.Parse(_adcden3);
            den4 = int.Parse(_adcden4);

            _adcden1 = "";
            _adcden2 = "";
            _adcden3 = "";
            _adcden4 = "";
        }

        public string docADC()
        {
            SDIVBoard.Write("R\r");
            Thread.Sleep(200);
            string str = _adcden1 + _adcden2 + _adcden3 + _adcden4;
            _adcden1 = "";
            _adcden2 = "";
            _adcden3 = "";
            _adcden4 = "";
           
            return str;
        }

        public string docADCtest()
        {
            SDIVBoard.Write("R\r");
            Thread.Sleep(200);
            string str = _adcden1 + "-" + _adcden2 + "-" + _adcden3 + "-" + _adcden4;
            _adcden1 = "";
            _adcden2 = "";
            _adcden3 = "";
            _adcden4 = "";

            return str;
        }

        public string docketqua()
        {
            string str;
            SDIVBoard.Write("S\r");
            while (_ketquaLED.Length == 0) { }
            str = _ketquaLED;
            _ketquaLED = "";
            return str;
        }

        public string docdongtesttool()
        {
            string str;
            SDIVBoard.Write("L\r");
            Thread.Sleep(200);
            str = _dongtesttool;
            return str;
        }
        public void offset(string offset,string led)
        {
            
            if(offset.Length==0)
            {
                SDIVBoard.Write("O"+led+ "000" + offset);
            }else if(offset.Length==1)
            {
                SDIVBoard.Write("O" +led+ "00" + offset);
            }else if(offset.Length==2)
            {
                SDIVBoard.Write("O" +led+ "0" + offset);
            }
            else
            {
                SDIVBoard.Write("O" +led+offset);
            }

        }
    }
}
