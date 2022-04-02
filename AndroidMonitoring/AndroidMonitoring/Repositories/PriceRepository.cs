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
    public class PriceRepository
    {
        private readonly SQLiteConnection _database;
        public PriceRepository(SQLiteConnection database)
        {
            _database = database;
            _database.CreateTable<PriceEntity>();
        }

        public IEnumerable<PriceEntity> GetPrices()
        {
            return _database.Table<PriceEntity>().ToList();
        }

        public PriceEntity GetPriceById(int id)
        {
            return _database.Get<PriceEntity>(id);
        }
        public int Delete(int id)
        {
            return _database.Delete<PriceEntity>(id);
        }
        public int SaveOrUpdate(PriceEntity product)
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

        public int DeleteAll(int productId)
        {
            object stub = new object();
            string query = $"DELETE FROM Prices Where ProductId={productId}";
            lock (stub)
            {
               return _database.Execute(query);
            }
        }

    }
}