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
    public class ProductStatisticsDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public List<PriceDto> Prices { get; set; }
    }
}