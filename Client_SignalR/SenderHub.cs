using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Linq;

namespace Client_SignalR
{
    class SenderHub
    {
        HubConnection connectionIN;

        public void StartHubs()
        {
            //WebApii
            connectionIN = new HubConnectionBuilder()
              .WithUrl("http://localhost:49402/signalr")
              .Build();

            ConnectIN();

            while (true)
            {

            }
           
        }

        public async void ConnectIN()
        {
            connectionIN.On<MapObjectChangedEventArgs>("objectchanged", objectchangedIN);

            try
            {
                await connectionIN.StartAsync();
                Console.WriteLine("Connetion success");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        //Слушаем ответы от ХАБА service->NotifyAll
        public async void objectchangedIN(MapObjectChangedEventArgs message)
        {

            JArray list = (JArray)message.Parameters;

            if (list != null)
            {
                Console.WriteLine("NEW PACK------------------");
                for (int i=0; i<list.Count; i++)
                {
                    //Вот тут надо их перенаправить
                    Console.WriteLine(list[i].ToObject<VehicleDTO>().Id);
                    //Вот тут надо их перенаправить
                    //updateVehicles(updateMethod, message);
                }
            }
            
        }
    }
}
