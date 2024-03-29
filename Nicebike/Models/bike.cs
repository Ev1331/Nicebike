﻿namespace Nicebike.Models
{
    public class Bike
    {
        public int id { get; set; }
        public string color { get; set; }
        public string type { get; set; }
        public string size { get; set; }
        public string reference { get; set; }
        public int technician { get; set; }
        public int bikeModelId { get; set; }
        public string status { get; set; }
        public string bikeModelName { get; set; }
        public int bikePrice { get; set; }
        public string dateProduction { get; set; }
        public Bike(int id, string color, string type, string size, string reference, int technician, int bikeModelId, string status, string bikeModelName, int bikePrice, string dateProduction)
        {
            this.id = id;
            this.color = color;
            this.type = type;
            this.size = size;
            this.reference = reference;
            this.technician = technician;
            this.bikeModelId = bikeModelId;
            this.status = status;
            this.bikeModelName = bikeModelName;
            this.bikePrice = bikePrice;
            this.dateProduction = dateProduction;
        }
    }
}