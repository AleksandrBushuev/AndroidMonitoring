using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using AndroidMonitoring.Dto;
using AndroidMonitoring.Repositories;
using AndroidMonitoring.Services;

namespace AndroidMonitoring
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ListProductsAdapter _productsAdapter;
        private ListView _listView;
        private MonitoringService _monitoringService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            var dbConnection = DataBaseProvider.GetSQLiteConnection();
            var productRepository = new ProductRepository(dbConnection);
            var priceRepository = new PriceRepository(dbConnection);
                      
            _monitoringService = new MonitoringService(productRepository, priceRepository);                   
            _listView = FindViewById<ListView>(Resource.Id.list);

            _listView.ItemClick += (sender, evt) =>
             {
                 int idProduct = (int) _productsAdapter.GetItemId(evt.Position);

                 Intent intent = new Intent(this, typeof(GraphActivity));
                 intent.PutExtra("id_product", idProduct);
                 StartActivity(intent);
             };           

        }

        protected override void OnStart()
        {
            base.OnStart();

            List<ProductDto> products = _monitoringService.GetProducts()
               .OrderBy(product => product.Name)
               .ToList();

            _productsAdapter = new ListProductsAdapter(this, products);
            _listView.Adapter = _productsAdapter;
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                Intent intent = new Intent(this, typeof(ProductActivity));
                StartActivity(intent);   
                
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }
                     
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

               
    }
}
