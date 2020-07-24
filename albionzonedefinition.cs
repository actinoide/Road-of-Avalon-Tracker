using System;
using System.Collections.Generic;
using System.Text;

namespace albion_avalon
{
    public class AlbionZoneDefinition
    {
        public string ZoneName { get; set; }//name of the zone
        public List<AlbionPortalDefinition> ConnectedZones { get; set; }//list containing one item for each portal in the zone
    }
    public class AlbionPortalDefinition
    {
        public string ConnectedZone { get; set; }//the name of the zone the portal leads to
        public double MinutesTillDecay { get; set; }//how many more minutes the portal is going to exist before it despawns/gets replaced
    }
}
