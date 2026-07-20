using CsvHelper;
using SalesFlow.Entity.Enums;
using SalesFlow.MLTrainer.Models;
using System.Globalization;

namespace SalesFlow.MLTrainer.Generators;

public class LeadTrainingDataGenerator
{
    private readonly Random _random = new();

    public void Generate(string outputPath, int count)
    {
        List<LeadTrainingRecord> records = new();

        for (int i = 0; i < count; i++)
        {
            records.Add(CreateRecord());
        }

        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

        using StreamWriter writer = new(outputPath);
        using CsvWriter csv = new(writer, CultureInfo.InvariantCulture);

        csv.WriteRecords(records);
    }

    private LeadTrainingRecord CreateRecord()
    {
        LeadSource source = (LeadSource)_random.Next(1, 8);
        LeadStatus status = (LeadStatus)_random.Next(1, 6);

        bool hasEmail = _random.NextDouble() < 0.90;
        bool hasPhone = _random.NextDouble() < 0.80;
        bool hasWebsite = _random.NextDouble() < 0.45;

        bool hasCompany = _random.NextDouble() < 0.60;
        bool hasAddress = _random.NextDouble() < 0.40;
        bool isAssigned = _random.NextDouble() < 0.75;

        int descriptionLength = _random.Next(20, 600);

        double probability = 0.15;

        // Lead Source etkisi
        switch (source)
        {
            case LeadSource.Referral:
                probability += 0.35;
                break;

            case LeadSource.Website:
                probability += 0.20;
                break;

            case LeadSource.SocialMedia:
                probability += 0.15;
                break;

            case LeadSource.Advertisement:
                probability += 0.10;
                break;

            case LeadSource.Phone:
                probability += 0.05;
                break;

            case LeadSource.Email:
                probability += 0.03;
                break;
        }

        // Lead Status etkisi
        switch (status)
        {
            case LeadStatus.Contacted:
                probability += 0.15;
                break;

            case LeadStatus.Qualified:
                probability += 0.40;
                break;

            case LeadStatus.Lost:
                probability = 0.01;
                break;

            case LeadStatus.Converted:
                probability = 1;
                break;
        }

        // Bilgilerin dolu olması avantaj sağlar
        if (hasEmail)
            probability += 0.05;

        if (hasPhone)
            probability += 0.05;

        if (hasWebsite)
            probability += 0.10;

        if (hasCompany)
            probability += 0.05;

        if (hasAddress)
            probability += 0.03;

        if (isAssigned)
            probability += 0.08;

        if (descriptionLength > 250)
            probability += 0.05;

        probability = Math.Clamp(probability, 0, 1);

        bool converted;

        if (status == LeadStatus.Converted)
        {
            converted = true;
        }
        else if (status == LeadStatus.Lost)
        {
            converted = false;
        }
        else
        {
            converted = _random.NextDouble() < probability;
        }

        return new LeadTrainingRecord
        {
            Source = (int)source,
            Status = (int)status,

            HasEmail = hasEmail,
            HasPhone = hasPhone,
            HasWebsite = hasWebsite,

            HasCompany = hasCompany,
            HasAddress = hasAddress,
            IsAssigned = isAssigned,

            DescriptionLength = descriptionLength,

            Converted = converted
        };
    }
}