using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Utilities
{
    public class Log
    {
        public enum CATEGORY { MAIN, EVENT, EXCEPTION, SETTING, THREAD, PLC }

        /// <summary>
        /// 시스템 로그
        /// </summary>
        /// <param name="category">로그 카테고리</param>
        /// <param name="log">로그 내용</param>
        public void Write_SystemLog(CATEGORY category, string log)
        {
            FileStream wstream;
            StreamWriter writer;

            Console.WriteLine(DateTime.Now.ToLongTimeString() + "\t" + log);

            try
            {
                StringBuilder path = new StringBuilder(Directory.GetCurrentDirectory());
                path.Append("\\System Log\\" + category + " Log\\");
                //Application.StartupPath + "\\" + log_path + " Log\\";

                if (!Directory.Exists(path.ToString()))
                {
                    Directory.CreateDirectory(path.ToString());
                }

                path.Append(String.Format("{0:0000}년{1:00}월{2:00}일.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));

                if (File.Exists(path.ToString()) == false)
                {
                    wstream = File.Create(path.ToString());
                    writer = new StreamWriter(wstream);
                }
                else
                {
                    wstream = new FileStream(path.ToString(), FileMode.Append, FileAccess.Write, FileShare.Read);
                    writer = new StreamWriter(wstream);
                }

                Object Log_Writing = new object();

                lock (Log_Writing)
                {
                    writer.WriteLine(DateTime.Now.ToLongTimeString() + "\t" + log);
                }

                writer.Close();
                wstream.Close();
            }
            catch (Exception ex)
            {
                //Write_SystemLog(ex.Message);
            }
            finally
            {
                writer = null;
                wstream = null;
            }
        }
    }
}
