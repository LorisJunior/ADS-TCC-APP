using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TCCApp.Model
{
    public class Message
    {
        public string Author { get; set; }
        public string Content { get; set; }
        public string UserKey { get; set; }
    }
    public class OutboundMessage : Message
    {
        [JsonProperty("Timestamp")]
        public ServerTimeStamp TimestampPlaceholder { get; } = new ServerTimeStamp();
    }

    /// <summary>
    /// Inbound message has <see cref="Timestamp"/> in a form of a UNIX timestamp.
    /// </summary>
    public class InboundMessage : Message
    {
        public long Timestamp { get; set; }
    }

    public class ServerTimeStamp
    {
        [JsonProperty(".sv")]
        public string TimestampPlaceholder { get; } = "timestamp";
    }
}
