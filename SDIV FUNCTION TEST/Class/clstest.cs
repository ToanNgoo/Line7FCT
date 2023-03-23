using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;
using System.ComponentModel;
using System.Linq;
using System.Diagnostics;
namespace SDIV_FUNCTION_TEST
{
    enum ketquatest
    {
        OK, NG
    }
    public class clstest
    {
        clshienthidata hienthidata;
        clsLocaldb Localdb;
        clsdataconvert dataconvert;
        clsMakelog makelog;
        OptionDefine.clsCheckTrungInformation checkdBoxed = new OptionDefine.clsCheckTrungInformation();
        List<string> ListBarcodeBoxed1 = new List<string>();
        List<string> ListBarcodeBoxed2 = new List<string>();
        List<string> ListBarcodeBoxed3 = new List<string>();
        private ketquatest _ketqua;
        int _repeat, nrp = 0;
        //int pin1, pin2, pin3, pin4;
        bool _timecheck_flag;

        //public int kqtk1
        //{
        //    get { return _kqtk1; }
        //    set { _kqtk1 = value; }
        //}
        //public int kqtk2
        //{
        //    get { return _kqtk2; }
        //    set { _kqtk2 = value; }
        //}

        public int Repeat
        {
            get { return _repeat; }
            set { _repeat = value; }
        }
        public bool timecheck_flag
        {
            get { return _timecheck_flag; }
            set { _timecheck_flag = value; }
        }
        internal ketquatest Ketqua
        {
            get { return _ketqua; }
            set { _ketqua = value; }
        }

        private bool _stopflag;

        public bool Stopflag
        {
            get { return _stopflag; }
            set { _stopflag = value; }
        }
        private int _ktrkq;

        public int ktrkq
        {
            get { return _ktrkq; }
            set { _ktrkq = value; }
        }
        private bool _testflag;

        public bool Testflag
        {
            get { return _testflag; }
            set { _testflag = value; }
        }

        private string _ProgramProcess;

        public string ProgramProcess
        {
            get { return _ProgramProcess; }
            set { _ProgramProcess = value; }
        }

        public int _error_flag;
        public int error_flag
        {
            get { return _error_flag; }
            set { _error_flag = value; }
        }
        public clstest()
        {
            hienthidata = new clshienthidata();
            Localdb = new clsLocaldb();
            dataconvert = new clsdataconvert();
            makelog = new clsMakelog();
            _testflag = false;
            _stopflag = false;
        }

        public void test(DataGridView dtvspec, Label lbresult, ProgressBar pgb, Label lblOK, Label lblNG, Label tile,
                        string packcode, int kenh, string ic, bool system, string DateMes, string SNMes, float offsetocv, int offsetir,
                        clsScanner scan, clscomboard com, clsDAQ daq, clsDVM dvm, clsHioki hioki, clsPWS pwsl, clsSDIVBoard sdiv, Frmmain frm, bool checkfail, ToolStripLabel lbrepeat, clsPLC PLC, TextBox model, Stopwatch stw, int count, bool autoscroll, bool special_control, int tabSelected)
        {
            /*
             * Thực hiện kiểm tra tính năng
             */
            if (packcode.Length != 0)
            {
                stw.Reset();
                stw.Start();
                _testflag = true;
                int item_id;
                string jtype, specmin, specmax, result, rawdata;
                string[] P = new string[100];
                string[] DAQVoltage = new string[100];
                //int errorflag = 0;
                _error_flag = 0;
                _ktrkq = 0;
                int k = 1;
                int PVM = 0;
                rawdata = "";
                string judgedatatype = "";
                string _barcodePCM = "";


                hienthidata.clearresult(dtvspec, lbresult, pgb, kenh);

                for (int irow = 0; irow < dtvspec.RowCount; irow++)
                {
                    if (irow % 14 == 0 && autoscroll && kenh == 1)
                    {
                        dtvspec.FirstDisplayedScrollingRowIndex = irow;
                    }
                    pgb.Value = (irow + 1) * 100 / dtvspec.RowCount;
                    item_id = int.Parse(dtvspec.Rows[irow].Cells[1].Value.ToString());
                    jtype = dtvspec.Rows[irow].Cells[3].Value.ToString();
                    specmin = dtvspec.Rows[irow].Cells[4].Value.ToString();
                    specmax = dtvspec.Rows[irow].Cells[5].Value.ToString();
                    nrp = 0;
                    _repeat = 0;
                repeat_test:
                    //result = takedatabyitem(ref DAQVoltage, offsetocv, offsetir ,system, packcode, item_id, ic, ref judgedatatype, ref P, ref k, ref PVM ,DateMes, SNMes,scan, com, daq, dvm, hioki,pwsl, sdiv, lbrepeat );
                    result = takedatabyitem(ref DAQVoltage, offsetocv, offsetir, system, packcode, item_id, ic, ref judgedatatype, ref P, ref k, ref PVM, DateMes, SNMes, scan, com, daq, dvm, hioki, pwsl, sdiv, lbrepeat, kenh, PLC, checkfail, ref _barcodePCM);
                    rawdata = rawdata + result + "\t";

                    lbresult.Font = new Font(FontFamily.GenericSansSerif, 8);
                    lbresult.Text = (string)dtvspec.Rows[irow].Cells[2].Value;
                    lbresult.BackColor = Color.Yellow;
                    lbresult.ForeColor = Color.Blue;
                    switch (Judgement(system, judgedatatype, item_id, specmin, specmax, result, jtype, DateMes, SNMes))
                    {
                        case true:
                            if(tabSelected == 1)
                                hienthidata.insertresult(dtvspec, irow, result, Color.FromName("RoyalBlue"), Color.White, kenh);
                            break;
                        case false:
                            if (nrp < _repeat)
                            {
                                nrp++;
                                goto repeat_test;
                            }
                            if(tabSelected == 1)
                                hienthidata.insertresult(dtvspec, irow, result, Color.Red, Color.White, kenh);
                            _error_flag += 1;
                            break;
                    }
                    if (_stopflag)
                    {
                        //thêm hạng mục tắt pwsl khi stop
                        //thêm hạng mục tắt pwsl khi stop
                        if (kenh == 1)
                        {
                            //_kqtk1 = 2;
                            if (model.Text == "BL1815N_5S1P_15M")
                            {
                                PLC.Writeplc("Y20", 1);
                                PLC.Writeplc("M5", 1);
                            }
                            else
                            {
                                //PLC.Writeplc("Y2", 0);
                                PLC.Writeplc("M5", 1);
                                //Thread.Sleep(200);
                                PLC.Writeplc("Y20", 1);

                            }

                        }
                        else if (kenh == 2)
                        {
                            //_kqtk2 = 2;
                            if (model.Text == "BL1815N_5S1P_15M")
                            {
                                PLC.Writeplc("Y21", 1);
                                PLC.Writeplc("M6", 1);
                            }
                            else
                            {
                                //PLC.Writeplc("Y3", 0);
                                Thread.Sleep(200);
                                PLC.Writeplc("Y21", 1);
                                PLC.Writeplc("M6", 1);
                            }

                        }
                        else
                        {
                            if (model.Text == "BL1815N_5S1P_15M")
                            {
                                PLC.Writeplc("Y22", 1);
                                PLC.Writeplc("M7", 1);
                            }
                            else
                            {
                                // PLC.Writeplc("Y4", 0);
                                // Thread.Sleep(200);
                                PLC.Writeplc("Y22", 1);
                                PLC.Writeplc("M7", 1);
                            }
                        }
                        pwsl.PWS_OUTPUT(0, 0, "OFF");
                        break;
                    }
                    if (checkfail && _error_flag != 0)
                    {
                        //thêm hạng mục tắt pwsl khi NG
                        if (kenh == 1)
                        {
                            //_kqtk1 = 2;
                            if (model.Text == "BL1815N_5S1P_15M")
                            {
                                PLC.Writeplc("Y20", 1);
                                PLC.Writeplc("M5", 1);
                            }
                            else
                            {
                                //PLC.Writeplc("Y2", 0);
                                //Thread.Sleep(200);
                                PLC.Writeplc("Y20", 1);
                                PLC.Writeplc("M5", 1);
                            }
                        }
                        else if (kenh == 2)
                        {
                            // _kqtk2 = 2;
                            if (model.Text == "BL1815N_5S1P_15M")
                            {
                                PLC.Writeplc("Y21", 1);
                                PLC.Writeplc("M6", 1);
                            }
                            else
                            {
                                //PLC.Writeplc("Y3", 0);
                                //Thread.Sleep(200);
                                PLC.Writeplc("Y21", 1);
                                PLC.Writeplc("M6", 1);
                            }
                        }
                        else
                        {
                            if (model.Text == "BL1815N_5S1P_15M")
                            {
                                PLC.Writeplc("Y22", 1);
                                PLC.Writeplc("M7", 1);
                            }
                            else
                            {
                                //PLC.Writeplc("Y4", 0);
                                //Thread.Sleep(200);
                                PLC.Writeplc("Y22", 1);
                                PLC.Writeplc("M7", 1);
                            }
                        }
                        pwsl.PWS_OUTPUT(0, 0, "OFF");
                        break;
                    }

                }
                if (system) //USING MES SYSTEM
                {
                    // frm.socket1.Packcode = packcode;
                    if (_error_flag == 0) //PASS
                    {
                        switch (_ProgramProcess)
                        {
                            case "CP-PT":
                                //frm.socket1.Send_Result_PT(kenh, true, dtvspec);
                                break;
                            case "CP-NPC":
                                //frm.socket1.Send_Result_NPC(kenh, tru)
                                break;
                            case "OBA":
                                break;
                        }
                        _ketqua = ketquatest.OK;
                        rawdata = rawdata + "OK\t";
                    }
                    else  //FAIL
                    {
                        lbresult.Font = new Font(FontFamily.GenericSansSerif, 16);
                        lbresult.Text = "NG";
                        lbresult.BackColor = Color.Red;
                        lbresult.ForeColor = Color.Yellow;
                        soundplay(false);
                        switch (_ProgramProcess)
                        {
                            case "CP-PT":

                                break;
                        }
                        _ketqua = ketquatest.NG;
                        rawdata = rawdata + "NG\t";
                        _testflag = false;
                        _stopflag = false;
                    }
                    makelog.saverawdatasystem(packcode, rawdata, kenh);
                }
                else //MASTER TEST
                {
                    if (_error_flag == 0)
                    {
                        lbresult.Font = new Font(FontFamily.GenericSansSerif, 16);
                        lbresult.Text = "OK";
                        lbresult.BackColor = Color.Blue;
                        lbresult.ForeColor = Color.Yellow;
                        rawdata = rawdata + "OK\t" + DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy\t");
                        lblOK.Text = (int.Parse(lblOK.Text) + 1).ToString();
                        //thêm hạng mục hiển thị tỉ lệ 04.03
                        if (lblOK.Text == "0" && lblNG.Text == "0")
                        {
                            tile.Text = "0";
                        }
                        else
                        {
                            tile.Text = ((float.Parse(lblNG.Text) / (float.Parse(lblOK.Text) + float.Parse(lblNG.Text))) * 100).ToString("00.00");
                        }

                        soundplay(true);
                        //thêm hạng mục tự động nhấc jig khi test OK(09/03)
                        //com.wakeup(4);
                        //Thread.Sleep(1000);
                        //com.Offcharge();
                        if (kenh == 1)
                        {
                            //_kqtk1 = 1;
                            if (model.Text == "BL1815N_5S1P_15M")
                            {
                                PLC.Writeplc("Y14", 1);
                                PLC.Writeplc("M2", 1);
                            }
                            else
                            {
                                //PLC.Writeplc("Y2", 0);
                                //Thread.Sleep(200);
                                PLC.Writeplc("Y14", 1);
                                PLC.Writeplc("M2", 1);
                            }

                        }
                        else if (kenh == 2)
                        {
                            //_kqtk2 = 1;
                            if (model.Text == "BL1815N_5S1P_15M")
                            {
                                PLC.Writeplc("Y15", 1);
                                PLC.Writeplc("M3", 1);
                            }
                            else
                            {
                                //PLC.Writeplc("Y3", 0);
                                //Thread.Sleep(200);
                                PLC.Writeplc("Y15", 1);
                                PLC.Writeplc("M3", 1);
                            }

                        }
                        else
                        {
                            if (model.Text == "BL1815N_5S1P_15M")
                            {
                                PLC.Writeplc("Y16", 1);
                                PLC.Writeplc("M4", 1);
                            }
                            else
                            {
                                //PLC.Writeplc("Y4", 0);
                                //Thread.Sleep(200);
                                PLC.Writeplc("Y16", 1);
                                PLC.Writeplc("M4", 1);
                            }
                        }
                        _ktrkq += 1;
                    }
                    else
                    {
                        lbresult.Font = new Font(FontFamily.GenericSansSerif, 16);
                        //lbresult.Text = "NG";
                        lbresult.BackColor = Color.Red;
                        lbresult.ForeColor = Color.Yellow;
                        rawdata = rawdata + "NG\t" + DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy\t");
                        lblNG.Text = (int.Parse(lblNG.Text) + 1).ToString();
                        //thêm hạng mục hiển thị tỉ lệ NG, thêm hạng mục tắt PWSL(04.03) 

                        if (lblOK.Text == "0" && lblNG.Text == "0")
                        {
                            tile.Text = "0";
                        }
                        else
                        {
                            tile.Text = ((float.Parse(lblNG.Text) / (float.Parse(lblOK.Text) + float.Parse(lblNG.Text))) * 100).ToString("00.00");
                        }

                        soundplay(false);
                        //thêm hạng mục bấm nút nhấc jig khi NG(09/03)
                        if (kenh == 1)
                        {
                            //_kqtk1 = 2;
                            if (model.Text == "BL1815N_5S1P_15M")
                            {
                                PLC.Writeplc("Y20", 1);
                                PLC.Writeplc("M5", 1);
                            }
                            else
                            {
                                //PLC.Writeplc("Y2", 0);
                                //Thread.Sleep(200);
                                PLC.Writeplc("Y20", 1);
                                PLC.Writeplc("M5", 1);
                            }

                        }
                        else if (kenh == 2)
                        {
                            // _kqtk2 = 2;
                            if (model.Text == "BL1815N_5S1P_15M")
                            {
                                PLC.Writeplc("Y21", 1);
                                PLC.Writeplc("M6", 1);
                            }
                            else
                            {
                                //PLC.Writeplc("Y3", 0);
                                //Thread.Sleep(200);
                                PLC.Writeplc("Y21", 1);
                                PLC.Writeplc("M6", 1);
                            }

                        }
                        else
                        {
                            if (model.Text == "BL1815N_5S1P_15M")
                            {
                                PLC.Writeplc("Y22", 1);
                                PLC.Writeplc("M7", 1);
                            }
                            else
                            {
                                //PLC.Writeplc("Y4", 0);
                                //Thread.Sleep(200);
                                PLC.Writeplc("Y22", 1);
                                PLC.Writeplc("M7", 1);
                            }
                        }
                        _ktrkq += 1;
                        makelog.saverawdatalocal_NG(packcode, rawdata, kenh, count);
                    }
                    switch (_ProgramProcess)
                    {

                        case "CP-PT":
                            break;
                    }

                    makelog.saverawdatalocal(packcode, rawdata, kenh, count);
                    _testflag = false;
                    _stopflag = false;

                }
            }

        }
        public void soundplay(bool OKsignal)
        {
            System.Media.SoundPlayer SP = new System.Media.SoundPlayer();
            switch (OKsignal)
            {
                case true:
                    SP.SoundLocation = Application.StartupPath + @"\Good.wav";
                    SP.Play();
                    break;
                case false:
                    SP.SoundLocation = Application.StartupPath + @"\NG.wav";
                    SP.Play();
                    break;
            }

        }

        private string convert(string data, string convert_type)
        {
            /*
             * Chuyển đổi dữ liệu trước khi đánh giá
             */
            switch (convert_type)
            {
                case "NO":
                    return data;
                case "C": //temperature
                    return Convert.ToString(dataconvert.H2D(data.Substring(2, 2) + data.Substring(0, 2)) / 10 - 273);
                case "PCMDATE":
                    return dataconvert.PCMDate(data);
                case "PACKDATE":
                    return dataconvert.PackDate(data);
                case "HEX2DEC":
                    return dataconvert.H2D(data).ToString();
                case "HEX2BIN":
                    return dataconvert.H2B(data);
                case "LLHH":
                    return dataconvert.HHLL_LLHH(data);
                case "ASCII":
                    return dataconvert.hex2str(data);
                case "LLHH_HEX":
                    return data.Substring(2, 2) + data.Substring(0, 2);
                default:
                    return "";
            }
        }

        private bool Judgement(bool system, string judgedatatype, int item_id, string specmin,
                        string specmax, string result, string jtype, string DateMES, string SNMes)
        {
            /*
             * Đánh giá kết quả
             */

            try
            {
                if (specmin.Length > 8 && specmin.Substring(1, 5) == "TODAY")
                {
                    specmin = "-" + specmin.Substring(8, specmin.Length - 8).Trim();
                }
                if (specmax.Length > 8 && specmax.Substring(1, 5) == "TODAY")
                {
                    specmax = "-" + specmax.Substring(8, specmax.Length - 8).Trim();
                }
                if (result == "NAK" || result == "FAIL")
                {
                    return false;
                }
                else if (result == "SKIP")
                {
                    return true;
                }
                else
                {
                    switch (jtype)
                    {
                        case "N":
                            return true;
                        case "<":
                            switch (judgedatatype)
                            {
                                case "BARCODE":
                                    if (result.Length <= int.Parse(specmin)) return true;
                                    break;
                                case "NUM":
                                    if (int.Parse(result) <= int.Parse(specmin)) return true;
                                    break;
                                case "HEX":
                                    if (result == specmin || result == specmax) return true;
                                    break;
                                case "DATE":
                                    //if (dataconvert.comparedate(result) < int.Parse(specmin)) return true;
                                    break;
                                case "COMMAND":
                                    if (result == "PASS") return true;
                                    break;
                                case "STR":
                                    if (result == specmin || result == specmax) return true;
                                    break;
                                case "NONE":
                                    return true;
                                case "DATE_MES":
                                    return true;
                                case "SN_MES":
                                    return true;
                                case "CONF_DATE":
                                    if (result == DateMES) return true;
                                    break;
                                case "CONF_SN":
                                    if (result == SNMes) return true;
                                    break;
                                case "ADC":
                                    string[] a = result.Split('-');
                                    int b = 0;
                                    for (int i = 0; i < a.Length; i++)
                                    {


                                        if (int.Parse(a[i]) > int.Parse(specmin))
                                        {
                                            b = b + 1;
                                        }
                                    }
                                    if (b == 0)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                    break;
                                //default:
                                //    break;
                            }
                            break;
                        case ">":
                            switch (judgedatatype)
                            {
                                case "BARCODE":
                                    if (result.Length >= int.Parse(specmax)) return true;
                                    break;
                                case "NUM":
                                    if (double.Parse(result) >= int.Parse(specmin)) return true;

                                    break;
                                case "HEX":
                                    if (result == specmin || result == specmax) return true;
                                    break;
                                case "DATE":
                                    //if (dataconvert.comparedate(result) > int.Parse(specmin)) return true;
                                    break;
                                case "COMMAND":
                                    if (result == "PASS") return true;
                                    break;
                                case "STR":
                                    if (result == specmin || result == specmax) return true;
                                    break;
                                case "NONE":
                                    return true;
                                case "DATE_MES":
                                    return true;
                                case "SN_MES":
                                    return true;
                                case "CONF_DATE":
                                    if (result == DateMES) return true;
                                    break;
                                case "CONF_SN":
                                    if (result == SNMes) return true;
                                    break;
                                case "ADC":

                                    string[] a = result.Split('-');
                                    int b = 0;
                                    for (int i = 0; i < a.Length; i++)
                                    {
                                        if (int.Parse(a[i]) < int.Parse(specmin))
                                        {
                                            b = b + 1;
                                        }

                                    }
                                    if (b == 0)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                //break;
                            }
                            break;
                        case "=":
                            switch (judgedatatype)
                            {
                                case "BARCODE":
                                    if (result.Length == int.Parse(specmin) || result.Length == int.Parse(specmax)) return true;
                                    break;
                                case "NUM":
                                    if (int.Parse(result) == int.Parse(specmin) || int.Parse(result) == int.Parse(specmax)) return true;
                                    break;
                                case "HEX":
                                    if (result == specmin || result == specmax) return true;
                                    break;
                                case "DATE":
                                    if (dataconvert.comparedate(result) == int.Parse(specmin) || dataconvert.comparedate(result) == int.Parse(specmax)) return true;
                                    break;
                                case "COMMAND":
                                    if (result == "PASS")
                                    {
                                        return true;
                                    }
                                    else if (result != "NAK" && result != "" && result != "Duplicate" && result != "LED Contact NG" && result != "LED NG")
                                    {
                                        return true;
                                    }
                                    break;
                                case "STR":
                                    if (result == specmin || result == specmax) return true;
                                    break;
                                case "NONE":
                                    return true;
                                case "DATE_MES":
                                    return true;
                                case "SN_MES":
                                    return true;
                                case "CONF_DATE":
                                    if (result == DateMES) return true;
                                    break;
                                case "CONF_SN":
                                    if (result == SNMes) return true;
                                    break;
                            }
                            break;
                        case "B":
                            switch (judgedatatype)
                            {
                                case "BARCODE":
                                    if (result.Length >= int.Parse(specmin) && result.Length <= int.Parse(specmax)) return true;
                                    break;
                                case "NUM":
                                    if (Convert.ToDouble(result) >= Convert.ToDouble(specmin) && Convert.ToDouble(result) <= Convert.ToDouble(specmax)) return true;
                                    break;
                                case "HEX":
                                    //if (result == specmin || result == specmax) return true;
                                    if (int.Parse(dataconvert.H2D(result).ToString()) >= int.Parse(dataconvert.H2D(specmin).ToString()) && int.Parse(dataconvert.H2D(result).ToString()) <= int.Parse(dataconvert.H2D(specmax).ToString())) return true;
                                    break;
                                case "DATE":
                                    if (dataconvert.comparedate(result) >= int.Parse(specmin) && dataconvert.comparedate(result) <= int.Parse(specmax)) return true;
                                    break;
                                case "COMMAND":
                                    if (result == "PASS") return true;
                                    break;
                                case "STR":
                                    if (result == specmin || result == specmax) return true;
                                    break;
                                case "NONE":
                                    return true;
                                case "DATE_MES":
                                    return true;
                                case "SN_MES":
                                    return true;
                                case "CONF_DATE":
                                    if (result == DateMES) return true;

                                    break;
                                case "CONF_SN":
                                    if (result == SNMes) return true;
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private string takedatabyitem(ref string[] DAQVoltage, float offsetocv, int offsetir, bool system, string packcode, int item_id, string ic, ref string datatype,
                ref string[] P, ref int k, ref int PVM, string DateMES, string SNMes,
                clsScanner scan, clscomboard com, clsDAQ daq, clsDVM dvm, clsHioki hioki, clsPWS pwsl, clsSDIVBoard sdiv, ToolStripLabel lbrepeat, int kenh, clsPLC PLC, bool checkNG, ref string barcodePCM)
        {
            /*
             * Item Array: 0: 
             * item_id, 1: 
             * seq, 2: 
             * scpi, 3: 
             * cmd, 4: 
             * addtitemcommand.Rows[dong], 5: 
             * len, 6: 
             * data, 7: 
             * conv, 8: 
             * storage, 9: 
             * ic
             */
            DataTable dtitemcommand = new DataTable();
            string Result = "";
            string[] V = new string[100];
            int itemseq = 1;
            string DateMESHex = "";
            string SNMESHex = "";
            int nrepeat = 1;

            dtitemcommand = Localdb.loaditemcommand(ic, item_id);

            if (system == true && DateMES != "" && SNMes != "")
            {
                DateMESHex = dataconvert.date2hex(DateMES);
                SNMESHex = dataconvert.hex(int.Parse(SNMes));
            }
            int rpc = 0;
            do
            {

                if (rpc > 0) { Thread.Sleep(1000); }
                rpc++;
                for (int dong = 0; dong < dtitemcommand.Rows.Count; dong++)
                {
                    string seq = dtitemcommand.Rows[dong].ItemArray[1].ToString();
                    string scpi = dtitemcommand.Rows[dong].ItemArray[2].ToString();
                    string cmd = dtitemcommand.Rows[dong].ItemArray[3].ToString();
                    string addr = dtitemcommand.Rows[dong].ItemArray[4].ToString();
                    string len = dtitemcommand.Rows[dong].ItemArray[5].ToString();
                    string data = dtitemcommand.Rows[dong].ItemArray[6].ToString();
                    string converttype = dtitemcommand.Rows[dong].ItemArray[7].ToString();
                    string storage = dtitemcommand.Rows[dong].ItemArray[8].ToString();

                    try
                    {
                        switch (scpi)
                        {
                            case "JUDGEMENT_TYPE":
                                switch (data)
                                {
                                    case "BARCODE":
                                        datatype = "BARCODE";
                                        break;
                                    case "NUM":
                                        datatype = "NUM";
                                        break;
                                    case "HEX":
                                        datatype = "HEX";
                                        break;
                                    case "DATE":
                                        datatype = "DATE";
                                        break;
                                    case "COMMAND":
                                        datatype = "COMMAND";
                                        break;
                                    case "STR":
                                        datatype = "STR";
                                        break;
                                    case "NONE":
                                        datatype = "NONE";
                                        break;
                                    case "DATE_MES":
                                        datatype = "DATE_MES";
                                        break;
                                    case "SN_MES":
                                        datatype = "SN_MES";
                                        break;
                                    case "CONF_DATE":
                                        datatype = "CONF_DATE";
                                        break;
                                    case "CONF_SN":
                                        datatype = "CONF_SN";
                                        break;
                                    case "SADC":
                                        datatype = "SADC";
                                        break;
                                    default:
                                        datatype = data;
                                        break;
                                }
                                break;

                            case "BARCODE_SCAN":

                                string code = "";
                                code = scan.Read_Scaner();
                                Result = dataconvert.removeline(code);
                                V[itemseq] = Result;
                                barcodePCM = Result;

                                if (dtitemcommand.Rows[dong].ItemArray[8].ToString() == "PUSH")
                                {
                                    P[k] = Result;
                                    k += 1;
                                }
                                //switch (data)
                                //{

                                //    case "CP":
                                //        //if (system)
                                //        //{
                                //        //Result = dataconvert.removeline(cpbarcode.Text);
                                //        // }
                                //        //else
                                //        //{
                                //        // Result = "SKIP";
                                //        //}
                                //        if (Result != "")
                                //        {
                                //            //Result = dataconvert.removeline(cpbarcode.Text);
                                //        }
                                //        else
                                //        {
                                //            Result = "SKIP";
                                //        }
                                //        break;
                                //    case "HP":
                                //        if (system)
                                //        {
                                //           // Result = dataconvert.removeline(hpbarcode.Text);
                                //        }
                                //        else
                                //        {
                                //            Result = "SKIP";
                                //        }
                                //        break;
                                //}
                                break;

                            case "DAQ"://thêm hạng mục check điện áp bằng DAQ(08.03)
                                switch (data)
                                {
                                    case "RIS":
                                        if (len.Substring(0, 1) == "@")
                                        {
                                            Result = (float.Parse(DAQVoltage[int.Parse(len.Substring(1, 1))])).ToString("000.0");
                                            V[itemseq] = Result;
                                            if (dtitemcommand.Rows[dong].ItemArray[8].ToString() == "PUSH")
                                            {
                                                P[k] = Result;
                                                k += 1;
                                            }
                                        }
                                        else
                                        {
                                            daq.setupkenhdoresistor(len.Substring(3, len.Length - 4), ref DAQVoltage, offsetocv);
                                        }
                                        itemseq += 1;
                                        break;
                                    case "VOL":
                                        if (len.Substring(0, 1) == "@")
                                        {
                                            if (cmd == "YES")
                                            {
                                                Result = (float.Parse(DAQVoltage[int.Parse(len.Substring(1, 1))]) + offsetocv).ToString("000.0");
                                                V[itemseq] = Result;
                                                if (dtitemcommand.Rows[dong].ItemArray[8].ToString() == "PUSH")
                                                {
                                                    P[k] = Result;
                                                    k += 1;
                                                }
                                            }
                                            else
                                            {
                                                Result = (float.Parse(DAQVoltage[int.Parse(len.Substring(1, 1))])).ToString("000.0");
                                                V[itemseq] = Result;
                                                if (dtitemcommand.Rows[dong].ItemArray[8].ToString() == "PUSH")
                                                {
                                                    P[k] = Result;
                                                    k += 1;
                                                }
                                            }

                                        }
                                        else
                                        {
                                            daq.setupkenhdoDAQ(len.Substring(3, len.Length - 4), ref DAQVoltage, offsetocv);
                                        }
                                        itemseq += 1;
                                        break;
                                    case "TEMP":
                                        if (len.Substring(0, 1) == "@")
                                        {
                                            Result = (float.Parse(DAQVoltage[int.Parse(len.Substring(1, 1))])).ToString("000.0");
                                            V[itemseq] = Result;
                                            if (dtitemcommand.Rows[dong].ItemArray[8].ToString() == "PUSH")
                                            {
                                                P[k] = Result;
                                                k += 1;
                                            }
                                        }
                                        else
                                        {
                                            daq.setupkenhdoTEMP(len.Substring(3, len.Length - 4), ref DAQVoltage, offsetocv);
                                        }
                                        itemseq += 1;
                                        break;
                                    case "SPCL":
                                        daq.setupkenhdoDAQ("@110,111,112,113", ref DAQVoltage, 0);
                                        if (float.Parse(DAQVoltage[1]) >= int.Parse(addr) && float.Parse(DAQVoltage[2]) > int.Parse(addr) && float.Parse(DAQVoltage[3]) > int.Parse(addr) && float.Parse(DAQVoltage[4]) > int.Parse(addr))
                                        {
                                            Result = "PASS";
                                        }
                                        else
                                        {
                                            Result = "LED Contact NG";
                                        }
                                        break;
                                }
                                break;
                            case "DVM":
                                Result = (float.Parse(convert(dvm.dodienap().ToString("00000.0"), converttype)) + offsetocv).ToString();
                                V[itemseq] = Result;
                                if (dtitemcommand.Rows[dong].ItemArray[8].ToString() == "PUSH")
                                {
                                    P[k] = Result;
                                    k += 1;
                                }
                                itemseq += 1;
                                break;

                            case "PLC":
                                // Result = "PASS";
                                switch (data)
                                {
                                    case "READ":
                                        if (kenh == 1)
                                        {
                                            //PLC.readplc();
                                        }
                                        else
                                        {
                                            //PLC.readplc();
                                        }
                                        break;
                                    case "WRITE":
                                        if (kenh == 1)
                                        {
                                            PLC.Writeplc(addr, int.Parse(len));
                                        }
                                        else if (kenh == 2)
                                        {
                                            if (addr.Length == 2)
                                            {
                                                PLC.Writeplc(addr.Substring(0, 1) + (int.Parse(addr.Substring(1, 1)) + 1), int.Parse(len));
                                            }
                                            else
                                            {
                                                PLC.Writeplc(addr.Substring(0, 1) + (int.Parse(addr.Substring(1, 2)) + 1), int.Parse(len));
                                            }

                                        }
                                        else
                                        {
                                            if (addr.Length == 2)
                                            {
                                                PLC.Writeplc(addr.Substring(0, 1) + (int.Parse(addr.Substring(1, 1)) + 2), int.Parse(len));
                                            }
                                            else
                                            {
                                                PLC.Writeplc(addr.Substring(0, 1) + (int.Parse(addr.Substring(1, 2)) + 2), int.Parse(len));
                                            }
                                        }
                                        break;
                                }
                                break;

                            case "POWER_SUPPLY":
                                Result = "PASS";
                                if (addr == "") { addr = "0"; }
                                if (cmd == "") { cmd = "0"; }
                                if (int.Parse(len) == kenh)
                                {
                                    pwsl.PWS_OUTPUT(double.Parse(addr.ToString()), double.Parse(cmd.ToString()), data);
                                }
                                break;

                            case "HIOKI":
                                switch (data)
                                {
                                    case "VOLTAGE":
                                        Result = (float.Parse(hioki.dodienap().ToString("00000.0")) + offsetocv).ToString();
                                        V[itemseq] = Result;
                                        if (dtitemcommand.Rows[dong].ItemArray[8].ToString() == "PUSH")
                                        {
                                            P[k] = Result;
                                            k += 1;
                                        }
                                        itemseq += 1;
                                        break;
                                    case "IR":
                                        Result = (float.Parse(hioki.dodientro().ToString("000.0")) + offsetir).ToString();
                                        V[itemseq] = Result;
                                        if (dtitemcommand.Rows[dong].ItemArray[8].ToString() == "PUSH")
                                        {
                                            P[k] = Result;
                                            k += 1;
                                        }
                                        itemseq += 1;
                                        break;
                                }
                                break;

                            case "LED":
                                Result = "NAK";
                                Result = sdiv.docketqua();
                                int count = 0;
                                do
                                {
                                    count++;
                                } while (Result == "NAK" && count == 1000);
                                V[itemseq] = Result;
                                if (dtitemcommand.Rows[dong].ItemArray[8].ToString() == "PUSH")
                                {
                                    P[k] = Result;
                                    k += 1;
                                }
                                itemseq += 1;
                                break;

                            case "ADC":
                                string a = sdiv.docADCtest();
                                if (cmd == "N")
                                {
                                    if (data == "S")
                                    {
                                        string[] b = a.Split('-');
                                        for (int i = 0; i < b.Length; i++)
                                        {
                                            if (int.Parse(b[i]) > int.Parse(len))
                                            {
                                                b[i] = (int.Parse(b[i]) - int.Parse(len)).ToString();
                                            }
                                        }
                                        Result = (int.Parse(b[0])).ToString() + "-" + (int.Parse(b[1])).ToString() + "-" + (int.Parse(b[2])).ToString() + "-" + (int.Parse(b[3])).ToString();
                                    }
                                    else
                                    {
                                        Result = a;
                                    }

                                    V[itemseq] = Result;
                                    if (dtitemcommand.Rows[dong].ItemArray[8].ToString() == "PUSH")
                                    {
                                        P[k] = Result;
                                        k += 1;
                                    }
                                    itemseq += 1;
                                }
                                else
                                {
                                    if (int.Parse(a.Substring(1, 3)) >= int.Parse(len) || int.Parse(a.Substring(5, 3)) >= int.Parse(len) || int.Parse(a.Substring(9, 3)) >= int.Parse(len) || int.Parse(a.Substring(13, 3)) >= int.Parse(len))
                                    {
                                        Result = "LED NG";
                                    }
                                    else
                                    {
                                        Result = "PASS";
                                    }

                                }

                                break;

                            case "REPEAT":
                                nrepeat += 1;
                                lbrepeat.Text = nrepeat.ToString();
                                if (nrepeat != int.Parse(data))
                                {
                                    dong = 0;
                                }
                                break;
                            case "TIME CHECK":
                                if (_timecheck_flag == false)
                                {

                                }
                                break;
                            case "NREPEAT":
                                _repeat = int.Parse(data);
                                break;
                            case "WAKEUP":    //thay đổi tín hiệu điều khiển comboard       
                                Result = "PASS";
                                switch (data)
                                {
                                    case "ON":
                                        com.wakeup(int.Parse(len));
                                        break;
                                    case "OFF":
                                        com.Offcharge();
                                        break;
                                }
                                break;

                            case "1W-RB?":
                                Result = convert(com.ReadBlock_1W(cmd, addr, len), converttype);
                                V[itemseq] = Result;
                                if (dtitemcommand.Rows[dong].ItemArray[8].ToString() == "PUSH")
                                {
                                    P[k] = Result;
                                    k += 1;
                                }
                                itemseq += 1;
                                break;

                            case "1W-WC?":
                                if (com.WriteCommand_1W(cmd, addr, len))
                                {
                                    Result = "PASS";
                                }
                                else
                                {
                                    Result = "NAK";
                                }
                                break;

                            case "1W-WB?":
                                if (com.WriteBlock_1W(cmd, addr, len, data))
                                {
                                    Result = "PASS";
                                }
                                else
                                {
                                    Result = "NAK";
                                }
                                break;
                            case "1W-WW?":
                                if (com.WriteBlock_WW(cmd, addr, len, data))
                                {
                                    Result = "PASS";
                                }
                                else
                                {
                                    Result = "NAK";
                                }
                                break;

                            case "SMB-RW?":
                                Result = convert(com.ReadWordSMB(cmd), converttype);
                                V[itemseq] = Result;
                                if (dtitemcommand.Rows[dong].ItemArray[8].ToString() == "PUSH")
                                {
                                    P[k] = Result;
                                    k += 1;
                                }
                                itemseq += 1;
                                break;

                            case "SMB-WW?":
                                switch (data)
                                {
                                    case "":
                                        if (com.WriteWord_SMB(cmd, addr))
                                        {
                                            Result = "PASS";
                                        }
                                        else
                                        {
                                            Result = "NAK";
                                        }
                                        break;
                                    case "@DATE_L":
                                        if (system)
                                        {
                                            if (com.WriteWord_SMB(cmd, DateMESHex.Substring(2, 2))) Result = "PASS";
                                            else Result = "NAK";
                                        }
                                        else Result = "SKIP";
                                        break;
                                    case "@DATE_H":
                                        if (system)
                                        {
                                            if (com.WriteWord_SMB(cmd, DateMESHex.Substring(0, 2))) Result = "PASS";
                                            else Result = "NAK";
                                        }
                                        else Result = "SKIP";
                                        break;
                                    case "@DATE":
                                        if (system)
                                        {
                                            if (com.WriteWord_SMB(cmd, DateMESHex)) Result = "PASS";
                                            else Result = "NAK";
                                        }
                                        else Result = "SKIP";
                                        break;
                                    case "@SN_L":
                                        if (system)
                                        {
                                            if (com.WriteWord_SMB(cmd, SNMESHex.Substring(2, 2))) Result = "PASS";
                                            else Result = "NAK";
                                        }
                                        else Result = "SKIP";
                                        break;
                                    case "@SN_H":
                                        if (system)
                                        {
                                            if (com.WriteWord_SMB(cmd, SNMESHex.Substring(0, 2))) Result = "PASS";
                                            else Result = "NAK";
                                        }
                                        else Result = "SKIP";
                                        break;
                                    case "@SN":
                                        if (system)
                                        {
                                            if (com.WriteWord_SMB(cmd, SNMESHex)) Result = "PASS";
                                            else Result = "NAK";
                                        }
                                        else Result = "SKIP";
                                        break;
                                }
                                break;

                            case "SMB-RB?":
                                Result = convert(com.ReadBlockSMB(cmd), converttype);
                                V[itemseq] = Result;
                                if (dtitemcommand.Rows[dong].ItemArray[8].ToString() == "PUSH")
                                {
                                    P[k] = Result;
                                    k += 1;
                                }
                                itemseq += 1;
                                break;

                            case "SMB-WB?":
                                if (com.WriteBlock_SMB(cmd, addr))
                                {
                                    Result = "PASS";
                                }
                                else
                                {
                                    Result = "NAK";
                                }
                                break;

                            case "CALL":
                                switch (data)
                                {
                                    case "CLR":
                                        for (k = 1; k < 100; k++)
                                        {
                                            P[k] = "";
                                        }
                                        k = 1;
                                        break;
                                    case "MID":
                                        Result = convert(V[int.Parse(cmd)].Substring(int.Parse(addr) - 1, int.Parse(len)), converttype);
                                        V[itemseq] = Result;
                                        if (dtitemcommand.Rows[dong].ItemArray[8].ToString() == "PUSH")
                                        {
                                            P[k] = Result;
                                            k += 1;
                                        }
                                        break;
                                    case "MAX":
                                        long max = int.Parse(V[1]);
                                        for (int i = 2; i < itemseq; i++)
                                        {
                                            if (int.Parse(V[i]) > max) max = int.Parse(V[i]);
                                        }
                                        Result = convert(max.ToString(), converttype);
                                        V[itemseq] = Result;
                                        if (dtitemcommand.Rows[dong].ItemArray[8].ToString() == "PUSH")
                                        {
                                            P[k] = Result;
                                            k += 1;
                                        }
                                        break;
                                    case "MIN":
                                        long min = int.Parse(V[1]);
                                        for (int i = 2; i < itemseq; i++)
                                        {
                                            if (int.Parse(V[i]) < min) min = int.Parse(V[i]);
                                        }
                                        Result = convert(min.ToString(), converttype);
                                        V[itemseq] = Result;
                                        if (dtitemcommand.Rows[dong].ItemArray[8].ToString() == "PUSH")
                                        {
                                            P[k] = Result;
                                            k += 1;
                                        }
                                        break;
                                    case "PVM":
                                        Result = convert(PVM.ToString(), converttype);
                                        V[itemseq] = Result;
                                        if (dtitemcommand.Rows[dong].ItemArray[8].ToString() == "PUSH")
                                        {
                                            P[k] = Result;
                                            k += 1;
                                        }
                                        break;
                                    default:
                                        if (data.Substring(0, 1) == "P")
                                        {
                                            Result = convert(P[int.Parse(data.Substring(1, data.Length - 1))], converttype);
                                            V[itemseq] = Result;
                                            if (dtitemcommand.Rows[dong].ItemArray[8].ToString() == "PUSH")
                                            {
                                                P[k] = Result;
                                                k += 1;
                                            }
                                            break;
                                        }
                                        break;
                                }
                                itemseq += 1;
                                break;

                            case "MSG":
                                MessageBox.Show(data, "Thông Báo");
                                break;

                            case "MSG Y/N?":
                                if (MessageBox.Show(data, "Thông Báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    Result = "PASS";
                                }
                                else
                                {
                                    Result = "FAIL";
                                }
                                break;

                            case "DELAY":
                                Thread.Sleep(int.Parse(data));
                                break;

                            case "CALC":
                                if (cmd == "&")
                                {
                                    string Date_SN = barcodePCM + "-" + V[1] + "-" + V[2];
                                    checkdBoxed.LoadList("PCM_INFO1", ref ListBarcodeBoxed1);
                                    checkdBoxed.LoadList("PCM_INFO2", ref ListBarcodeBoxed2);
                                    checkdBoxed.LoadList("PCM_INFO3", ref ListBarcodeBoxed3);
                                    if (checkdBoxed.CheckDuplicateInforamation(Date_SN, ListBarcodeBoxed1) == false || checkdBoxed.CheckDuplicateInforamation(Date_SN, ListBarcodeBoxed2) == false || checkdBoxed.CheckDuplicateInforamation(Date_SN, ListBarcodeBoxed3) == false)
                                    {
                                        Result = "Duplicate";
                                    }
                                    else
                                    {
                                        Result = Date_SN;
                                    }

                                }
                                else if (cmd == "save")
                                {
                                    if (kenh == 1)
                                    {
                                        string Date_SN = barcodePCM + "-" + V[3] + "-" + V[4];
                                        checkdBoxed.SaveList(Date_SN, "PCM_INFO1");
                                        checkdBoxed.SaveList(Date_SN, "PCM_INFO1.log", @Application.StartupPath + @"\log\Boxing\" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + @"\");
                                        Result = Date_SN;

                                    }
                                    else if (kenh == 2)
                                    {
                                        string Date_SN = barcodePCM + "-" + V[3] + "-" + V[4];
                                        checkdBoxed.SaveList(Date_SN, "PCM_INFO2");
                                        checkdBoxed.SaveList(Date_SN, "PCM_INFO2.log", @Application.StartupPath + @"\log\Boxing\" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + @"\");
                                        Result = Date_SN;
                                    }
                                    else
                                    {
                                        string Date_SN = barcodePCM + "-" + V[3] + "-" + V[4];
                                        checkdBoxed.SaveList(Date_SN, "PCM_INFO3");
                                        checkdBoxed.SaveList(Date_SN, "PCM_INFO3.log", @Application.StartupPath + @"\log\Boxing\" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + @"\");
                                        Result = Date_SN;
                                    }

                                }
                                else
                                {
                                    Result = convert(dataconvert.calc(data, V).ToString(), converttype);
                                    V[itemseq] = Result;
                                }

                                if (dtitemcommand.Rows[dong].ItemArray[8].ToString() == "PUSH")
                                {
                                    P[k] = Result;
                                    k += 1;
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        Result = "NAK";
                    }

                }
                if (rpc == 3)
                { break; }
            } while (Result == "NAK");
            return Result;
        }
    }
}
