using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace utill
{
    public class DBEngine
    {
        private readonly string conStr = @"Data Source=VisionData.db";

        public DBEngine()
        {

        }

        ~DBEngine()
        {

        }

        protected string[,] GetStringArr(String qry, params string[] columnNames)
        {

            using (DataSet dataSet = GetDataSet(qry))
            {
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
        }

        private DataSet GetDataSet(string qry)
        {
            SQLiteConnection con = new SQLiteConnection(conStr);

            con.Open();

            DataSet dataSet = new DataSet();
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(qry, con))
                    {
                        adapter.Fill(dataSet, "data");
                    }
                    break;
                }
                catch (Exception e)
                {

                }
            }

            con.Close();
            return dataSet;
        }


        protected long InsertData(string tableName, int startIndex, params string[] values)
        {

            long lastInsertedId = -1;
            try
            {
                SQLiteConnection con = new SQLiteConnection(conStr);

                con.Open();

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

                using (SQLiteCommand cmd = new SQLiteCommand(sb.ToString(), con))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        try
                        {
                            cmd.ExecuteNonQuery();

                            lastInsertedId = con.LastInsertRowId;
                            break;
                        }
                        catch
                        {

                        }
                    }
                }

                con.Close();
            }
            catch (Exception e)
            {

            }

            return lastInsertedId;
        }


        protected string InsertDataTerm(string tableName, params string[] values)
        {
            //테스트 추가
            SQLiteConnection con = new SQLiteConnection(conStr);

            con.Open();
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

            using (SQLiteCommand cmd = new SQLiteCommand(sb.ToString(), con))
            {
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        lastInsertedId = con.LastInsertRowId;
                        break;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            con.Close();

            return lastInsertedId.ToString();
        }

        protected void UpdateData(string tableName, string termQry, params string[] columnNameAndValue)
        {
            //테스트 추가
            SQLiteConnection con = new SQLiteConnection(conStr);

            con.Open();

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
            using (SQLiteCommand cmd = new SQLiteCommand(qry, con))
            {
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
            }

            con.Close();
        }

        protected void DeleteData(string tableName, params string[] termQrys)
        {

            SQLiteConnection con = new SQLiteConnection(conStr);

            con.Open();

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
            using (SQLiteCommand cmd = new SQLiteCommand(qry, con))
            {
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
            }

            con.Close();
        }
    }
}
