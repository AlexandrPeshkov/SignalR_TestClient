using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client_SignalR
{
    /// <summary>
    /// SignalR клиент. Получение координат техники
    /// </summary>
    /// <typeparam name="T">
    /// Предпочтительный тип данных MapObjectChangedEventArgs
    /// \Objects-WebApi\Objects.BLL\Core\MapObjectChangedEventArgs.cs
    /// </typeparam>
    class SignalRTestHub<T> where T : EventArgs
    {
        /// <summary>
        /// Клиентский ХАБ
        /// </summary>
        private HubConnection ClientHub { get; set; }

        /// <summary>
        /// Состояние подключения хаба к серверу
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Имя метода для удаленного вызова
        /// задан в  \Objects-WebApi\Objects.DM\Structs\SignalRSendMethods.cs
        /// </summary>
        private readonly string RemoteEventName = "objectchanged";

        /// <summary>
        /// Делегат процедуры обратного вызова
        /// </summary>
        protected Action<T> OnObjectChanged { get; set; }

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
            ClientHub.On<T>(RemoteEventName, OnObjectChanged);

            ClientHub.Closed += OnClientHubClosed;
        }

        private System.Threading.Tasks.Task OnClientHubClosed(Exception arg)
        {
            return new System.Threading.Tasks.Task(() => IsConnected = false);
        }

        /// <summary>
        /// Попытка подключить хаб
        /// </summary>
        public async void TryConnect()
        {
            try
            {
                await ClientHub.StartAsync();
                IsConnected = true;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public void Start()
        {
            while (IsConnected)
            {
                //Просто ждем данные
            }
        }

        /// <summary>
        /// Реализация callback метода. Вызывается при получении новых данных 
        /// </summary>
        /// <param name="Data"> 
        /// </param>
        public void OnObjectChangedCallBack(T Data)
        {
            //to do
            //Парсим данные в зависимости от реализации клиента
            var EventArgs = Data as MapObjectChangedEventArgs;

            if(EventArgs!=null)
            {
                JArray jArray = (JArray)EventArgs.Parameters;

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
