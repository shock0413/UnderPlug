using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using utill;

namespace PLCCommunicator
{
    public class Config
    {
        public Config()
        {
            
        }

        readonly IniFile iniFile = new IniFile(System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\..\\config.ini");

        public int StationNumber { get { return iniFile.GetInt32("PLCCommunicator", "StationNumber", 1); } }

        public int ServerPort { get { return iniFile.GetInt32("PLCCommunicator", "Port", 9980); } }

        public string ReadCommand { get { return iniFile.GetString("PLCCommunicator", "Read", "Read"); } }


        public string InitCommand { get { return iniFile.GetString("PLCCommunicator", "Init", "Init"); } }

        public string WriteCommand { get { return iniFile.GetString("PLCCommunicator", "Write", "Write"); } }

        public int MessageLimitCount { get { return iniFile.GetInt32("PLCCommunicator", "MessageLimitCount", 30); } }

        public int SaveDays { get { return iniFile.GetInt32("PLCCommunicator", "SaveDays", 7); } }

        public Dictionary<string, string> InputAddress { get { return iniFile.GetSectionValues("Input"); } }
        public Dictionary<string, string> OutputAddress { get { return iniFile.GetSectionValues("Output"); } }
    }
}
