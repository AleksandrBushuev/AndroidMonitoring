using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndroidMonitoring.Entities
{
    [Table("Prices")]
    public class PriceEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
        public int ProductId { get; set; }
    }
}