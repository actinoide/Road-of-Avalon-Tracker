using System;
using System.Collections.Generic;
using System.Text;

namespace albion_avalon
{
    public static class GlobalVariables
    {
        public static List<AlbionZoneDefinition> VisitedZones = new List<AlbionZoneDefinition>();//list of zones 
        public static uint ZoneIDCounter = 0;//counter for zone ids
        public static bool IsInAutoDeleteMode = false;// bool for auto delete mode
    }
}
