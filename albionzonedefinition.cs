using System;
using System.Collections.Generic;
using System.Text;

namespace albion_avalon
{
    public class AlbionZoneDefinition
    {
        public string ZoneName { get; set; }//name of the zone
        public List<AlbionPortalDefinition> ConnectedZones { get; set; }//list containing one item for each portal in the zone
        public int ZoneID { get; set; }//id for the zone
    }
    public class AlbionPortalDefinition
    {
        public string ConnectedZone { get; set; }//the name of the zone the portal leads to
        public DateTime DespawnTime { get; set; }//how many more minutes the portal is going to exist before it despawns/gets replaced
        public int PortalID { get; set; }//id of the portal
    }
    public class SerializerDataFormat
    {
        public int ZoneIdCounter { get; set; }
        public List<AlbionZoneDefinition> VisitedPlaces { get; set; }
    }
}
