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
            UIUpdater();//loads ui
            Activated += MainWindowActivated;//attaches an eventhandler to the activated event
            Closed += MainWindowClosed;//attaches a eventhandler for when the app closes
            FileAndSerializationMannagment.LoadFromFileAndDeserialize(true);//loads autosave
        }

        private void MainWindowClosed(object sender, EventArgs e)//called when the window is about to close
        {
            FileAndSerializationMannagment.SerializeAndSaveToFile(true);//saves file when the program is about to close
        }

        private void MainWindowActivated(object sender, EventArgs e)
        {
            UIUpdater();//updates the ui
        }

        public void UIUpdater()//call to update the ui
        {
            ListOfZones.Children.Clear();
            foreach (AlbionZoneDefinition Zone in GlobalVariables.VisitedZones)//goes through all zones
            {
                Button TemporaryZoneButton = new Button { Content = Zone.ZoneName };
                TemporaryZoneButton.Tag = Zone.ZoneID;
                TemporaryZoneButton.Click += ZoneButtonClick;//creates a button and attaches an eventhandler
                ListOfZones.Children.Add(TemporaryZoneButton);
                foreach(AlbionPortalDefinition Portal in Zone.ConnectedZones)//then goes through all connected portals and lists their info
                {
                    Button TemporaryPortalButton = new Button { Content = Portal.ConnectedZone + "     " + Portal.DespawnTime , Margin = new Thickness(50, 0, 0, 0) };
                    TemporaryPortalButton.Tag = Portal.PortalID + "_" + Zone.ZoneID;
                    TemporaryPortalButton.Click += PortalButtonClick;//creates a button and attaches an eventhandler
                    ListOfZones.Children.Add(TemporaryPortalButton);
                }
            }
        }

        private void PortalButtonClick(object sender, RoutedEventArgs e)
        {
            Button Temporary = (Button)sender;//casts sender to button
            int Counter = 0;
            foreach (AlbionZoneDefinition Zone in GlobalVariables.VisitedZones.ToList())//goes through visitedzones
            {
                foreach (AlbionPortalDefinition Portal in Zone.ConnectedZones.ToList())//then through the portals in each zone
                {
                    if(Convert.ToString(Temporary.Tag) == Portal.PortalID + "_" + Zone.ZoneID)//checks if conent is equal to the values of sender
                    {
                        Zone.ConnectedZones.RemoveAt(Counter);//deletes the item from the list if it is
                        UIUpdater();//updates ui to reflect changes
                        return;
                    }
                    Counter++;
                }
                Counter = 0;
            }
            UIUpdater();//updates ui to reflect changes
        }

        private void ZoneButtonClick(object sender, RoutedEventArgs e)
        {
            Button Temporary = (Button)sender;//casts sender to button
            int Counter = 0;
            foreach(AlbionZoneDefinition Zone in GlobalVariables.VisitedZones.ToList())//goes through visitedzones 
            {
                if(Convert.ToInt32(Temporary.Tag) == Zone.ZoneID)//and checks if sender.content is equal to content 
                {
                    GlobalVariables.VisitedZones.RemoveAt(Counter);//deletes the item in the list if it is
                    UIUpdater();//updates ui to reflect changes
                    return;
                }
                Counter++;
            }
            UIUpdater();//updates the change to the ui
        }

        private void FinnishInputButtonClick(object sender, RoutedEventArgs e)//event handler for input (attached to the button)
        {
            AlbionZoneDefinition ToBeAdded = new AlbionZoneDefinition();//creates new zone definition
            ToBeAdded.ConnectedZones = new List<AlbionPortalDefinition>();//adds the list 
            ToBeAdded.ZoneName = ZoneNameBox.Text;//sets the name
            ToBeAdded.ZoneID = GlobalVariables.ZoneIDCounter;
            GlobalVariables.ZoneIDCounter++;
            for(int Counter = 1;Counter < 7; Counter++)//loops throuh all textboxes and reads their input
            {
                switch (Counter)
                {
                    case 1:
                        if (FirstPortalNameBox.Text == "") continue;//if there is nothing in the name box it continues
                        ToBeAdded.ConnectedZones.Add(ReadUserInput(FirstPortalNameBox.Text, FirstPortalDecayBox.Text,1));//otherwise it adds the item to the list
                        break;
                    case 2:
                        if (SecondPortalNameBox.Text == "") continue;
                        ToBeAdded.ConnectedZones.Add(ReadUserInput(SecondPortalNameBox.Text, SecondPortalDecayBox.Text,2));
                        break;
                    case 3:
                        if (ThirdPortalNameBox.Text == "") continue;
                        ToBeAdded.ConnectedZones.Add(ReadUserInput(ThirdPortalNameBox.Text, ThirdPortalDecayBox.Text,3));
                        break;
                    case 4:
                        if (FourthPortalNameBox.Text == "") continue;
                        ToBeAdded.ConnectedZones.Add(ReadUserInput(FourthPortalNameBox.Text, FourthPortalDecayBox.Text,4));
                        break;
                    case 5:
                        if (FifthPortalNameBox.Text == "") continue;
                        ToBeAdded.ConnectedZones.Add(ReadUserInput(FifthPortalNameBox.Text, FifthPortalDecayBox.Text,5));
                        break;
                    case 6:
                        if (SixthPortalNameBox.Text == "") continue;
                        ToBeAdded.ConnectedZones.Add(ReadUserInput(SixthPortalNameBox.Text, SixthPortalDecayBox.Text,6));
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
        private AlbionPortalDefinition ReadUserInput(string TargetLocationName , string PortalDecayTime,int PortalID)
        {
            DateTime CurrentTime;
            AlbionPortalDefinition CreatedPortal = new AlbionPortalDefinition();//creates a new portal definition
            try//try catch for invalid inputs
            {
                CurrentTime = Convert.ToDateTime(PortalDecayTime);//converts from string to datetime
            }
            catch
            {
                CurrentTime = DateTime.MinValue;//if it cant convert it sets the datetime to its minvalue
                CreatedPortal.DespawnTime = CurrentTime;//adds both the minutes calculated earlier and the string from when the method was called to the definition
                CreatedPortal.ConnectedZone = TargetLocationName;
                CreatedPortal.PortalID = PortalID;
                return CreatedPortal;//returns the definition
            }
            CurrentTime = DateTime.Now.AddHours(CurrentTime.Hour).AddMinutes(CurrentTime.Minute);//ads the given time to now
            CreatedPortal.DespawnTime = CurrentTime;//adds both the minutes calculated earlier and the string from when the method was called to the definition
            CreatedPortal.ConnectedZone = TargetLocationName;
            CreatedPortal.PortalID = PortalID;
            return CreatedPortal;//returns the definition
        }

        private void LoadFromFileButtonClick(object sender, RoutedEventArgs e)
        {
            FileAndSerializationMannagment.LoadFromFileAndDeserialize(false);//loads data from file
            UIUpdater();//updates ui
        }

        private void SaveToFileButtonClick(object sender, RoutedEventArgs e)
        {
            FileAndSerializationMannagment.SerializeAndSaveToFile(false);//saves data to file
        }
    }
}
