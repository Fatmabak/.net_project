using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Models.Repositories
{
    public class IRepository : IRepository<Product>
    {
        private readonly ProductDbContext context;

        public IRepository(ProductDbContext context)
        {
            this.context = context;
        }

        public Product Add(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
            return product;
        }

        public Product Delete(int id)
        {
            Product product = context.Products.Find(id);
            if (product != null)
            {
                context.Products.Remove(product);
                context.SaveChanges();
            }
            return product;
        }

        public IEnumerable<Product> GetAll()
        {
            return context.Products;
        }

        public Product Get(int id)
        {
            return context.Products.Find(id);
        }

        public Product Update(Product product)
        {
            context.Products.Attach(product);
            context.Entry(product).State = EntityState.Modified;
            context.SaveChanges();
            return product;
        }

        public IEnumerable<Product> Search(string term)
        {
            if (!string.IsNullOrEmpty(term))
            {
                return context.Products.Where(p => EF.Functions.Like(p.Désignation, $"%{term}%"));
            }
            else
            {
                return context.Products;
            }
        }
    }
}
