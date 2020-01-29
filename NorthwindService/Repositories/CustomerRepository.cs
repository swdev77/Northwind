using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Packt.Shared;

namespace NorthwindService.Repositories
{
    public class CustomerRepository: ICustomerRepository
    {
        private static ConcurrentDictionary<string, Customer> customerCache;
        private readonly Northwind db;

        public CustomerRepository(Northwind db)
        {
            this.db = db;
            customerCache = new ConcurrentDictionary<string, Customer>(
                db.Customers.ToDictionary(c => c.CustomerID)
            );
        }

        public async Task<Customer> CreateAsync(Customer c)
        {
            c.CustomerID = c.CustomerID.ToUpper();
            EntityEntry<Customer> added = await db.Customers.AddAsync(c);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                return customerCache.AddOrUpdate(c.CustomerID, c, UpdateCache);
            }
            else
            {
                return null;
            }
        }

        private Customer UpdateCache(string id, Customer c)
        {
            Customer old;
            if (customerCache.TryGetValue(id, out old))
            {
                if(customerCache.TryUpdate(id, c, old))
                {
                    return c;
                }
            }
            return null;
        }

        public async Task<bool?> DeleteAsync(string id)
        {
            id = id.ToUpper();

            Customer c = db.Customers.Find(id);

            db.Customers.Remove(c);
            int affected = await db.SaveChangesAsync();

            if (affected == 1)
            {
                return customerCache.TryRemove(id, out c);
            }
            else 
            {
                return null;
            }
        }

        public Task<IEnumerable<Customer>> RetrieveAllAsync()
        {
            return Task.Run<IEnumerable<Customer>>(
                () => customerCache.Values
            );
        }

        public Task<Customer> RetrieveAsync(string id)
        {
            return Task.Run(() =>
            {
                id = id.ToUpper();
                Customer c;
                customerCache.TryGetValue(id, out c);
                return c;
            });
        }

        public async Task<Customer> UpdateAsync(string id, Customer c)
        {
            id = id.ToUpper();
            c.CustomerID = c.CustomerID.ToUpper();
            db.Customers.Update(c);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                return UpdateCache(id, c);
            }
            return null;
        }
    }
}