using beClean.Models;
using beClean.Views.Base;
using System.Collections.Generic;

namespace beClean.Views.RoomsPage
{
    public class RoomsPageVM : BaseVM
    {
        public IEnumerable<RoomObject> Rooms
        {
            get => Get<IEnumerable<RoomObject>>();
            set => Set(value);
        }
        public RoomsPageVM() : base("Комнаты")
        {
            Rooms = new List<RoomObject>(new[]
            {
                new RoomObject(){Id=1,Name="Гостинная", IconSource= "resource://beClean.Resources.Svg.rooms.bedroom.svg"},
                new RoomObject(){Id=2,Name="Прихожая", IconSource= "resource://beClean.Resources.Svg.rooms.hanger.svg"},
                new RoomObject(){Id=3,Name="Кухня", IconSource= "resource://beClean.Resources.Svg.rooms.kitchen.svg"},
                new RoomObject(){Id=3,Name="Спальня", IconSource= "resource://beClean.Resources.Svg.rooms.living-room.svg"},
            });
        }
    }
}
