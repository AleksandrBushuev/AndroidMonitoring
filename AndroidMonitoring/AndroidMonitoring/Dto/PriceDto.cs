using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndroidMonitoring.Dto
{
    public class PriceDto
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
    }
}