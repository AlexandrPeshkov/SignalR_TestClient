using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client_SignalR
{
    /// <summary>
    /// SignalR клиент. Получение координат техники
    /// </summary>
    class SignalRTestHub
    {
        /// <summary>
        /// Клиентский ХАБ
        /// </summary>
        private HubConnection ClientHub { get; set; }

        /// <summary>
        /// Имя метода для удаленного вызова
        /// задан в  \Objects-WebApi\Objects.DM\Structs\SignalRSendMethods.cs
        /// </summary>
        private readonly string RemoteEventName = "objectchanged";

        /// <summary>
        /// Делегат процедуры обратного вызова
        /// </summary>
        protected Action<MapObjectChangedEventArgs> OnObjectChanged { get; set; }

        /// <summary>
        /// Состояние подключения хаба к серверу
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Создание хаба
        /// </summary>
        /// <param name="URL"></param>
        public SignalRTestHub(string URL = "http://localhost:49402/signalr")
        {
            ClientHub = new HubConnectionBuilder()
              .WithUrl(URL)
              .Build();

            IsConnected = false;

            ///Метод обратного вызова на каждое обновление данных
            OnObjectChanged = OnObjectChangedCallBack;

            ///Подписки
            ClientHub.On<MapObjectChangedEventArgs>(RemoteEventName, OnObjectChanged);
            ClientHub.Closed += OnClientHubClosed;
        }

        public void Start()
        {
            TryConnect();

            while (true)
            {
                //Просто ждем данные от WebApi
            }
        }

        private Task OnClientHubClosed(Exception arg)
        {
            //Меняем статус на "отключен"
            System.Diagnostics.Debug.WriteLine("Was DISconnected");
            return new Task(() => IsConnected = false);

        }

        /// <summary>
        /// Попытка подключить хаб
        /// </summary>
        private async void TryConnect()
        {
            try
            {
                await ClientHub.StartAsync();
                IsConnected = true;
                System.Diagnostics.Debug.WriteLine("Was connected");
            }
            catch (Exception exception)
            {
                IsConnected = false;
                System.Diagnostics.Debug.WriteLine(exception.Message);
                throw new Exception();
            }
        }

        /// <summary>
        /// Вариант реализации callback обработки
        /// </summary>
        /// <param name="Data"> 
        /// Список техники 
        /// </param>
        private void OnObjectChangedCallBack(MapObjectChangedEventArgs Data)
        {
            var EventArgs = Data as MapObjectChangedEventArgs;

            if(EventArgs!=null)
            {

                JArray jArray = (JArray)EventArgs.Parameters;
                Console.WriteLine("New pack -----------------------------");
                foreach(var elem in jArray)
                {
                    Console.WriteLine(""
                       + $"ID { elem.ToObject<VehicleDTO>().Id }"
                       + $" [ { elem.ToObject<VehicleDTO>().Point.X} ;"
                        + $"{ elem.ToObject<VehicleDTO>().Point.Y} ]"
                         + $"IsActive: { elem.ToObject<VehicleDTO>().IsIgnitionActive}"
                        );
                }
            }
        }
    }
}
