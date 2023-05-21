using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassificationEngine
{
    public class ModelInput
    {
        [ColumnName("Label"), LoadColumn(0)]
        public string Label { get; set; }


        [ColumnName("ImageSource"), LoadColumn(1)]
        public string ImageSource { get; set; }

    }
}
