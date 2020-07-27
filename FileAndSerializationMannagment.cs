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
        public static void SerializeAndSaveToFile()
        {
            string FileName;
            string JsonString;
            FolderBrowserDialog FileDialog = new FolderBrowserDialog();
            SerializerDataFormat DataToSerialize = new SerializerDataFormat();//initializing variables and objekts
            DataToSerialize.UpdateTime = DateTime.Now;
            DataToSerialize.VisitedPlaces = GlobalVariables.VisitedZones;
            if(FileDialog.ShowDialog() == DialogResult.OK)//asks user to select a directory path
            {
                FileName = FileDialog.SelectedPath;
            }
            else//should the user cancle the selection the method returns
            {
                return;
            }
            JsonString = JsonSerializer.Serialize(DataToSerialize);//turns the info into json
            FileName = FileName + "/" + GlobalVariables.LastUpdateTime.Hour + "_" + GlobalVariables.LastUpdateTime.Minute +"_"+GlobalVariables.LastUpdateTime.Second + "AlbionRoadsData.txt";//adds the name of the file
            File.WriteAllText(FileName , JsonString);//writes the file at the specified location
        }
        public static void LoadFromFileAndDeserialize()
        {
            string FileName;
            string JsonString;//initializing variables and objekts
            Microsoft.Win32.OpenFileDialog FileDialog = new Microsoft.Win32.OpenFileDialog();
            FileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";//setting filter
            if(FileDialog.ShowDialog() == true)//asking user to select a txt file
            {
                FileName = FileDialog.FileName;
            }
            else//should the user cancel the selection the method returns
            {
                return;
            }
            JsonString = File.ReadAllText(FileName);//reads the file
            SerializerDataFormat DeserializedData = new SerializerDataFormat();//creates objekt for the deserialized data
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
