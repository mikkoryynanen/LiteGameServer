using System;

namespace Shared.Models
{
    [Serializable] // TODO This is due to this object being send over the wire, Make DTO and send that
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}