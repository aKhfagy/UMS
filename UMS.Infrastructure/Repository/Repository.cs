using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Models;
using UMS.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace UMS.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
		protected readonly UmsdbContext _context;
		protected readonly DbSet<T> table;
		protected readonly IConfiguration configuration;

		public Repository(IConfiguration configuration)
		{
            _context = new UmsdbContext();
			table = _context.Set<T>();
			this.configuration = configuration;
		}
		public void Delete(int id)
        {
            try
            {
				T obj = table.Find(id);
				if(obj != null)
					table.Remove(obj);

				throw new Exception("Id does not exist in Db");
			}
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public IEnumerable<T> GetAll()
        {
			return table.ToList();
		}

        public T GetById(int id)
        {
			try
			{
				T obj = table.Find(id);
                if (obj != null)
                    return obj;

                throw new Exception("Id does not exist in Db");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
                return null;
			}
		}

        public void Insert(T obj)
        {
			table.Add(obj);
		}

        public void Save()
        {
			_context.SaveChanges();
		}

        public void Update(T obj)
        {
			table.Attach(obj);
			_context.Entry(obj).State = EntityState.Modified;
		}
    }
}
