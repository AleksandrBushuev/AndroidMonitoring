using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidMonitoring.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndroidMonitoring.Repositories
{
    public class ProductRepository
    {
        private readonly SQLiteConnection _database;
        public ProductRepository(SQLiteConnection database) 
        {
            _database = database;
            _database.CreateTable<ProductEntity>();
        }

        public IEnumerable<ProductEntity> GetProducts()
        {            
            return _database.Table<ProductEntity>().ToList(); 
        }

        public ProductEntity GetProductById(int id)
        {
            return _database.Get<ProductEntity>(id);
        }
        public int Delete(int id)
        {
            return _database.Delete<ProductEntity>(id);
        }
        public int SaveOrUpdate(ProductEntity product)
        {
            if (product.Id != 0)
            {
                _database.Update(product);
                return product.Id;
            }
            else
            {
                return _database.Insert(product);
            }
        }
    }
}