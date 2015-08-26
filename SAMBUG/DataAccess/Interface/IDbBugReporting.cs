using DataAccess.Models;

namespace DataAccess.Interface
{
    public interface IDbBugReporting
    {
        Farm GetFarmById(long farmId);
    }
}