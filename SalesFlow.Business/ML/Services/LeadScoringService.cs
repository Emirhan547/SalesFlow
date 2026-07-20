using Microsoft.ML;
using SalesFlow.Business.Dtos.LeadDtos;
using SalesFlow.Business.ML.Helpers;
using SalesFlow.Business.ML.Mapper;
using SalesFlow.Business.ML.Models;
using SalesFlow.Entity.Entities;

namespace SalesFlow.Business.ML.Services;

public class LeadScoringService : ILeadScoringService
{
    private readonly PredictionEngine<LeadScoringInput, LeadScoringPrediction> _engine;

    public LeadScoringService()
    {
        MLContext mlContext = new();

        string modelPath = Path.Combine(
            AppContext.BaseDirectory,
            "ML",
            "TrainedModels",
            "LeadScoringModel.zip");

        if (!File.Exists(modelPath))
        {
            throw new FileNotFoundException(
                $"Lead scoring model not found: {modelPath}");
        }

        ITransformer model =
            mlContext.Model.Load(
                modelPath,
                out _);

        _engine =
            mlContext.Model.CreatePredictionEngine<
                LeadScoringInput,
                LeadScoringPrediction>(model);
    }

    public Task<LeadScoreResponse> PredictAsync(Lead lead)
    {
        LeadScoringInput input =
            LeadScoringMapper.Map(lead);

        LeadScoringPrediction prediction =
            _engine.Predict(input);

        double score =
            prediction.Probability * 100;

        return Task.FromResult(new LeadScoreResponse
        {
            Score = score,
            Category =
                LeadRecommendationBuilder.BuildCategory(score),

            Recommendation =
                LeadRecommendationBuilder.BuildRecommendation(score)
        });
    }
}