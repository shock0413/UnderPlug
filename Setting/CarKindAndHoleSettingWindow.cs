using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;

namespace Setting
{
    public partial class CarKindAndHoleSettingWindow : Form
    {
        private List<StructCarKind> carKinds = new List<StructCarKind>();
        private List<StructHole> holes = new List<StructHole>();
        private IniFile m_Config = new IniFile(Environment.CurrentDirectory + "\\Config.ini");
        private IniFile m_InspectionDetectConfig = new IniFile(Environment.CurrentDirectory + "\\InspectionDetector\\Config.ini");
        private IniFile m_SettingDetectConfig = new IniFile(Environment.CurrentDirectory + "\\SettingDetector\\Config.ini");
        private int Score = 0;
        private int AIScore = 0;
        private int BeforeScore = 0;
        private int BeforeAIScore = 0;

        
        public CarKindAndHoleSettingWindow()
        {
            InitializeComponent();

            dg_CarKind.EditingControlShowing += dg_CarKind_EditingControlShowing;

            SetCarKindList();
            SetHoleList();

            Score = m_Config.GetInt32("Pattern", "Score", 60);
            AIScore = m_InspectionDetectConfig.GetInt32("Info", "Score", 70);
            BeforeScore = Score;
            BeforeAIScore = AIScore;
            score_numeric.Value = Score;
            ai_score_numeric.Value = AIScore;
        }

        private void dg_CarKind_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            ComboBox cb = e.Control as ComboBox;

            if (cb != null)
            {
                cb.SelectedIndexChanged -= cb_SelectedIndexChanged;
                cb.SelectedIndexChanged += cb_SelectedIndexChanged;
            }
        }

        private void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string item = cb.Text;
            int index = dg_CarKind.CurrentCell.RowIndex;

            if (item != null)
            {
                if (item == "AI + 패턴 모드")
                {
                    holes[index].InspectionMode = "ALL";
                }
                else if (item == "AI 모드")
                {
                    holes[index].InspectionMode = "AI";
                }
                else if (item == "패턴 모드")
                {
                    holes[index].InspectionMode = "PATTERN";
                }

                holes[index].InspectionModeStr = item;
            }
        }

        private void SetCarKindList()
        {
            carKinds.Add(new StructCarKind()
            {
                Name = "CN7",
                TotalHoleCount = 9,
                IsEven = false
            });

            carKinds.Add(new StructCarKind()
            {
                Name = "CN7E",
                TotalHoleCount = 4,
                IsEven = true
            });

            carKinds.Add(new StructCarKind()
            {
                Name = "PD",
                TotalHoleCount = 2,
                IsEven = true
            });
        }

        private void SetHoleList()
        {
            for (int i = 0; i < carKinds.Count; i++)
            {
                StructCarKind structCarKind = carKinds[i];

                for (int j = 0; j < structCarKind.TotalHoleCount * 2; j++)
                {
                    StructHole structHole = new StructHole();

                    structHole.Model = structCarKind.Name;

                    if (j < structCarKind.TotalHoleCount)
                    {
                        if (structCarKind.IsEven)
                        {
                            structHole.Position = string.Format("{0:00}", ((j + 1) * 2));
                        }
                        else
                        {
                            structHole.Position = string.Format("{0:00}", (j + 1));
                        }
                    }
                    else
                    {
                        if (structCarKind.IsEven)
                        {
                            structHole.Position = string.Format("{0:00}", ((((j + 1) - structCarKind.TotalHoleCount) * 2) + 100));
                        }
                        else
                        {
                            structHole.Position = string.Format("{0:00}", (((j + 1) - structCarKind.TotalHoleCount) + 100));
                        }
                    }

                    string inspectionMode = m_Config.GetString("InspectionMode", structHole.Model + structHole.Position, "ALL").ToUpper();

                    structHole.InspectionMode = inspectionMode;
                    structHole.BeforeInspectionMode = inspectionMode;

                    if (inspectionMode == "ALL")
                    {
                        structHole.InspectionModeStr = "AI + 패턴 모드";
                        structHole.BeforeInspectionModeStr = "AI + 패턴 모드";
                    }
                    else if (inspectionMode == "AI")
                    {
                        structHole.InspectionModeStr = "AI 모드";
                        structHole.BeforeInspectionModeStr = "AI 모드";
                    }
                    else if (inspectionMode == "PATTERN")
                    {
                        structHole.InspectionModeStr = "패턴 모드";
                        structHole.BeforeInspectionModeStr = "패턴 모드";
                    }

                    structHole.CorrectX = m_Config.GetDouble("Correction", structHole.Model + structHole.Position + "X", 0);
                    structHole.CorrectY = m_Config.GetDouble("Correction", structHole.Model + structHole.Position + "Y", 0);
                    structHole.BeforeCorrectX = structHole.CorrectX;
                    structHole.BeforeCorrectY = structHole.CorrectY;
                    structHole.AICorrectX = m_Config.GetDouble("AI Correction", structHole.Model + structHole.Position + "X", 0);
                    structHole.AICorrectY = m_Config.GetDouble("AI Correction", structHole.Model + structHole.Position + "Y", 0);
                    structHole.BeforeAICorrectX = structHole.AICorrectX;
                    structHole.BeforeAICorrectY = structHole.AICorrectY;

                    holes.Add(structHole);
                }
            }

            dg_CarKind.DataSource = holes;
        }

        private void save_btn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < holes.Count; i++)
            {
                StructHole structHole = holes[i];

                m_Config.WriteValue("InspectionMode", structHole.Model + structHole.Position, structHole.InspectionMode);
                holes[i].BeforeInspectionMode = structHole.InspectionMode;
                holes[i].BeforeInspectionModeStr = structHole.InspectionModeStr;

                m_Config.WriteValue("Correction", structHole.Model + structHole.Position + "X", structHole.CorrectX);
                m_Config.WriteValue("Correction", structHole.Model + structHole.Position + "Y", structHole.CorrectY);
                holes[i].BeforeCorrectX = structHole.CorrectX;
                holes[i].BeforeCorrectY = structHole.CorrectY;

                m_Config.WriteValue("AI Correction", structHole.Model + structHole.Position + "X", structHole.AICorrectX);
                m_Config.WriteValue("AI Correction", structHole.Model + structHole.Position + "Y", structHole.AICorrectY);
                holes[i].BeforeAICorrectX = structHole.AICorrectX;
                holes[i].BeforeAICorrectY = structHole.AICorrectY;
            }

            Score = Convert.ToInt32(score_numeric.Value);
            AIScore = Convert.ToInt32(ai_score_numeric.Value);
            BeforeScore = Score;
            BeforeAIScore = AIScore;

            m_Config.WriteValue("Pattern", "Score", Score);
            m_InspectionDetectConfig.WriteValue("Info", "Score", AIScore);
            m_SettingDetectConfig.WriteValue("Info", "Score", AIScore);

            dg_CarKind.Invalidate();
        }

        

        private void restore_btn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < holes.Count; i++)
            {
                StructHole structHole = holes[i];

                holes[i].InspectionMode = structHole.BeforeInspectionMode;
                holes[i].InspectionModeStr = structHole.BeforeInspectionModeStr;
                holes[i].CorrectX = structHole.BeforeCorrectX;
                holes[i].CorrectY = structHole.BeforeCorrectY;
                holes[i].AICorrectX = structHole.BeforeAICorrectX;
                holes[i].AICorrectY = structHole.BeforeAICorrectY;
            }

            score_numeric.Value = BeforeScore;
            ai_score_numeric.Value = BeforeAIScore;

            dg_CarKind.Invalidate();
        }
    }

    public class StructCarKind
    {
        public string Name { get; set; }
        public int TotalHoleCount { get; set; }
        public bool IsEven { get; set; }
    }

    public class StructHole
    {
        public string Model { get; set; }
        public string Position { get; set; }
        public string BeforeInspectionMode { get; set; }
        public string BeforeInspectionModeStr { get; set; }
        public string InspectionMode { get; set; }
        public string InspectionModeStr { get; set; }
        public double BeforeCorrectX { get; set; }
        public double BeforeCorrectY { get; set; }
        public double CorrectX { get; set; }
        public double CorrectY { get; set; }
        public double BeforeAICorrectX { get; set; }
        public double BeforeAICorrectY { get; set; }
        public double AICorrectX { get; set; }
        public double AICorrectY { get; set; }
    }
}
