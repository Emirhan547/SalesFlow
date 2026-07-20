using Microsoft.ML;
using Microsoft.ML.Data;
using SalesFlow.Business.ML.Models;

namespace SalesFlow.MLTrainer.Trainers;

public class LeadModelTrainer
{
    private readonly MLContext _mlContext = new(seed: 1);

    public void Train(string dataPath, string modelPath)
    {
        IDataView data = _mlContext.Data.LoadFromTextFile<LeadScoringInput>(
            path: dataPath,
            hasHeader: true,
            separatorChar: ',');

        DataOperationsCatalog.TrainTestData split =
            _mlContext.Data.TrainTestSplit(
                data,
                testFraction: 0.2);

        var pipeline =
            _mlContext.Transforms.Conversion.ConvertType(
                nameof(LeadScoringInput.Source),
                outputKind: DataKind.String)

            .Append(
                _mlContext.Transforms.Categorical.OneHotEncoding(
                    "SourceEncoded",
                    nameof(LeadScoringInput.Source)))

            .Append(
                _mlContext.Transforms.Conversion.ConvertType(
                    nameof(LeadScoringInput.Status),
                    outputKind: DataKind.String))

            .Append(
                _mlContext.Transforms.Categorical.OneHotEncoding(
                    "StatusEncoded",
                    nameof(LeadScoringInput.Status)))

            // Bool -> Float
            .Append(_mlContext.Transforms.Conversion.ConvertType(
                "HasEmailFloat",
                nameof(LeadScoringInput.HasEmail),
                outputKind: DataKind.Single))

            .Append(_mlContext.Transforms.Conversion.ConvertType(
                "HasPhoneFloat",
                nameof(LeadScoringInput.HasPhone),
                outputKind: DataKind.Single))

            .Append(_mlContext.Transforms.Conversion.ConvertType(
                "HasWebsiteFloat",
                nameof(LeadScoringInput.HasWebsite),
                outputKind: DataKind.Single))

            .Append(_mlContext.Transforms.Conversion.ConvertType(
                "HasCompanyFloat",
                nameof(LeadScoringInput.HasCompany),
                outputKind: DataKind.Single))

            .Append(_mlContext.Transforms.Conversion.ConvertType(
                "HasAddressFloat",
                nameof(LeadScoringInput.HasAddress),
                outputKind: DataKind.Single))

            .Append(_mlContext.Transforms.Conversion.ConvertType(
                "IsAssignedFloat",
                nameof(LeadScoringInput.IsAssigned),
                outputKind: DataKind.Single))

            .Append(
                _mlContext.Transforms.NormalizeMinMax(
                    nameof(LeadScoringInput.DescriptionLength)))

            .Append(
                _mlContext.Transforms.Concatenate(
                    "Features",
                    "SourceEncoded",
                    "StatusEncoded",

                    "HasEmailFloat",
                    "HasPhoneFloat",
                    "HasWebsiteFloat",

                    "HasCompanyFloat",
                    "HasAddressFloat",
                    "IsAssignedFloat",

                    nameof(LeadScoringInput.DescriptionLength)))

            .Append(
                _mlContext.BinaryClassification.Trainers
                    .SdcaLogisticRegression());

        ITransformer model = pipeline.Fit(split.TrainSet);

        IDataView predictions = model.Transform(split.TestSet);

        CalibratedBinaryClassificationMetrics metrics =
            _mlContext.BinaryClassification.Evaluate(predictions);

        Console.WriteLine("========== MODEL METRICS ==========");
        Console.WriteLine($"Accuracy : {metrics.Accuracy:P2}");
        Console.WriteLine($"AUC      : {metrics.AreaUnderRocCurve:P2}");
        Console.WriteLine($"F1 Score : {metrics.F1Score:P2}");
        Console.WriteLine($"Precision: {metrics.PositivePrecision:P2}");
        Console.WriteLine($"Recall   : {metrics.PositiveRecall:P2}");
        Console.WriteLine("===================================");

        Directory.CreateDirectory(Path.GetDirectoryName(modelPath)!);

        _mlContext.Model.Save(
            model,
            split.TrainSet.Schema,
            modelPath);

        Console.WriteLine();
        Console.WriteLine($"Model saved:");
        Console.WriteLine(modelPath);
    }
}