using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.ML;

namespace ClassificationEngine
{
    public class ClassificationManager
    {
        private Lazy<PredictionEngine<ModelInput, ModelOutput>> predictionEngine;
        MLContext mlContext;
        string modelPath;

        public ClassificationManager()
        {
            mlContext = new MLContext();
        }

        public void Create(string modelPath)
        {
            this.modelPath = modelPath;
            predictionEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(CreatePredictionEngine);
        }

        public PredictionEngine<ModelInput, ModelOutput> CreatePredictionEngine()
        {
            //모델과 예측엔진 불러오기
            string path = modelPath;
            ITransformer mlModel = mlContext.Model.Load(path, out var modelInputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

            return predEngine;
        }

        public Result Run(string path)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ModelInput input = new ModelInput()
            {
                ImageSource = path
            };
            
            ModelOutput output = predictionEngine.Value.Predict(input);

            Result result = new Result();
            result.Model = output.Prediction;
            result.Score = output.Score;
            sw.Stop();
            Console.WriteLine("검사 소요 시간 : " + sw.ElapsedMilliseconds);

            return result;
        }

        public class Result
        {
            public string Model { get; set; }
            public float[] Score { get; set; }
        }
    }
}
