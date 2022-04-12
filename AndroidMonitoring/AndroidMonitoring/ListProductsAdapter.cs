using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidMonitoring.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndroidMonitoring
{
    public class ListProductsAdapter : BaseAdapter
    {
        private Context _context;
        private List<ProductDto> _data;

        public override int Count => _data.Count();

        public ListProductsAdapter(Context context, List<ProductDto> data)
        {
            _context = context;
            _data = data;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return _data[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (convertView == null)
            {
                LayoutInflater inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
                view = inflater.Inflate(Resource.Layout.list_item, null);
            }

            TextView textView = view.FindViewById<TextView>(Resource.Id.textViewListItem);
            var product = _data[position];
            textView.Text = product.Name;
            return view;
        }
    }
}