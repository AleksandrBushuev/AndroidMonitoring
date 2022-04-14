using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidMonitoring.Common;
using AndroidMonitoring.Dto;
using AndroidMonitoring.Repositories;
using AndroidMonitoring.Services;
using Microcharts;
using Microcharts.Droid;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AndroidMonitoring
{
    [Activity(Label = "GraphActivity")]
    public class GraphActivity : Activity
    {       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.layout_graph);

            int idProduct = Intent.GetIntExtra("id_product", -1);

            var dbConnection = DataBaseProvider.GetSQLiteConnection();
            var productRepository = new ProductRepository(dbConnection);
            var priceRepository = new PriceRepository(dbConnection);

            var monitoringService = new MonitoringService(productRepository, priceRepository);

            List<PriceDto> prices = new List<PriceDto>();
            try
            {
                var statistic = monitoringService.GetStatistics(idProduct);
                prices = statistic.Prices;                   
            }catch(KeyNotFoundException)
            {
                Toast.MakeText(this, "Продукт не существует", ToastLength.Short).Show();
            }catch(Exception)
            {
                Toast.MakeText(this, "Ошибка при выполнении операции", ToastLength.Short).Show();
            }           
            
            if (prices.Any())
            {
                DrawChart(prices);
            }
            else
            {
                Toast.MakeText(this, "Отсутствуют данные для построения графика", ToastLength.Short).Show();
            }

            Button buttonUpdate = FindViewById<Button>(Resource.Id.buttonUpdate);
                      

            buttonUpdate.Click += async (sender, evt) =>
            {
                bool isUpdate = await monitoringService.UpdatePricesAsync(idProduct, new SportDepoShop());
                if(!isUpdate)
                    Toast.MakeText(this, "Не удалось обновить данные", ToastLength.Short).Show();

            };           

        }

        private void DrawChart(List<PriceDto> prices)
        {
            List<ChartEntry> entries = prices
                .OrderBy(p => p.Date)
                .Select(p => new ChartEntry((float)p.Price)
                {
                    Label = p.Date.ToString("dd.MM.yy"),
                    ValueLabel = p.Price.ToString()
                }).ToList();

            var chart = new LineChart() {
                Entries = entries,
                LineMode = LineMode.Straight,
                LabelTextSize = 30                
            };  

            var chartView = FindViewById<ChartView>(Resource.Id.chartView);
            chartView.Chart = chart;
           
        }
    }
}