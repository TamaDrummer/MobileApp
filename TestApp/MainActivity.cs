using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Runtime;
using System.Diagnostics;
using System.Timers;
using WebSocketSharp;

namespace TestApp
{
    [Activity(Label = "TestApp", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private bool btnClicked = false;
        private bool msgReceived = false;
        private bool connected = false;
        bool gpiobuttonpressed = false;

      
        RaspData raspdata;
        JsonData jsondata;

        TextView tvRaspTemp;
        TextView tvRaspHum;
        TextView tvRaspSysData;
        Timer timer;
        Button btngpio;
        Button btnSensorData;
        Button btnRaspDisconnect;
        Button btnRaspConnect;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Just for Testing

            raspdata = new RaspData();
            /*raspdata.Connect();
            raspdata.Disconnect();
            raspdata.sendMessage("Ich bin es ");
            raspdata.getMessage();*/

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Button showPopupMenu = FindViewById<Button>(Resource.Id.popupbutton);
            Button nextMenuButton = FindViewById<Button>(Resource.Id.nextmenubutton);

            btnRaspConnect = FindViewById<Button>(Resource.Id.buttonConnect);
            btnRaspDisconnect = FindViewById<Button>(Resource.Id.buttonDisconnect);
            btnSensorData = FindViewById<Button>(Resource.Id.buttonSensorData);
            btngpio = FindViewById<Button>(Resource.Id.buttongpio);

            tvRaspHum = FindViewById<TextView>(Resource.Id.textViewHumidity);
            tvRaspTemp = FindViewById<TextView>(Resource.Id.textViewTemperature);
            tvRaspSysData = FindViewById<TextView>(Resource.Id.textViewSysData);




            btnRaspConnect.Click += (s,arg) =>
            {
                Task.Factory.StartNew(ConnectToRasp);
            };

            btnRaspDisconnect.Click += (s, arg) =>
            {
                if(raspdata.connected)
                    raspdata.Disconnect();

                else 
                {
                    btngpio.Enabled = false;
                }
            };

            btnSensorData.Click += (s, arg) =>
            {
                
                btnClicked = !btnClicked;
                //raspdata.sendMessage("hum");

            };

            btngpio.Click += (s, arg) =>
            {
                gpiobuttonpressed = !gpiobuttonpressed;
                if (gpiobuttonpressed)
                {
                    raspdata.sendMessage("gpioon");
                    btngpio.Text = "GPIO OFF";
                }
                else
                {
                    raspdata.sendMessage("gpiooff");
                    btngpio.Text = "GPIO ON";
                }
                    
                

                
            };

            raspdata.OnMessageReceived += a_MessageReceived;
            raspdata.OnErrorDetected += a_ErrorDetected;
            raspdata.OnConnection += a_connected;
            raspdata.OnDisconnection += a_disconnected;

             

            showPopupMenu.Click += (s, arg) =>
            {
                PopupMenu menu = new PopupMenu(this, showPopupMenu);
                menu.Inflate(Resource.Menu.popMenu);
                menu.Show();
            };

            nextMenuButton.Click += (s, arg) =>
            {
                
                StartActivity(typeof(SubActivity));

            };
            //var alarmIntent = new Intent(this, typeof(BackgroundReceiver));
            //var pending = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
            //var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();
            //alarmManager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 3 * 1000, pending);


        }



       protected override void OnStart()
        {
            /*Task.Run(async() =>
            {
                await Task.Delay(1000);

            });*/



            //Task.Delay(1000);

            base.OnStart();

        }

        protected override void OnResume()
        {
            base.OnResume();
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();


        }
        //alarmManager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 3 * 1000, pending);

           
        private void ConnectToRasp()
        {

                raspdata.Connect();
              

                if (raspdata.connectionError || raspdata.connected == false)
                {
                    RunOnUiThread(() =>
                    {
                        btngpio.Enabled = false;
                        btnRaspDisconnect.Enabled = false;
                        btnSensorData.Enabled = false;

                    });



                }
                else if (raspdata.connected)
                {
                    RunOnUiThread(() =>
                    {
                        btnSensorData.Enabled = true;
                        btnRaspDisconnect.Enabled = true;
                        btngpio.Enabled = true;
                    });
                }
 
            
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.optionsMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if(item.ItemId == Resource.Id.file_settings)
            {
                //do something here
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (btnClicked && raspdata.connected)
            {


                Task.Run(async () =>
                {
                    //raspdata.sendMessage("video");
                    await Task.Delay(1000);
                    raspdata.sendMessage("sysinfo");
                    /*await Task.Delay(1000);
                    raspdata.sendMessage("gpioon");
                    await Task.Delay(1000); 
                    raspdata.sendMessage("hum");
                    await Task.Delay(1000);
                    raspdata.sendMessage("temp");
                    await Task.Delay(1000);
                    raspdata.sendMessage("gpiooff");
                    await Task.Delay(1000);*/

                });

            }
        }

        public void a_ErrorDetected(object sender, EventArgs e)
        {
            connected = false;
            RunOnUiThread(() =>
            {
                btngpio.Enabled = false;
                btnRaspDisconnect.Enabled = false;
                btnSensorData.Enabled = false;

            });
        }

        public void a_connected(object sender, EventArgs e)
        {
            connected = true;
            RunOnUiThread(() =>
            {
                 btnSensorData.Enabled = true;
                 btnRaspDisconnect.Enabled = true;
                 btngpio.Enabled = true;
            });
            

        }
        public void a_disconnected(object sender, EventArgs e)
        {
            RunOnUiThread(() =>
            {
                btnSensorData.Enabled = false;
                btnRaspDisconnect.Enabled = false;
                btngpio.Enabled = false;
            });
        }



        public void a_MessageReceived(object sender, EventArgs e)
        {
            msgReceived = true;
            if (msgReceived)
            {
                var message = raspdata.actualMessage;
                if (message.Contains("Temp"))
                {
                    RunOnUiThread(() =>
                    {
                        tvRaspTemp.Text = message;
                    });
                    
                }
                else if (message.Contains("Hum"))
                {
                    RunOnUiThread(() =>
                    {
                        tvRaspHum.Text = message;
                    });
                    
                }
                else if (message.Contains("Sysinfo"))
                {


                    //string hum = data[""][""];


                    RunOnUiThread(() =>
                    {
                        tvRaspSysData.Text = message;
                    });
                }
            }
        }
    }


    
}

