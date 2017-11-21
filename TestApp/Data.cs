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

namespace TestApp
{
    public class RaspData
    {
        public event EventHandler OnMessageReceived;
        const string host = "ws://192.168.178.52:8181/mycode2";
        private WebSocket client = new WebSocket(host);
        public bool connected { get; set; }
        public bool disconnected { get; set; }

        public string actualMessage { get; set; }

        public RaspData()
        {
            this.connected = false;
            this.disconnected = false;

            client.OnOpen += (ss, ee) =>
            this.connected = true;

            client.OnMessage += (ss, ee) =>
            {
                this.actualMessage = ee.Data;
                msgReceived();
                
            };

            client.OnClose += (ss, ee) =>
            this.disconnected = true;
           
        }

        public void Connect()
        { 
            client.Connect();    
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
        

    }
}