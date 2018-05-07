using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cloud_MR
{
    public partial class DeserializingDataFromSensors
    {
        [JsonProperty("IMU")]
        public IMU IMU { get; set; }
        [JsonProperty("GPS")]
        public GPS GPS { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("time")]
        public int Time { get; set; }
    }
    public class IMU
    {
        [JsonProperty("gyroscope")]
        public float[] Gyroscope { get; set; }
        [JsonProperty("pressure")]
        public float Pressure { get; set; }
        [JsonProperty("accelerometer")]
        public float[] Accelerometer { get; set; }
        [JsonProperty("magnetometer")]
        public float[] Magnetometer { get; set; }
        [JsonProperty("temperature")]
        public float Temperature { get; set; }
    }
    public class GPS
    {
        [JsonProperty("gpsLat")]
        public float GpsLat { get; set; }
        [JsonProperty("gpsLon")]
        public float GpsLon { get; set; }
        [JsonProperty("gpsDate")]
        public float GpsDate { get; set; }
        [JsonProperty("gpsAlt")]
        public float GpsAlt { get; set; }
        [JsonProperty("gpsHDOP")]
        public float GpsHDOP { get; set; }
        [JsonProperty("gpsSat")]
        public float GpsSat { get; set; }

        [JsonProperty("gpsSpeed")]
        public float GpsSpeed { get; set; }
        [JsonProperty("gpsTime")]
        public float GpsTime { get; set; }
    }
    public partial class DeserializingDataFromSensors
    {
        public DeserializingDataFromSensors FromJson(string json)
        {
            return JsonConvert.DeserializeObject<DeserializingDataFromSensors>(json, Converter.Settings);
        }
    }
    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}