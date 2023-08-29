using Final.Models;
using Final.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Final.Repositories
{
    public class GenericRepo<T> : IGenericRepo<T> where T : EntityBase
    {
        DbContext db;
        DbSet<T> dbSet;
        public GenericRepo(DbContext db)
        {
            this.db = db;
            this.dbSet = db.Set<T>();
        }

        public T Get(int id, string include = "")
        {
            if(include == "")
                return dbSet.FirstOrDefault(x=> x.Id  == id);
            else
                return dbSet.Include(include).FirstOrDefault(x=> x.Id == id);
        }

        public IEnumerable<T> GetAll(string include = "")
        {
            if (include == "")
                return dbSet.ToList();
            else
                return dbSet.Include(include).ToList();
        }

        public void Insert(T item)
        {
            dbSet.Add(item);
            db.SaveChanges();
        }

        public void Update(T item)
        {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void Delete(int id)
        {
            var item = dbSet.FirstOrDefault(x=> x.Id == id);
            if (item !=null)
            {
                dbSet.Remove(item);
                db.SaveChanges();
            }
        }

        public void ExecuteCommand(string sql)
        {
            db.Database.ExecuteSqlCommand(sql);
        }

        public IEnumerable<K> ExecuteSqlCollection<K>(string sql) where K : EntityBase
        {
            return db.Database.SqlQuery<K>(sql).ToList();
        }

        public K ExecuteSqlSingle<K>(string sql) where K : EntityBase
        {
             return db.Database.SqlQuery<K>(sql).FirstOrDefault();
        }

        
    }
}