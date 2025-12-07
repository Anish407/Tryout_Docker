using CalllerApi.Core.Repository;

namespace CallerApi.Infra.Repository
{
    public class Repository:IRepository
    {
        public string GetData()
        {
            return "Data from CallerApi.Infra.Repository";
        }
    }
}
