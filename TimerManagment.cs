﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace albion_avalon
{
    public class TimerManagment
    {
        System.Timers.Timer UpdateTimer;
        public void InitializeTimer()
        {
            UpdateTimer = new System.Timers.Timer(GlobalVariables.TimerInterval);//creates a timer for regular updates
            UpdateTimer.AutoReset = true;//makes timer loop
            UpdateTimer.Elapsed += UpdateTimerEvent;//attaches eventhandler
            UpdateTimer.Start();//starts the timer
        }
        public void StopTimer()//method to get read of the timer to release the ressources
        {
            UpdateTimer.Dispose();
        }
        private void UpdateTimerEvent(object sender, ElapsedEventArgs e)//eventhandler which is called by the timer
        {
            foreach (AlbionZoneDefinition Zone in GlobalVariables.VisitedZones)//goes through all saved zones 
            {
                foreach (AlbionPortalDefinition Portal in Zone.ConnectedZones)//goes through all portals in those zones
                {
                    if(Portal.MinutesTillDecay <= 0)//if time is less than or equal to 0 it gets set to 0 and not decreased
                    {
                        Portal.MinutesTillDecay = 0;
                        continue;
                    }
                    Portal.MinutesTillDecay -= GlobalVariables.TimerInterval / 60000;//decreases the time until the portal despawns by the apropriate amount
                }
            }
            GlobalVariables.LastUpdateTime = DateTime.Now;
        }
        public static void UpdateTime(DateTime LastUpdate,List<AlbionZoneDefinition> ZoneDefinitions)
        {
            if (ZoneDefinitions == null) return;
            TimeSpan Difference = DateTime.Now - LastUpdate;//callculates the time that passed since the last update
            foreach (AlbionZoneDefinition Zone in ZoneDefinitions)//goes through all saved zones 
            {
                foreach (AlbionPortalDefinition Portal in Zone.ConnectedZones)//goes through all portals in those zones
                {
                    if (Portal.MinutesTillDecay <= 0)//if time is less than or equal to 0 it gets set to 0 and not decreased
                    {
                        Portal.MinutesTillDecay = 0;
                        continue;
                    }
                    Portal.MinutesTillDecay -= Difference.TotalMinutes;//decreases the time until the portal despawns by the apropriate amount
                    Portal.MinutesTillDecay = Math.Round(Portal.MinutesTillDecay);
                }
            }
        }
    }
}
