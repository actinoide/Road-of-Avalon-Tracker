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
        public static void SerializeAndSaveToFile(bool IsInCloseMode)
        {
            string FileName;
            string JsonString;
            SerializerDataFormat DataToSerialize = new SerializerDataFormat();//initializing variables and objekts
            DataToSerialize.UpdateTime = DateTime.Now;
            DataToSerialize.VisitedPlaces = GlobalVariables.VisitedZones;
            if (!IsInCloseMode) //if were not in close mode we open a folder dialog
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
                FileName = FileName + "/" + DateTime.Now.ToString("H mm ss") + "AlbionRoadsData.json";//adds the name of the file
            }
            else//otherwise we just use appdata
            {
                FileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/AlbionAvalonianRoadAutoSaveData.json";//if in close mode we write to appdata
            }
            JsonString = JsonSerializer.Serialize(DataToSerialize);//turns the info into json
            File.WriteAllText(FileName , JsonString);//writes the file at the specified location
        }
        public static void LoadFromFileAndDeserialize(bool IsInAutoLoadMode)
        {
            string FileName;
            string JsonString;//initializing variables and objekts
            if (!IsInAutoLoadMode)
            { 
                Microsoft.Win32.OpenFileDialog FileDialog = new Microsoft.Win32.OpenFileDialog();
                FileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";//setting filter
                if (FileDialog.ShowDialog() == true)//asking user to select a json file
                {
                    FileName = FileDialog.FileName;
                }
                else//should the user cancel the selection the method returns
                {
                    return;
                }
            }
            else
            {
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/AlbionAvalonianRoadAutoSaveData.json"))
                {
                    FileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/AlbionAvalonianRoadAutoSaveData.json";
                }
                else
                {
                    return;
                }
            }
            JsonString = File.ReadAllText(FileName);//reads the file
            SerializerDataFormat DeserializedData;//creates objekt for the deserialized data
            try
            {
                DeserializedData = JsonSerializer.Deserialize<SerializerDataFormat>(JsonString);//deserializes the data into the objekt
            }
            catch//should the deserializer throw an exeption(for example becouse of invalid json) the method returns
            {
                return;
            }
            TimerManagment.UpdateTime(DeserializedData.UpdateTime, DeserializedData.VisitedPlaces);//updates the time on the loaded objekts
            if (DeserializedData.VisitedPlaces == null) return;
            foreach(AlbionZoneDefinition Zone in DeserializedData.VisitedPlaces)
            {
                GlobalVariables.VisitedZones.Add(Zone);//adds the objekts to the basic list
            }
        }
    }
}
