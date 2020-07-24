using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Threading;
using System.Security.Policy;

namespace albion_avalon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();//loads the window
            System.Timers.Timer updatetimer = new System.Timers.Timer(globalvariables.updatetime);//creates a timer for regular updates
            updatetimer.AutoReset = true;//makes timer loop
            updatetimer.Elapsed += updatetimerevent;//attaches eventhandler
            updatetimer.Start();//starts the timer
        }

        private void updatetimerevent(object sender, ElapsedEventArgs e)//eventhandler which is called by the timer
        {
            foreach(albionzonedefinition zone in globalvariables.visitedzones)//goes through all saved zones 
            {
                foreach(albionportaldefinition portal in zone.connectedzones)//goes through all portals in those zones
                {
                    portal.minutestilldecay -= globalvariables.updatetime / 60000;//decreases the time until the portal despawns by the apropriate amount
                }
            }
        }
    }
}
