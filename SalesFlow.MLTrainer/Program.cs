using SalesFlow.MLTrainer.Generators;
using SalesFlow.MLTrainer.Trainers;

LeadTrainingDataGenerator generator = new();

generator.Generate(
    @"Data\lead-training-data.csv",
    5000);

Console.WriteLine("Training data generated.");

string solutionRoot = Path.GetFullPath(
    Path.Combine(
        AppContext.BaseDirectory,
        "..",
        "..",
        "..",
        ".."));

string modelPath = Path.Combine(
    solutionRoot,
    "SalesFlow.Business",
    "ML",
    "TrainedModels",
    "LeadScoringModel.zip");

Console.WriteLine($"Model Path: {modelPath}");

LeadModelTrainer trainer = new();

trainer.Train(
    @"Data\lead-training-data.csv",
    modelPath);

Console.WriteLine("Training completed.");