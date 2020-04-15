namespace ITMO.SoftwareTesting.Dates.Database.Abstracts
{
    public interface IDbContextFactory
    {
        DatesContext Create();
    }
}