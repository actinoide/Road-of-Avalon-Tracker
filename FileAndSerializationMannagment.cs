using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace albion_avalon
{
    class FileAndSerializationMannagment
    {
        public static void SerializeAndSaveToFile(bool IsInCloseMode , bool IsInClipboardMode)
        {
            if (IsInClipboardMode && IsInCloseMode) return;//makes sure that only one condition is true
            string FileName = "";
            string JsonString;
            SerializerDataFormat DataToSerialize = new SerializerDataFormat();//initializing variables and objekts
            DataToSerialize.ZoneIdCounter = GlobalVariables.ZoneIDCounter;
            DataToSerialize.VisitedPlaces = GlobalVariables.VisitedZones;
            if (!IsInCloseMode && !IsInClipboardMode) //if were not in close mode or clipboard mode we open a folder dialog
            { 
                FolderBrowserDialog FileDialog = new FolderBrowserDialog();
                if (FileDialog.ShowDialog() == DialogResult.OK)//asks user to select a directory path
                {
                    FileName = FileDialog.SelectedPath;
                }
                else//should the user cancle the selection the method returns
                {
                    return;
                }
                FileName = FileName + "/" + DateTime.UtcNow.ToString("H mm ss") + "AlbionRoadsData.json";//adds the name of the file
            }
            else if(IsInCloseMode)//otherwise we just use appdata
            {
                FileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/AlbionAvalonianRoadAutoSaveData.json";//if in close mode we write to appdata
            }
            JsonString = JsonSerializer.Serialize(DataToSerialize);//turns the info into json
            if(IsInClipboardMode)//if it is in clipboard mode
            {
                Clipboard.SetData(DataFormats.Text, JsonString);//pushes the data into the clipboard
                return;
            }
            File.WriteAllText(FileName , JsonString);//writes the file at the specified location
        }
        public static void LoadFromFileAndDeserialize(bool IsInAutoLoadMode , bool IsInClipboardMode)
        {
            if (IsInAutoLoadMode && IsInClipboardMode) return;//makes sure that only one condition is true
            string FileName;
            string JsonString;//initializing variables and objekts
            if (!IsInAutoLoadMode && !IsInClipboardMode)
            { 
                Microsoft.Win32.OpenFileDialog FileDialog = new Microsoft.Win32.OpenFileDialog();
                FileDialog.Filter = "json files (*.json)|*.json";//setting filter
                if (FileDialog.ShowDialog() == true)//asking user to select a json file
                {
                    FileName = FileDialog.FileName;
                }
                else//should the user cancel the selection the method returns
                {
                    return;
                }
                JsonString = File.ReadAllText(FileName);//reads the file
            }
            else if(IsInAutoLoadMode)
            {
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/AlbionAvalonianRoadAutoSaveData.json"))
                {
                    FileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/AlbionAvalonianRoadAutoSaveData.json";
                }
                else
                {
                    return;
                }
                JsonString = File.ReadAllText(FileName);//reads the file
            }
            else//if it is in clipbord mode
            {
                JsonString = Clipboard.GetText(TextDataFormat.Text);//it takes the text from the clipboard
            }
            SerializerDataFormat DeserializedData;//creates objekt for the deserialized data
            try
            {
                DeserializedData = JsonSerializer.Deserialize<SerializerDataFormat>(JsonString);//deserializes the data into the objekt
            }
            catch//should the deserializer throw an exeption(for example becouse of invalid json) the method returns
            {
                return;
            }
            if(GlobalVariables.ZoneIDCounter < DeserializedData.ZoneIdCounter)//sets the idcounter to the higher one unless the new one is less than 50
            {
                if (!(GlobalVariables.ZoneIDCounter < 500))GlobalVariables.ZoneIDCounter = DeserializedData.ZoneIdCounter;
            }
            if (DeserializedData.VisitedPlaces == null) return;
            foreach(AlbionZoneDefinition Zone in DeserializedData.VisitedPlaces)
            {
                GlobalVariables.VisitedZones.Add(Zone);//adds the objekts to the basic list
            }
        }
    }
}
