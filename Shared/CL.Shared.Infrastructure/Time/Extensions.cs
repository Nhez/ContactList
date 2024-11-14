namespace CL.Shared.Infrastructure.Time;

public static class Extensions
{
    private static readonly DateTimeOffset ClarionGenesisDateOffset = new DateTime(1800, 12, 28, 0, 0, 0);
    private static readonly DateTime ClarionGenesisDate = new DateTime(1800, 12, 28, 0, 0, 0);

    public static DateTimeOffset ToDateTimeOffsetFromClarion(this int clarionNumberOfDays)
    {
        var clarionDateTimeOffset = ClarionGenesisDateOffset.AddDays(clarionNumberOfDays);

        return clarionDateTimeOffset;
    }

    public static DateTime ToDateTimeFromClarion(this int clarionNumberOfDays)
    {
        var clarionDateTime = ClarionGenesisDate.AddDays(clarionNumberOfDays);

        return clarionDateTime;
    }
}