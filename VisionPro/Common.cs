using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace Hansero.VisionLib.VisionPro
{
    public class Common
    {
        
        //공용 클래스
        //폴더 생성하는 함수...
        public void Create(string PathName)
        {
            string[] str = PathName.Split('\\');

            if (!System.IO.Directory.Exists(PathName.Substring(0, PathName.Length - str[str.Length - 1].Length)))
            {
                System.IO.Directory.CreateDirectory(PathName.Substring(0, PathName.Length - str[str.Length - 1].Length));
            }
           
        }
       
    }
}
