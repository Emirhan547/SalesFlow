namespace SalesFlow.Business.ML.Helpers;

public static class LeadRecommendationBuilder
{
    public static string BuildRecommendation(double score)
    {
        return score switch
        {
            >= 90 => "Prioritize this lead immediately.",
            >= 70 => "Contact this lead soon.",
            >= 50 => "Monitor and nurture this lead.",
            >= 30 => "Low priority follow-up recommended.",
            _ => "Minimal effort recommended."
        };
    }

    public static string BuildCategory(double score)
    {
        return score switch
        {
            >= 90 => "Very High",
            >= 70 => "High",
            >= 50 => "Medium",
            >= 30 => "Low",
            _ => "Very Low"
        };
    }
}