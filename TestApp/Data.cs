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

using WebSocketSharp;
using System.Threading.Tasks;
using System.Linq;

namespace TestApp
{
    public class RaspData
    {
        public event EventHandler OnMessageReceived;
        public event EventHandler OnErrorDetected;
        public event EventHandler OnConnection;
        public event EventHandler OnDisconnection;

        JsonData jsondata;
  

        const string host = "ws://192.168.178.52:8181/mycode2";
        private WebSocket client = new WebSocket(host);
        public bool connected { get; set; }
        //public bool disconnected { get; set; }
        public bool connectionError { get; set; }
        public string connectionErrorMessage { get; set; }

        public string actualMessage { get; set; }

        public RaspData()
        {
            this.connected = false;
            //this.disconnected = false;

            client.OnOpen += (ss, ee) =>
            {
                OnConnection(this, EventArgs.Empty);
                this.connected = true;
                OnRaspConnection();

            };
            

            client.OnMessage += (ss, ee) =>
            {
                this.actualMessage = ee.Data;
                msgReceived();
                this.connected = true;
                dynamic data = null;
                Task.Factory.StartNew(() =>
                {
                    data = jsondata.DeSerializeJsonData(ee.Data);
                });
                

            };
            client.OnError += (ss, ee) =>
            {
               
                serverError(ee.Message);
                this.connected = false;
            };

            client.OnClose += (ss, ee) =>
            {
                this.connected = false;
                OnDisconnection(this, EventArgs.Empty);
            };

           
        }

        public void Connect()
        {
            
            this.connectionError = false;
            client.Connect();
            return;
 


        } 
        public void Disconnect()
        {
            client.Close();
        }

        public void sendMessage(string msg)
        {
            client.Send(msg);

        }
        public void msgReceived()
        {
           if(OnMessageReceived != null)
           {
                OnMessageReceived(this, EventArgs.Empty);
           } 
        }

        public void serverError(string error)
        {
            if(OnErrorDetected != null)
            {
                this.connectionError = true;
                this.connectionErrorMessage = error;
                OnErrorDetected(this, EventArgs.Empty);
                

            }

        }
        public void OnRaspConnection()
        {

        }
        

    }
}