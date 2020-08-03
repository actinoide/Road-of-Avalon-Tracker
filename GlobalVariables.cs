using System;
using System.Collections.Generic;
using System.Text;

namespace albion_avalon
{
    public static class GlobalVariables
    {
        public static int TimerInterval = 60000;//time between updates in ms
        public static List<AlbionZoneDefinition> VisitedZones = new List<AlbionZoneDefinition>();//list of zones 
        public static bool IsInTimerMode = true;//bool to switch between timer and foreground based update modes
        public static DateTime LastUpdateTime = DateTime.UtcNow;//to keep track of the last time the update function was called on foreground update mode
        public static int ZoneIDCounter = 0;//counter for zone ids
    }
}
