using System;

namespace Client_SignalR
{
    /// <summary>
    /// Структура для точки(х,у) с типом double
    /// </summary>
    public struct Point
    {
        #region properties
        /// <summary>
        /// Координата x.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Координата y.
        /// </summary>
        public double Y { get; set; }

        #endregion

        #region constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="x">Координата x.</param>
        /// <param name="y">Координата y.</param>
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        #endregion

    }

    /// <summary>
    /// Тип оповещения
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// Никому
        /// </summary>
        None = 0,
        /// <summary>
        /// Всем кроме текущего пользователя
        /// </summary>
        ExceptCaller = 1,
        /// <summary>
        /// Всем
        /// </summary>
        All = 2
    }

    public class VehicleDTO
    {
        public int Id { get; set; }
        public Point Point { get; set; }

        public bool? IsIgnitionActive { get; set; }
    }

    /// <summary>
    /// Аргументы события изменения объекта карты
    /// </summary>
    public class MapObjectChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Псевдоним типа объекта
        /// </summary>
        public string ObjectTypeAlias { get; set; }
        /// <summary>
        /// Пользователь
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// Тип события
        /// </summary>
        public int ActionType { get; set; }
        /// <summary>
        /// Тип оповещения
        /// </summary>
        public NotificationType NotificationType { get; set; }
        /// <summary>
        /// Конструктор
        /// </summary>
        public MapObjectChangedEventArgs()
        {
            NotificationType = NotificationType.ExceptCaller;
        }
        /// <summary>
        /// Дополнительные параметры
        /// </summary>
        public object Parameters { get; set; }
    }
}
