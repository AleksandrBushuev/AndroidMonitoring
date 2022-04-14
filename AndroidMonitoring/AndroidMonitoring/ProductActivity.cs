using Android.App;
using Android.OS;
using Android.Widget;
using AndroidMonitoring.Entities;
using AndroidMonitoring.Repositories;
using System.Text.RegularExpressions;

namespace AndroidMonitoring
{
    [Activity(Label = "ProductActivity")]
    class ProductActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.layout_product);

            var dbConnection = DataBaseProvider.GetSQLiteConnection();
            var productRepository = new ProductRepository(dbConnection);
                 

            var btnAddProduct = FindViewById<Button>(Resource.Id.button1);
            var editTextProductName = FindViewById<EditText>(Resource.Id.editTextProductName);
            var editTextUrl = FindViewById<EditText>(Resource.Id.editTextUrl);



            btnAddProduct.Click += (sender, evt) =>
            {
                if (string.IsNullOrEmpty(editTextProductName.Text))
                {
                    Toast.MakeText(this, "Введите название продукта", ToastLength.Short).Show();
                    return;
                }
                                      
                var urlRegex = new Regex(@"^http[s]*://[\w|\S|\s]+");
                if (!urlRegex.IsMatch(editTextUrl.Text))
                {
                    Toast.MakeText(this, "Адрес указан не верно", ToastLength.Short).Show();
                    return;
                }                   

                ProductEntity productEntity = new ProductEntity()
                {
                    Name = editTextProductName.Text,
                    Url = editTextUrl.Text
                };

                int id= productRepository.SaveOrUpdate(productEntity);

                if (id != 0)
                {
                    //показать сообщение
                    editTextProductName.Text = string.Empty;
                    editTextUrl.Text = string.Empty;
                }
                   

            };

        }
    }
}