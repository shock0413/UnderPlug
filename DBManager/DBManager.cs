using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public class DBManager : DBEngine
    {
        private const string RESULT_TBL = "Result_tbl";
        private const string PART_RESULT_TBL = "Part_Result_tbl";
        private const string POS_TBL = "pos_tbl";
        private const string Complete_tbl = "Complete_tbl";
        private const string RECEIVE_POS_TBL = "receive_pos_tbl";

        public long InsertResult(int bak_index, DateTime dateTime, string model, string info1, string info2)
        {
            return InsertData(RESULT_TBL, bak_index, 1, dateTime.ToString("yyyy-MM-dd HH:mm:ss"), model, info1, info2);
        }

        public long InsertPartResult(int bak_index, long? resultIdx, int position, double visionMoveX, double visionMoveY, bool visionResult, DateTime dateTime, string model, string info1, string info2, string visionName, string inspectionMode)
        {
            return InsertData(PART_RESULT_TBL, bak_index, 1,
                resultIdx.ToString(),
                "$" + position,
                "$" + visionMoveX,
                "$" + visionMoveY,
                visionResult.ToString(),
                dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                model,
                info1,
                info2,
                visionName,
                inspectionMode
                );
        }

        public long InsertReceivePosResult(long index, double beforeX, double beforeY, double beforeZ, double afterX, double afterY, double afterZ, DateTime datetime, string visionName, double beforeJ1, double beforeJ2, double beforeJ3, double beforeJ4, double beforeJ5, double beforeJ6, double afterJ1, double afterJ2, double afterJ3, double afterJ4, double afterJ5, double afterJ6)
        {
            return InsertData(RECEIVE_POS_TBL, 0, 1,
                datetime.ToString("yyyy-MM-dd HH:mm:ss"),
                visionName,
                "$" + beforeX,
                "$" + beforeY,
                "$" + beforeZ,
                "$" + afterX,
                "$" + afterY,
                "$" + afterZ,
                "$" + beforeJ1,
                "$" + beforeJ2,
                "$" + beforeJ3,
                "$" + beforeJ4,
                "$" + beforeJ5,
                "$" + beforeJ6,
                "$" + afterJ1,
                "$" + afterJ2,
                "$" + afterJ3,
                "$" + afterJ4,
                "$" + afterJ5,
                "$" + afterJ6);
        }

        public long InsertPosResult(int bak_index, long index, int position, double detectX, double detectY, double detectZ, double insertX, double insertY, double insertZ, DateTime dateTime, string model, string info1, string info2, string visionName, double detectJ1, double detectJ2, double detectJ3, double detectJ4, double detectJ5, double detectJ6, double insertJ1, double insertJ2, double insertJ3, double insertJ4, double insertJ5, double insertJ6)
        {
            return InsertData(POS_TBL, bak_index, 1,
                "$" + index,
                "$" + position,
                "$" + detectX,
                "$" + detectY,
                "$" + detectZ,
                "$" + insertX,
                "$" + insertY,
                "$" + insertZ,
                dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                model,
                info1,
                info2,
                visionName,
                "$" + detectJ1,
                "$" + detectJ2,
                "$" + detectJ3,
                "$" + detectJ4,
                "$" + detectJ5,
                "$" + detectJ6,
                "$" + insertJ1,
                "$" + insertJ2,
                "$" + insertJ3,
                "$" + insertJ4,
                "$" + insertJ5,
                "$" + insertJ6
                );
        }

        public long InsertPosResult(int bak_index, long index, int position, double detectX, double detectY, double detectZ, double insertX, double insertY, double insertZ, DateTime dateTime, string model, string info1, string info2, string visionName, double detectJ1, double detectJ2, double detectJ3, double detectJ4, double detectJ5, double detectJ6, double insertJ1, double insertJ2, double insertJ3, double insertJ4, double insertJ5, double insertJ6, double validX, double validY, double validZ, double validJ1, double validJ2, double validJ3, double validJ4, double validJ5, double validJ6, double waitX, double waitY, double waitZ, double waitJ1, double waitJ2, double waitJ3, double waitJ4, double waitJ5, double waitJ6)
        {
            return InsertData(POS_TBL, bak_index, 1,
                "$" + index,
                "$" + position,
                "$" + detectX,
                "$" + detectY,
                "$" + detectZ,
                "$" + insertX,
                "$" + insertY,
                "$" + insertZ,
                dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                model,
                info1,
                info2,
                visionName,
                "$" + detectJ1,
                "$" + detectJ2,
                "$" + detectJ3,
                "$" + detectJ4,
                "$" + detectJ5,
                "$" + detectJ6,
                "$" + insertJ1,
                "$" + insertJ2,
                "$" + insertJ3,
                "$" + insertJ4,
                "$" + insertJ5,
                "$" + insertJ6,
                "$" + validX,
                "$" + validY,
                "$" + validZ,
                "$" + validJ1,
                "$" + validJ2,
                "$" + validJ3,
                "$" + validJ4,
                "$" + validJ5,
                "$" + validJ6,
                "$" + waitX,
                "$" + waitY,
                "$" + waitZ,
                "$" + waitJ1,
                "$" + waitJ2,
                "$" + waitJ3,
                "$" + waitJ4,
                "$" + waitJ5,
                "$" + waitJ6
                );
        }

        public long InsertPosResult(int bak_index, long index, int position, double detectX, double detectY, double detectZ, double insertX, double insertY, double insertZ, double detectJ1, double detectJ2, double detectJ3, double detectJ4, double detectJ5, double detectJ6,
            double insertJ1, double insertJ2, double insertJ3, double insertJ4, double insertJ5, double insertJ6,
            DateTime dateTime, string model, string info1, string info2, string visionName)
        {
            return InsertData(POS_TBL, bak_index, 1,
                "$" + index,
                "$" + position,
                "$" + detectX,
                "$" + detectY,
                "$" + detectZ,
                "$" + insertX,
                "$" + insertY,
                "$" + insertZ,
                dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                model,
                info1,
                info2,
                visionName,
                "$" + detectJ1,
                "$" + detectJ2,
                "$" + detectJ3,
                "$" + detectJ4,
                "$" + detectJ5,
                "$" + detectJ6,
                "$" + insertJ1,
                "$" + insertJ2,
                "$" + insertJ3,
                "$" + insertJ4,
                "$" + insertJ5,
                "$" + insertJ6
                );
        }

        public long? GetResultIndex(DateTime dateTime, string hangerNumber, string bodyNumber)
        {
            string[,] datas = null;

            try
            {
                string qry = "select idx from Result_tbl where " +
                "datetime between '" + dateTime.ToString("yyyy-MM-dd") + " 00:00:00' and '" + dateTime.AddDays(1).ToString("yyyy-MM-dd") + " 23:59:59'" +
                " and bodyNumber = '" + bodyNumber + "'" + " and seq = '" + hangerNumber + "'";

                datas = GetStringArr(qry,
                    "idx"
                    );
            }
            catch
            {

            }

            if (datas != null)
            {
                return Convert.ToInt64(datas[0, 0]);
            }
            else
            {
                return null;
            }
        }

        public List<StructResult> GetResult(DateTime startDateTime, DateTime endDateTime)
        {
            List<StructResult> results = new List<StructResult>();

            string qry = "select * from part_result_tbl where datetime between '" + startDateTime.ToString("yyyy-MM-dd 00:00:00") + "' and '" + endDateTime.ToString("yyyy-MM-dd 23:59:59") + "'";
            string[,] datas = GetStringArr(qry,
                "idx",
                "datetime",
                "model", 
                "seq",
                "bodyNumber",
                "position",
                "vision_move_x",
                "vision_move_y",
                "vision_result",
                "vision_name"
                );
            if (datas != null)
            {
                for (int i = 0; i < datas.GetLength(0); i++)
                {
                    StructResult result = new StructResult();
                    result.Idx = datas[i, 0];
                    result.DateTime = DateTime.Parse(datas[i, 1], CultureInfo.CurrentCulture).ToString("yyyy-MM-dd HH:mm:ss");
                    result.Model = datas[i, 2];
                    result.Seq = datas[i, 3];
                    result.BodyNumber = datas[i, 4];
                    result.Position = datas[i, 5];
                    result.moveX = datas[i, 6];
                    result.moveY = datas[i, 7];
                    result.Result = datas[i, 8];
                    result.VisionName = datas[i, 9];

                    results.Add(result);
                }
            }

            return results;
        }

        public int GetCarTotalCount(DateTime startDateTime, DateTime endDateTime)
        {
            
            string qry = "select * from part_Result_tbl where datetime between '" + startDateTime.ToString("yyyy-MM-dd 00:00:00") + "' and '" + endDateTime.ToString("yyyy-MM-dd 23:59:59") + "'  group by bodyNumber";
            string[,] datas = GetStringArr(qry,
                "idx"
                );

            return datas.Length;
        }


        public long InsertTotalCompleteData(int bak_index, long resultIndex, DateTime dateTime, string model, string info1, string info2)
        {

            return InsertData(Complete_tbl, bak_index, 1,
                "$" + resultIndex,
                dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                model,
                info1,
                info2
                );
        }
    }

    public class StructResult
    {
        public string Idx { get; set; }
        public string DateTime { get; set; }
        public string Position { get; set; }
        public string Model { get; set; }
        public string Seq { get; set; }
        public string BodyNumber { get; set; }
    
        public string moveX { get; set; }
        public string moveY { get; set; }
        public string Result { get; set; }

        public string VisionName { get; set; }
    }
}
