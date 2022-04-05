using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Core.Persistence
{
    public class Orders
    {
        [Key]
        [Column("event_id")]
        public Guid EventId { get; set; }

        [Column("order_id")]
        public Guid OrderId { get; set; }

        [Column("payload", TypeName = "jsonb")]
        public object Payload { get; set; }
    }
}