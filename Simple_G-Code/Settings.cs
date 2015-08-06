using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Db = Autodesk.AutoCAD.DatabaseServices;


namespace Simple_G_Code
{
    
        //const string podachaXY = " F1000"; //Скорость подачи для осей XY
        //const string podachaZ = " F200"; //скорость подачи для оси Z
        //const double arcAngleSegm = 5; //угол сегмента при апроксимации кривой (5 градусов)
        //const string path = "D:\\";  //Папка для сохранения файлов g-code
        //const string Z_feed = "0";  //рабочий уровень инструмента
        //const string Z_seed = "3";  // уровень перемещения инструмента
        //const int round = 3;  // уровень jrheuktybz  
    
    
    public class Settings
    {
        private Db.LineWeight _weight = Db.LineWeight.LineWeight009;
        private int _colorInd = 10;
        private string _layerName = "xline";

        public Settings()
        {
        }

        public Settings(int colorInd, Db.LineWeight weight, string layerName)
        {
            this.colorInd = colorInd;
            this.weight = weight;
            this.layerName = layerName;
        }



        public int colorInd
        {
            get { return _colorInd; }
            set { _colorInd = value; }
        }
        public Db.LineWeight weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        public string layerName
        {
            get { return _layerName; }
            set { _layerName = value; }
        }



        public Settings getParam()
        {

            string path = Assembly.GetExecutingAssembly().Location;
            FileInfo fi1 = new FileInfo(path);
            path = fi1.Directory.ToString() + "\\MySettings.xml";
            fi1 = null;

            Settings myObject = default(Settings);
            if (System.IO.File.Exists(path))
            {
                try
                {
                    XmlSerializer mySerializer = new XmlSerializer(typeof(Settings));
                    using (FileStream myFileStream = new FileStream(path, FileMode.Open))
                    {
                        myObject = (Settings)mySerializer.Deserialize(myFileStream);
                    }

                }
                catch (System.InvalidOperationException ex)
                {
                    System.IO.File.Delete(path);
                    myObject = new Settings(256, Db.LineWeight.LineWeight009, "xline");
                }

            }
            else
            {
                myObject = new Settings(256, Db.LineWeight.LineWeight009, "xline");
            }
            return myObject;
        }

        public void saveParam(Settings Setting)
        {
            string path = Assembly.GetExecutingAssembly().Location;
            FileInfo fi1 = new FileInfo(path);
            path = fi1.Directory.ToString() + "\\MySettings.xml";
            fi1 = null;

            XmlSerializer ser = new XmlSerializer(typeof(Settings));
            using (TextWriter writer = new StreamWriter(path, false))
            {
                ser.Serialize(writer, Setting);
            }
        }







    }
}
