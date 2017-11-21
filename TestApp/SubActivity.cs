using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TestApp;
//using Xamarin.Forms;
//using System.Collections.Generic;

namespace TestApp
{
    [Activity(Label = "SubActivity")]
    public class SubActivity : Activity
    {
        private ListView listnames;
        private List<string> itemlist;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.NextMenu);

            /*var cars = new string[]{
                "BMW",
                "Audi",
                "Porsche",
                "VW",
                "Seat"
            };*/

            listnames = FindViewById<ListView>(Resource.Id.listViewCars);

            itemlist = new List<string>();
            itemlist.Add("Porsche");
            itemlist.Add("Audi");
            itemlist.Add("VW");
            itemlist.Add("Seat");



            ArrayAdapter<string>ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, itemlist);
            listnames.Adapter = ListAdapter;
            listnames.ItemClick += Listnames_ItemClick;
            //protected override void OnListItemClick(ListView l, View v, int position, long id)

            //listview.ItemsSource = cars;


            // Create your application here
        }
        private void Listnames_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, e.Position.ToString(), ToastLength.Long).Show();

            switch(e.Position)
            {
                case 0:
                    StartActivity(typeof(PorscheActivity));
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }
    }
}