using System;
using System.Collections.Generic;
using System.Text;

namespace albion_avalon
{
    public static class GlobalVariables
    {
        public static int UpdateTime = 60000;//time between updates in ms
        public static List<AlbionZoneDefinition> VisitedZones = new List<AlbionZoneDefinition>();//list of zones 
    }
}
