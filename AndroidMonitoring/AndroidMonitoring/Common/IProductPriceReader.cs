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
using System.Threading.Tasks;

namespace AndroidMonitoring.Common
{
    public interface IProductPriceReader
    {
        public Task<double> GetPriceAsync(string url);
    }
}