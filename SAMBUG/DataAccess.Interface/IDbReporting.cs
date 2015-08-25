using DataAccess.Interface.Domain;

namespace DataAccess.Interface
{
    public interface IDbReporting
    {
        void GetFarmById(long farmId);
    }
}