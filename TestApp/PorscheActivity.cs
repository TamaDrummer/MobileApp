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

namespace TestApp
{
    [Activity(Label = "PorscheActivity")]
    public class PorscheActivity : Activity
    {
        
        
        
        

        
        //private string[] itemarray;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            

            SetContentView(Resource.Layout.Porsche);
            CheckBox checkbox1 = FindViewById<CheckBox>(Resource.Id.checkBox1);
            CheckBox checkbox2 = FindViewById<CheckBox>(Resource.Id.checkBox2);
            RadioButton radiobutton1 = FindViewById<RadioButton>(Resource.Id.radioButton1);
            RadioButton radiobutton2 = FindViewById<RadioButton>(Resource.Id.radioButton2);
            SeekBar seekbar = FindViewById<SeekBar>(Resource.Id.seekBar1);
            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner1);
            Switch switch1 = FindViewById<Switch>(Resource.Id.switch1);
            ProgressBar progressbar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            TextView textview = FindViewById<TextView>(Resource.Id.textView1);


            //Just for Testing
            /*RaspData raspdata = new RaspData();

            raspdata.Connect();
            raspdata.Disconnect();
            raspdata.sendMessage("Ich bin es ");
            raspdata.getMessage();*/



            checkbox1.Click += (o, e) =>
            {
                if (checkbox1.Checked)
                {
                    Toast.MakeText(this, " 1 Selected", ToastLength.Short).Show();
                }
                else
                    Toast.MakeText(this, " 1 Not Selected", ToastLength.Short).Show();

            };
            checkbox2.Click += (o, e) =>
            {
                if (checkbox2.Checked)
                {
                    Toast.MakeText(this, " 2 Selected", ToastLength.Short).Show();
                }
                else
                    Toast.MakeText(this, " 2 Not Selected", ToastLength.Short).Show();

            };
            radiobutton1.Click += (o, e) =>
            {

                Toast.MakeText(this, " Radio 1 Selected", ToastLength.Short).Show();
            };
            radiobutton2.Click += (o, e) =>
            {

                Toast.MakeText(this, " Radio 2 Selected", ToastLength.Short).Show();
            };

            seekbar.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
                if (e.FromUser)
                {
                    textview.Text = string.Format("The value of the SeekBar is {0}", e.Progress);
                }
            };

            //var items = new string[] { "Venus", "Saturn", "Neptun" };


            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.planets_array, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            switch1.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs e) {
                var toast = Toast.MakeText(this, "Your answer is " +
                (e.IsChecked ? "correct" : "incorrect"), ToastLength.Short);
                toast.Show();
            };

            // Create your application here
        }
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            string toast = string.Format("The planet is {0}", spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(this, toast, ToastLength.Long).Show();
        }



}
}