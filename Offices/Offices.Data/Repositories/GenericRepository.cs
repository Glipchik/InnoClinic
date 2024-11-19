using Offices.Data.Models;

namespace Offices.Data.Repositories
{
    public class GenericRepository<T>: IGenericRepository<T> where T : class
    {
    }
}
