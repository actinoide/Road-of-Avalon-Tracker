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
            FileAndSerializationMannagment.LoadFromFileAndDeserialize(true,false);//loads autosave
        }

        private void MainWindowClosed(object sender, EventArgs e)//called when the window is about to close
        {
            FileAndSerializationMannagment.SerializeAndSaveToFile(true,false);//saves file when the program is about to close
        }

        private void MainWindowActivated(object sender, EventArgs e)
        {
            UIUpdater();//updates the ui
        }

        public void UIUpdater()//call to update the ui
        {
            ListOfZones.Children.Clear();
            foreach (AlbionZoneDefinition Zone in GlobalVariables.VisitedZones.ToList())//goes through all zones
            {
                if (Zone.ConnectedZones.Count == 0 && GlobalVariables.IsInAutoDeleteMode)
                {
                    GlobalVariables.VisitedZones.Remove(Zone);//if the zone contains no portals and autodelete is on the zone gets deleted
                    continue;
                }
                Button TemporaryZoneButton = new Button { Content = Zone.ZoneName };
                TemporaryZoneButton.Tag = Zone.ZoneID;
                TemporaryZoneButton.Click += ZoneButtonClick;//creates a button and attaches an eventhandler
                ListOfZones.Children.Add(TemporaryZoneButton);
                foreach(AlbionPortalDefinition Portal in Zone.ConnectedZones.ToList())//then goes through all connected portals and lists their info
                {
                    Button TemporaryPortalButton = new Button { Content = Portal.ConnectedZone + "     " + Portal.DespawnTime , Margin = new Thickness(50, 0, 0, 0) };
                    TemporaryPortalButton.Tag = Portal.PortalID + "_" + Zone.ZoneID;
                    if (Portal.DespawnTime <= DateTime.UtcNow)
                    {
                        if (GlobalVariables.IsInAutoDeleteMode) Zone.ConnectedZones.Remove(Portal);//if the portal has ran out it gets deleted
                        TemporaryPortalButton.Foreground = Brushes.Blue;
                    }
                    else if (Portal.DespawnTime <= DateTime.UtcNow.AddMinutes(30)) TemporaryPortalButton.Foreground = Brushes.Red;
                    else if (Portal.DespawnTime <= DateTime.UtcNow.AddHours(1)) TemporaryPortalButton.Foreground = Brushes.Orange;
                    else TemporaryPortalButton.Foreground = Brushes.Green;//sets the color of the text depending on the time till decay
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
            CurrentTime = DateTime.UtcNow.AddHours(CurrentTime.Hour).AddMinutes(CurrentTime.Minute);//ads the given time to now
            CreatedPortal.DespawnTime = CurrentTime;//adds both the minutes calculated earlier and the string from when the method was called to the definition
            CreatedPortal.ConnectedZone = TargetLocationName;
            CreatedPortal.PortalID = PortalID;
            return CreatedPortal;//returns the definition
        }

        private void LoadFromFileButtonClick(object sender, RoutedEventArgs e)
        {
            FileAndSerializationMannagment.LoadFromFileAndDeserialize(false ,false);//loads data from file
            UIUpdater();//updates ui
        }

        private void SaveToFileButtonClick(object sender, RoutedEventArgs e)
        {
            FileAndSerializationMannagment.SerializeAndSaveToFile(false,false);//saves data to file
        }

        private void SearchTextBoxKeyUp(object sender,KeyEventArgs e)//eventhandler for when the user presses a key in the search box
        {
            SearchResultStackPanel.Children.Clear();//emptys stackpanel
            string Query = SearchTextBox.Text.ToLower();//reads the text in the searchbox and converts it to lowercase 
            bool WriteZone = false;//initializes variable
            if(Query.Length == 0)//if the length of the query is 0
            {
                SearchBorder.Visibility = Visibility.Collapsed;//the border gets hidden
                return;//and we return
            }
            SearchBorder.Visibility = Visibility.Visible;//otherwise the border is visible
            foreach(AlbionZoneDefinition Zone in GlobalVariables.VisitedZones.ToList())//goes through all zones
            {
                if (Zone.ZoneName.ToLower().Contains(Query)) WriteZone = true;//checks if their name contains the query and if  it does seets writezone to true
                foreach (AlbionPortalDefinition Portal in Zone.ConnectedZones.ToList())//goes through all portals in the zone
                {
                    if (Portal.ConnectedZone.ToLower().Contains(Query)) WriteZone = true;//and checks if they contain the query
                }
                if(WriteZone == true)//if something in this zone contained the query the entire zone with all portals is written to the stackpanel
                {
                    SearchResultStackPanel.Children.Add(new TextBlock { Text = Zone.ZoneName });
                    foreach (AlbionPortalDefinition Portal in Zone.ConnectedZones.ToList())
                    {
                        SearchResultStackPanel.Children.Add(new TextBlock { Text = Portal.ConnectedZone + "     " + Portal.DespawnTime, Margin = new Thickness(50, 0, 0, 0) });
                    }
                }
                WriteZone = false;//resets writezone for the next iteration of the loop
            }
        }

        private void LoadFromClipboardButtonClick(object sender, RoutedEventArgs e)
        {
            FileAndSerializationMannagment.LoadFromFileAndDeserialize(false, true);//loads data from clipboard
            UIUpdater();//updates the ui
        }

        private void SaveToClipboardButtonClick(object sender, RoutedEventArgs e)
        {
            FileAndSerializationMannagment.SerializeAndSaveToFile(false, true);//saves data to clipboard
        }

        private void AutoDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("this will imediately delete all expired portals and zones without portals. it will also delete any portals as soon as they expire and any zones as soon as they are empty. this cannot be undone. are you sure ?","warning",MessageBoxButton.YesNo) == MessageBoxResult.Yes)//asks user for confirmation
            {
                GlobalVariables.IsInAutoDeleteMode = true;//sets the auto delete variable to true
            }
            else
            {
                return;
            }
            UIUpdater();//updaes the ui
        }
    }
}
