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
        HubConnection connectionOUT;

        string MethodName = "SendMapObjectsChangedMessage";
        string updateMethod = "MapObject_Changed";

        public void StartHubs()
        {
            //WebApii
            connectionIN = new HubConnectionBuilder()
              .WithUrl("http://localhost:49402/signalr")
              .Build();

            //BrowserMap Web
            connectionOUT = new HubConnectionBuilder()
            .WithUrl("http://localhost:49403/signalr")
            .Build();

            ConnectIN();
            //ConnectOUT();

            var Args = new MapObjectChangedEventArgs()
            {
                ActionType = 6,
                Id = 0,
                NotificationType = NotificationType.All,
                ObjectTypeAlias = "vehicles",
                Parameters = new List<VehicleDTO>()
                    {
                        new VehicleDTO()
                        {
                            Id = 236019374,
                            Point = new Point(
                                18415.486677775938,
                                6908.086287633304),
                            IsIgnitionActive = true
                        },
                    }
            };
            //RunMethod(MethodName, Args);

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

        public async void ConnectOUT()
        {
            try
            {
                await connectionOUT.StartAsync();
                Console.WriteLine("Resender tunnel connected");
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

        public async void RunMethod(string Method, MapObjectChangedEventArgs Args)
        {
            try
            {
                await connectionIN.InvokeAsync(Method, Args);
                Console.WriteLine("Send method " + Method);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async void updateVehicles(string Method, MapObjectChangedEventArgs Args)
        {
            try
            {
                await connectionOUT.InvokeAsync(Method, Args);
                Console.WriteLine("Update vehicle " + Method);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
