using Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Repositories.Interfaces
{
    public interface IGenericRepo<T> where T :  EntityBase
    {
        IEnumerable<T> GetAll(string include = "");
        T Get(int id, string include = "");
        void Insert(T item);
        void Update(T item);
        void Delete(int id);
        
        K ExecuteSqlSingle<K>(string sql) where K: EntityBase;
        IEnumerable<K> ExecuteSqlCollection<K>(string sql) where K: EntityBase;
        void ExecuteCommand(string sql);
    }
}
