using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace albion_avalon
{
    class TimerManagment
    {
        public void InitializeTimer()
        {
            System.Timers.Timer UpdateTimer = new System.Timers.Timer(GlobalVariables.UpdateTime);//creates a timer for regular updates
            UpdateTimer.AutoReset = true;//makes timer loop
            UpdateTimer.Elapsed += UpdateTimerEvent;//attaches eventhandler
            UpdateTimer.Start();//starts the timer
        }
        private void UpdateTimerEvent(object sender, ElapsedEventArgs e)//eventhandler which is called by the timer
        {
            foreach (AlbionZoneDefinition Zone in GlobalVariables.VisitedZones)//goes through all saved zones 
            {
                foreach (AlbionPortalDefinition Portal in Zone.ConnectedZones)//goes through all portals in those zones
                {
                    if(Portal.MinutesTillDecay <= 0)
                    {
                        Portal.MinutesTillDecay = 0;
                        continue;
                    }
                    Portal.MinutesTillDecay -= GlobalVariables.UpdateTime / 60000;//decreases the time until the portal despawns by the apropriate amount
                }
            }
        }
    }
}
