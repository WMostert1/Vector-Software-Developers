using DataAccess.Interface.Domain;

namespace DataAccess.Interface
{
    public interface IDbReporting
    {
        Farm GetFarmById(long farmId);
    }
}