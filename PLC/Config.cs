using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace PLC
{
    public class Config
    {
        IniFile iniFile = new IniFile(System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\config.ini");

        public string ServerIP { get { return iniFile.GetString("PLCCommunicator", "IP", "127.0.0.1"); } }

        public int ServerPort { get { return iniFile.GetInt32("PLCCommunicator", "Port", 9980); } }

        public string ReadCommand { get { return iniFile.GetString("PLCCommunicator", "Read", "Read"); } }

        public string InitCommand { get { return iniFile.GetString("PLCCommunicator", "Init", "Init"); } }

        public string WriteCommand { get { return iniFile.GetString("PLCCommunicator", "Write", "Write"); } }
    }
}
