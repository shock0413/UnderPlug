using Hansero;
using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Utilities;

namespace DBManager
{
    public class DBEngine
    {
        private IniFile m_Config = new IniFile(Environment.CurrentDirectory + "\\Config.ini");
        private string connStr
        {
            get
            {
                return @"Server=" + m_Config.GetString("Database", "IP", "localhost") + ";DATABASE=vision;Port=3306;Uid=root;Pwd=1234;Connect Timeout=1";
            }
        }
        
        public LogManager logManager = new LogManager(true, true);
        public DBEngine()
        {
            
        }

        ~DBEngine()
        {

        }

        public void CreateAllTable()
        {
            MySqlConnection conn = new MySqlConnection(connStr);

            try
            {
                conn.Open();

                string cmdStr = "Create Table Result_tbl (" +
                    "idx integer primary key auto_increment, " +
                    "datetime datetime, " +
                    "model text, " +
                    "seq text, " +
                    "bodyNumber text" +
                    ");";

                MySqlCommand cmd = new MySqlCommand(cmdStr, conn);
                int result = cmd.ExecuteNonQuery();

                if (result == 0)
                {
                    logManager.Info("Result Tbl 생성 완료");
                }
            }
            catch
            {
                
            }

            try
            {
                string cmdStr = "Create Table part_Result_tbl (" + 
                    "idx integer primary key auto_increment," + 
                    "result_idx integer, " + 
                    "position integer, " + 
                    "vision_move_x double, " + 
                    "vision_move_y double, " + 
                    "vision_result bool, " +
                    "datetime datetime, " +
                    "model text, " +
                    "seq text, " +
                    "bodyNumber text, " +
                    "vision_name text, " +
                    "foreign key (result_idx) references result_tbl(idx)" +
                    
                ")";

                MySqlCommand cmd = new MySqlCommand(cmdStr, conn);
                int result = cmd.ExecuteNonQuery();

                if (result == 0)
                {
                    logManager.Info("Part Result Tbl 생성 완료");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                string cmdStr = "Create Table pos_tbl (" +
                    "idx integer primary key auto_increment," +
                    "result_idx integer, " +
                    "position integer, " +
                    "detect_x double, " +
                    "detect_y double, " +
                    "detect_z double, " +
                    "insert_x double, " +
                    "insert_y double, " +
                    "insert_z double, " +
                    "datetime datetime, " +
                    "model text, " +
                    "seq text, " +
                    "bodyNumber text, " +
                    "vision_name text, " +
                    "foreign key (result_idx) references result_tbl(idx)" +

                ")";

                MySqlCommand cmd = new MySqlCommand(cmdStr, conn);
                int result = cmd.ExecuteNonQuery();

                if (result == 0)
                {
                    logManager.Info("Part Result Tbl 생성 완료");
                }

                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected string[,] GetStringArr(String qry, params string[] columnNames)
        {
            DataSet dataSet = GetDataSet(qry);

            if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            DataTable table = dataSet.Tables[0];

            string[,] result = new string[table.Rows.Count, columnNames.Length];

            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < columnNames.Length; j++)
                {
                    result[i, j] = table.Rows[i][columnNames[j]].ToString();
                }
            }

            return result;
        }

        private DataSet GetDataSet(string qry)
        {
            MySqlConnection conn = new MySqlConnection(connStr);

            conn.Open();

            DataSet dataSet = new DataSet();
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(qry, conn);
                    adapter.Fill(dataSet, "data");
                    break;
                }
                catch (Exception e)
                {

                }
            }

            conn.Close();
            return dataSet;
        }

        public void DataBackup(string tableName, int bak_index, params string[] values)
        {
            logManager.Trace("데이터베이스 연결 오류로 백업 시작");

            string content = tableName + "," + bak_index;

            for (int i = 0; i < values.Length; i++)
            {
                content += "," + values[i]; 
            }

            content += Environment.NewLine;

            try
            {
                File.AppendAllText(Environment.CurrentDirectory + "\\database.bak", content, Encoding.ASCII);
            }
            catch
            {

            }

            logManager.Trace("데이터베이스 연결 오류로 백업 종료");
        }

        protected long InsertData(string tableName, int bak_index, int startIndex, params string[] values)
        {

            long lastInsertedId = -1;
            try
            {
                MySqlConnection conn = new MySqlConnection(connStr);

                // logManager.Trace("데이터베이스 오픈 시작");

                conn.Open();

                // logManager.Trace("데이터베이스 오픈 종료");

                StringBuilder sb = new StringBuilder();
                sb.Append("insert into ");
                sb.Append(tableName);
                sb.Append(" values(");
                for (int i = 0; i < values.Length + startIndex; i++)
                {
                    if (i > 0)
                        sb.Append(", ");

                    if (startIndex > i)
                    {
                        sb.Append("null");
                    }
                    else
                    {
                        string value = values[i - startIndex];

                        bool isStr = true;

                        if (value == null)
                            value = "";

                        if (value.StartsWith("$"))
                        {
                            isStr = false;
                            value = value.Remove(0, 1);
                        }

                        if (isStr)
                        {
                            sb.Append("\"");
                        }

                        if (!isStr && value.Length == 0)
                        {
                            sb.Append("null");
                        }
                        else
                        {
                            sb.Append(value.Replace("\"", "\\\""));
                        }


                        if (isStr)
                        {
                            sb.Append("\"");
                        }
                    }
                }

                sb.Append(")");

                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        lastInsertedId = Convert.ToInt64(cmd.ExecuteScalar());
                        // cmd.ExecuteNonQuery();

                        // lastInsertedId = conn.LastInsertRowId;
                        break;
                    }
                    catch (Exception e)
                    {
                        logManager.Error(e.Message + " (" + sb + ")");
                    }
                }

                conn.Close();
            }
            catch (Exception e)
            {
                DataBackup(tableName, bak_index, values);
            }

            return lastInsertedId;
        }


        protected string InsertDataTerm(string tableName, params string[] values)
        {
            //테스트 추가
            MySqlConnection conn = new MySqlConnection(connStr);

            conn.Open();
            long lastInsertedId = -1;

            StringBuilder sb = new StringBuilder();
            sb.Append("insert into ");
            sb.Append(tableName);
            sb.Append("(");

            bool isFirst = true;

            for (int i = 0; i < values.Length; i += 2)
            {
                if (!isFirst)
                {
                    sb.Append(",");
                }

                sb.Append(values[i]);

                isFirst = false;
            }

            sb.Append(")");
            sb.Append(" values(");

            isFirst = true;

            for (int i = 1; i < values.Length; i += 2)
            {
                if (!isFirst)
                {
                    sb.Append(",");
                }

                string value = values[i];

                if (value == null)
                {
                    value = "";
                }

                bool isStr = true;

                if (value.StartsWith("$"))
                {
                    isStr = false;
                    value = value.Remove(0, 1);
                }

                if (isStr)
                {
                    sb.Append("\"");
                }

                if (!isStr && value.Length == 0)
                {
                    sb.Append("null");
                }
                else
                {
                    sb.Append(value.Replace("\"", "\\\""));
                }


                if (isStr)
                {
                    sb.Append("\"");
                }

                isFirst = false;
            }

            sb.Append(")");

            MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    lastInsertedId = Convert.ToInt64(cmd.ExecuteScalar());
                    
                    // lastInsertedId = conn.LastInsertRowId;
                    break;
                }
                catch (Exception ex)
                {

                }
            }

            conn.Close();

            return lastInsertedId.ToString();
        }

        protected void UpdateData(string tableName, string termQry, params string[] columnNameAndValue)
        {
            //테스트 추가
            MySqlConnection conn = new MySqlConnection(connStr);

            conn.Open();

            StringBuilder sb = new StringBuilder();
            sb.Append(" update ");
            sb.Append(tableName);
            sb.Append(" set ");
            for (int i = 0; i < columnNameAndValue.Length; i++)
            {
                if (i % 2 == 0)
                {
                    if (i > 0)
                        sb.Append(", ");
                    //컬럼명
                    sb.Append(columnNameAndValue[i]);
                }
                else
                {
                    sb.Append(" = ");

                    string value = columnNameAndValue[i];
                    value = value.Replace("\"", "\\\"");

                    bool isStr = true;

                    if (value.StartsWith("$"))
                    {
                        isStr = false;
                        value = value.Remove(0, 1);
                    }

                    if (isStr)
                        sb.Append("\"");

                    if (!isStr && value.Length == 0)
                    {
                        sb.Append("null");
                    }
                    else
                    {
                        sb.Append(value);
                    }
                    if (isStr)
                        sb.Append("\"");
                }
            }

            sb.Append(" where ");
            sb.Append(termQry);
            string qry = sb.ToString();
            MySqlCommand cmd = new MySqlCommand(qry, conn);


            for (int i = 0; i < 10; i++)
            {
                try
                {
                    cmd.ExecuteNonQuery();
                    break;
                }
                catch (Exception e)
                {

                }
            }

            conn.Close();
        }

        protected void DeleteData(string tableName, params string[] termQrys)
        {

            MySqlConnection conn = new MySqlConnection(connStr);

            conn.Open();

            StringBuilder sb = new StringBuilder();
            sb.Append("delete from ");
            sb.Append(tableName);
            if (termQrys.Length > 0)
            {
                sb.Append(" where ");
                foreach (string term in termQrys)
                {
                    sb.Append(" " + term);
                }
            }

            string qry = sb.ToString();
            MySqlCommand cmd = new MySqlCommand(qry, conn);


            for (int i = 0; i < 10; i++)
            {
                try
                {
                    cmd.ExecuteNonQuery();
                    break;
                }
                catch
                {

                }
            }

            conn.Close();
        }
    }
}
