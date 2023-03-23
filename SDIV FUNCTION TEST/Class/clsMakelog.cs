using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace SDIV_FUNCTION_TEST
{
    class clsMakelog
    {
        public void saverawdatalocal(string packcode, string data, int kenh,int count)
        {
            string path;
            switch (kenh)
            {
                case 1:
                    path = Application.StartupPath + @"/Log/Local/CH1/" + packcode + DateTime.Now.ToString("_yyyyMMdd") + "_" + count +@".CSV";
                    break;
                case 2:
                    path = Application.StartupPath + @"/Log/Local/CH2/" + packcode + DateTime.Now.ToString("_yyyyMMdd") + "_" + count + @".CSV";
                    break;
                case 3:
                    path = Application.StartupPath + @"/Log/Local/CH3/" + packcode + DateTime.Now.ToString("_yyyyMMdd") + "_" + count + @".CSV";
                    break;
                default:
                    path = Application.StartupPath + @"/Log/Local/CH1/" + packcode + @".CSV";
                    break;
            }
            FileStream fs = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            string firstline = data;
            string[] data1 = firstline.Split('\t');
            sw.WriteLine("");
            for (int irow = 0; irow < data1.Length; irow++)
            {
                sw.Write(data1[irow] + ",");
            }
          
            sw.Flush();
            sw.Close();
            fs.Close();

        }

        public void createlog(string packcode, DataGridView dtvspec,int count)
        {
            string pathlocal1, pathlocal2, pathlocal3;
          
            string data1 = "";
           
            pathlocal1 = Application.StartupPath + @"/Log/Local/CH1/" + packcode + DateTime.Now.ToString("_yyyyMMdd") + "_" + count+ @".CSV";
            pathlocal2 = Application.StartupPath + @"/Log/Local/CH2/" + packcode + DateTime.Now.ToString("_yyyyMMdd") + "_" + count + @".CSV";
            pathlocal3 = Application.StartupPath + @"/Log/Local/CH3/" + packcode + DateTime.Now.ToString("_yyyyMMdd") + "_" + count + @".CSV";


            FileStream fslocal1 = new FileStream(pathlocal1, FileMode.Append);
            FileStream fslocal2 = new FileStream(pathlocal2, FileMode.Append);
            FileStream fslocal3 = new FileStream(pathlocal3, FileMode.Append);


            StreamWriter swlocal1 = new StreamWriter(fslocal1, System.Text.Encoding.UTF8);
            StreamWriter swlocal2 = new StreamWriter(fslocal2, System.Text.Encoding.UTF8);
            StreamWriter swlocal3 = new StreamWriter(fslocal3, System.Text.Encoding.UTF8);


            for (int irow = 0; irow <= (dtvspec.RowCount+1); irow++)
            {
                if (irow < dtvspec.RowCount)
                {
                    data1 = dtvspec.Rows[irow].Cells[2].Value.ToString() + "\t";
                    swlocal1.Write(data1+",");
                    
                    swlocal2.Write(data1+",");
                    
                    swlocal3.Write(data1+",");
                    
                }
                else if (irow == dtvspec.RowCount)
                {
                    data1 = "Ket Qua\t";
                    swlocal1.Write(data1 + ",");
                    swlocal2.Write(data1 + ",");
                    swlocal3.Write(data1 + ",");
                }
                else
                {
                    data1 = "Gio khoi tao: " + DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
                    swlocal1.Write(data1);
                    swlocal2.Write(data1);
                    swlocal3.Write(data1);
                }
                
            }
            swlocal1.WriteLine("");
            swlocal2.WriteLine("");
            swlocal3.WriteLine("");
            
            for (int irow = 0; irow < dtvspec.RowCount-1; irow++)
            {
                data1 =dtvspec.Rows[irow].Cells[4].Value.ToString() + " ~ " + dtvspec.Rows[irow].Cells[5].Value.ToString() + "\t";
                swlocal1.Write(data1 + ",");

                swlocal2.Write(data1 + ",");

                swlocal3.Write(data1 + ",");
                
            }
            swlocal1.WriteLine(data1);
            swlocal2.WriteLine(data1);
            swlocal3.WriteLine(data1);
           
            swlocal1.Flush();
            swlocal1.Close();

            swlocal2.Flush();
            swlocal2.Close();

            swlocal3.Flush();
            swlocal3.Close();

            

            fslocal1.Close();
            fslocal2.Close();
            fslocal3.Close();
            
        }

        public void createlog_NG(string packcode, DataGridView dtvspec,int count)
        {
            string pathlocal1, pathlocal2, pathlocal3;

            string data1 = "";

            pathlocal1 = Application.StartupPath + @"/Log/Local/Log_NG/CH1/" + packcode + DateTime.Now.ToString("_yyyyMMdd") + "_" + count + @".CSV";
            pathlocal2 = Application.StartupPath + @"/Log/Local/Log_NG/CH2/" + packcode + DateTime.Now.ToString("_yyyyMMdd") + "_" + count + @".CSV";
            pathlocal3 = Application.StartupPath + @"/Log/Local/Log_NG/CH3/" + packcode + DateTime.Now.ToString("_yyyyMMdd") + "_" + count + @".CSV";


            FileStream fslocal1 = new FileStream(pathlocal1, FileMode.Append);
            FileStream fslocal2 = new FileStream(pathlocal2, FileMode.Append);
            FileStream fslocal3 = new FileStream(pathlocal3, FileMode.Append);


            StreamWriter swlocal1 = new StreamWriter(fslocal1, System.Text.Encoding.UTF8);
            StreamWriter swlocal2 = new StreamWriter(fslocal2, System.Text.Encoding.UTF8);
            StreamWriter swlocal3 = new StreamWriter(fslocal3, System.Text.Encoding.UTF8);


            for (int irow = 0; irow <= (dtvspec.RowCount+1); irow++)
            {
                if (irow < dtvspec.RowCount)
                {
                    data1 = dtvspec.Rows[irow].Cells[2].Value.ToString() + "\t";
                    swlocal1.Write(data1 + ",");

                    swlocal2.Write(data1 + ",");

                    swlocal3.Write(data1 + ",");

                }
                else if (irow == dtvspec.RowCount)
                {
                    data1 = "Ket Qua";
                    swlocal1.Write(data1 + ",");
                    swlocal2.Write(data1 + ",");
                    swlocal3.Write(data1 + ",");
                }
                else
                {
                    data1 = "Gio khoi tao: " + DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
                    swlocal1.Write(data1);
                    swlocal2.Write(data1);
                    swlocal3.Write(data1);
                }

            }
            swlocal1.WriteLine("");
            swlocal2.WriteLine("");
            swlocal3.WriteLine("");

            for (int irow = 0; irow < dtvspec.RowCount - 1; irow++)
            {
                data1 = dtvspec.Rows[irow].Cells[4].Value.ToString() + " ~ " + dtvspec.Rows[irow].Cells[5].Value.ToString() + "\t";
                swlocal1.Write(data1 + ",");

                swlocal2.Write(data1 + ",");

                swlocal3.Write(data1 + ",");

            }
            swlocal1.WriteLine(data1);
            swlocal2.WriteLine(data1);
            swlocal3.WriteLine(data1);

            swlocal1.Flush();
            swlocal1.Close();

            swlocal2.Flush();
            swlocal2.Close();

            swlocal3.Flush();
            swlocal3.Close();



            fslocal1.Close();
            fslocal2.Close();
            fslocal3.Close();
        }

        public void saverawdatalocal_NG(string packcode, string data, int kenh,int count)
        {
            string path;
            switch (kenh)
            {
                case 1:
                    path = Application.StartupPath + @"/Log/Local/Log_NG/CH1/" + packcode + DateTime.Now.ToString("_yyyyMMdd") + "_" + count + @".CSV";
                    break;
                case 2:
                    path = Application.StartupPath + @"/Log/Local/Log_NG/CH2/" + packcode + DateTime.Now.ToString("_yyyyMMdd") + "_" + count + @".CSV";
                    break;
                case 3:
                    path = Application.StartupPath + @"/Log/Local/Log_NG/CH3/" + packcode + DateTime.Now.ToString("_yyyyMMdd") + "_" + count + @".CSV";
                    break;
                default:
                    path = Application.StartupPath + @"/Log/Local/Log_NG/CH1/" + packcode + @".CSV";
                    break;
            }
            FileStream fs = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            string firstline = data;
            string[] data1 = firstline.Split('\t');
            sw.WriteLine("");
            for (int irow = 0; irow < data1.Length; irow++)
            {
                sw.Write(data1[irow] + ",");
            }
           
            sw.Flush();
            sw.Close();
            fs.Close();
        }


        public void saverawdatasystem(string packcode, string data, int kenh)
        {
            string path;
            switch (kenh)
            {
                case 1:
                    path = Application.StartupPath + @"/Log/Measure/CH1/" + packcode + DateTime.Now.ToString("_yyyyMMdd") + @".log";
                    break;
                case 2:
                    path = Application.StartupPath + @"/Log/Measure/CH2/" + packcode + DateTime.Now.ToString("_yyyyMMdd") + @".log";
                    break;
                case 3:
                    path = Application.StartupPath + @"/Log/Measure/CH3/" + packcode + DateTime.Now.ToString("_yyyyMMdd") + @".log";
                    break;
                default:
                    path = Application.StartupPath + @"/Log/Measure/CH1/" + packcode + DateTime.Now.ToString("_yyyyMMdd") + @".log";
                    break;
            }
            FileStream fs = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

            sw.WriteLine(data);
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        public void savelogsocket(string data,int channels)
        {
            string path;
            if(channels ==1)
            {
                path = Application.StartupPath + @"/Log/Socket/CH1/" + DateTime.Now.ToString("yyyyMMdd") + @".log";
                FileStream fs = new FileStream(path, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                sw.WriteLine(data);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            if(channels ==2)
            {
                path = Application.StartupPath + @"/Log/Socket/CH2/" + DateTime.Now.ToString("yyyyMMdd") + @".log";
                FileStream fs = new FileStream(path, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                sw.WriteLine(data);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
                
               
            
            
        }
    }
}
