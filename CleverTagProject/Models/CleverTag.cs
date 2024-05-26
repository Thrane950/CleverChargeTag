using System.Text.Json.Serialization;

namespace CleverTagProject.Models
{
    public class CleverTag
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string? PlateNum { get; set; }//License Plate Number

        public string Tag { get; set; }//"dk-1234567890-clever"

        public long Rfidnum { get; set; }//Number on rfid tag from default
        [JsonIgnore]
        public bool Isblocked { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.Now;


    }
}
