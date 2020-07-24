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
            if (GlobalVariables.IsInTimerMode) 
            {
                GlobalVariables.Timer.InitializeTimer();//starts the timer
            }
            UIUpdater();//loads ui
            Activated += MainWindowActivated;//attaches an eventhandler to the activated event
        }

        private void MainWindowActivated(object sender, EventArgs e)
        {
            UIUpdater();//updates the ui
            if (!GlobalVariables.IsInTimerMode) TimerManagment.UpdateTime();
        }

        public void UIUpdater()//call to update the ui
        {
            ListOfZones.Children.Clear();
            foreach (AlbionZoneDefinition Zone in GlobalVariables.VisitedZones)//goes through all zones
            {
                ListOfZones.Children.Add(new TextBlock {Text = Zone.ZoneName });//gives them each a textbox that lists their name
                foreach(AlbionPortalDefinition Portal in Zone.ConnectedZones)//then goes through all connected portals and lists their info
                {
                    ListOfZones.Children.Add(new TextBlock {Text= "   " + Portal.ConnectedZone + "    " + Portal.MinutesTillDecay + "minutes" });
                }
            }
        }

        private void SwitchUpdateMode(object sender, RoutedEventArgs e)
        {
            if (GlobalVariables.IsInTimerMode)
            {
                GlobalVariables.IsInTimerMode = false;//if in timer mode the timer gets diposed the button renamed and last update time set to now
                UpdateModeButton.Content = "switch to timer based update mode";
                GlobalVariables.Timer.StopTimer();
                GlobalVariables.LastUpdateTime = DateTime.Now;
            }
            else
            {
                GlobalVariables.IsInTimerMode = true;//if in update mode the timer gets created and started and the button renamed 
                UpdateModeButton.Content = "switch to foreground based update mode";
                GlobalVariables.Timer.InitializeTimer();
            }
        }

        private void FinnishInputButtonClick(object sender, RoutedEventArgs e)//event handler for input (attached to the button)
        {
            AlbionZoneDefinition ToBeAdded = new AlbionZoneDefinition();//creates new zone definition
            ToBeAdded.ConnectedZones = new List<AlbionPortalDefinition>();//adds the list 
            ToBeAdded.ZoneName = ZoneNameBox.Text;//sets the name
            for(int Counter = 1;Counter < 7; Counter++)//loops throuh all textboxes and reads their input
            {
                switch (Counter)
                {
                    case 1:
                        if (FirstPortalNameBox.Text == "") continue;//if there is nothing in the name box it continues
                        ToBeAdded.ConnectedZones.Add(ReadUserInput(FirstPortalNameBox.Text, FirstPortalDecayBox.Text));//otherwise it adds the item to the list
                        break;
                    case 2:
                        if (SecondPortalNameBox.Text == "") continue;
                        ToBeAdded.ConnectedZones.Add(ReadUserInput(SecondPortalNameBox.Text, SecondPortalDecayBox.Text));
                        break;
                    case 3:
                        if (ThirdPortalNameBox.Text == "") continue;
                        ToBeAdded.ConnectedZones.Add(ReadUserInput(ThirdPortalNameBox.Text, ThirdPortalDecayBox.Text));
                        break;
                    case 4:
                        if (FourthPortalNameBox.Text == "") continue;
                        ToBeAdded.ConnectedZones.Add(ReadUserInput(FourthPortalNameBox.Text, FourthPortalDecayBox.Text));
                        break;
                    case 5:
                        if (FifthPortalNameBox.Text == "") continue;
                        ToBeAdded.ConnectedZones.Add(ReadUserInput(FifthPortalNameBox.Text, FifthPortalDecayBox.Text));
                        break;
                    case 6:
                        if (SixthPortalNameBox.Text == "") continue;
                        ToBeAdded.ConnectedZones.Add(ReadUserInput(SixthPortalNameBox.Text, SixthPortalDecayBox.Text));
                        break;
                    default:
                        break;
                }
            }
            ZoneNameBox.Text = "";//resets all the textboxes to be empty
            FirstPortalNameBox.Text = "";
            FirstPortalDecayBox.Text = "";
            SecondPortalNameBox.Text = "";
            SecondPortalDecayBox.Text = "";
            ThirdPortalNameBox.Text = "";
            ThirdPortalDecayBox.Text = "";
            FourthPortalNameBox.Text = "";
            FourthPortalDecayBox.Text = "";
            FifthPortalNameBox.Text = "";
            FifthPortalDecayBox.Text = "";
            SixthPortalNameBox.Text = "";
            SixthPortalDecayBox.Text = "";
            GlobalVariables.VisitedZones.Add(ToBeAdded);//adds the created zone to the list of existing zones
            UIUpdater();//updates the ui to display the newly added data
        }
        private AlbionPortalDefinition ReadUserInput(string TargetLocationName , string PortalDecayTime)
        {
            double CurrentConversion;//creates variables for conversions
            DateTime CurrentTime;
            try//try catch for invalid inputs
            {
                CurrentTime = Convert.ToDateTime(PortalDecayTime);//converts from string to datetime
            }
            catch
            {
                CurrentTime = DateTime.MinValue;//if it cant convert it sets the datetime to its minvalue
            }
            CurrentConversion = CurrentTime.Hour * 60 + CurrentTime.Minute;//converts from minutes and hours to just minutes
            AlbionPortalDefinition CreatedPortal = new AlbionPortalDefinition();//creates a new portal definition
            CreatedPortal.MinutesTillDecay = CurrentConversion;//adds both the minutes calculated earlier and the string from when the method was called to the definition
            CreatedPortal.ConnectedZone = TargetLocationName;
            return CreatedPortal;//returns the definition
        }
    }
}
