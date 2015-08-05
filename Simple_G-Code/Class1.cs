using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using App = Autodesk.AutoCAD.ApplicationServices;
using cad = Autodesk.AutoCAD.ApplicationServices.Application;
using Db = Autodesk.AutoCAD.DatabaseServices;
using Ed = Autodesk.AutoCAD.EditorInput;
using Gem = Autodesk.AutoCAD.Geometry;
using Rtm = Autodesk.AutoCAD.Runtime;

[assembly: Rtm.CommandClass(typeof(Simple_G_Code.Commands))]

namespace Simple_G_Code
{
    public class Commands
    {

        const string podachaXY = " F1000"; //Скорость подачи для осей XY
        const string podachaZ = " F200"; //скорость подачи для оси Z
        const double arcAngleSegm = 5; //угол сегмента при апроксимации кривой (5 градусов)
        const string path = "D:\\";  //Папка для сохранения файлов g-code
        const string Z_feed = "0";  //рабочий уровень инструмента
        const string Z_seed = "1";  // уровень перемещения инструмента


        [Rtm.CommandMethod("Get_GCode_from_pline")]
        static public void Get_GCode_from_pline()
        {
            // Получение текущего документа и базы данных
            App.Document acDoc = App.Application.DocumentManager.MdiActiveDocument;
            Db.Database acCurDb = acDoc.Database;
            Ed.Editor acEd = acDoc.Editor;


            //Ed.PromptEntityOptions opt = new Ed.PromptEntityOptions("\nВыбери плинию:");
            //Ed.PromptEntityResult rez = acEd.GetEntity(opt);
            //if (rez.Status != Ed.PromptStatus.OK)
            //    return;


            //Db.TypedValue[] acTypValAr=new Db.TypedValue[1];
            Db.TypedValue[] acTypValAr = new Db.TypedValue[1] { new Db.TypedValue(0, "LWPOLYLINE,POLYLINE") };
            //acTypValAr.SetValue(new Db.TypedValue((int)Db.DxfCode.Start, "PLINE"), 0);
            Ed.SelectionFilter acSelFtr = new Ed.SelectionFilter(acTypValAr);
            Ed.PromptSelectionResult acSSPrompt = acDoc.Editor.GetSelection(acSelFtr);
            if (acSSPrompt.Status != Ed.PromptStatus.OK)
                return;

            Ed.SelectionSet acSSet = acSSPrompt.Value;


           
            //string podachaXY = " F2500";
            //string podachaZ = " F200";
            string gcode = "\nG0 X0 Y0 " + podachaXY;

            bool down = true;
            // старт транзакции
            using (Db.Transaction acTrans = acCurDb.TransactionManager.StartOpenCloseTransaction())
            {
                // Открытие таблицы Блоков для чтения
                Db.BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, Db.OpenMode.ForRead) as Db.BlockTable;

                // Открытие записи таблицы Блоков пространства Модели для записи
                Db.BlockTableRecord acBlkTblRec = acTrans.GetObject(acBlkTbl[Db.BlockTableRecord.ModelSpace],
                                                                                Db.OpenMode.ForWrite) as Db.BlockTableRecord;

            
                foreach (Ed.SelectedObject acSSObj in acSSet)
                {

                    Db.Polyline acPl = acTrans.GetObject(acSSObj.ObjectId, Db.OpenMode.ForRead) as Db.Polyline;
                    
                    
                    for (int i = 0; i < acPl.NumberOfVertices; i++)
                    {

                        Gem.Point2d p2d = acPl.GetPoint2dAt(i);
                        string x = Math.Round(p2d.X, 2).ToString().Replace(',', '.');
                        string y = Math.Round(p2d.Y, 2).ToString().Replace(',', '.');

                        gcode = gcode + "\nG0 X" + x + " Y" + y + podachaXY;

                        // Создание отрезка начинающегося в 0,0 и заканчивающегося в 5,5
                        Db.DBPoint acP = new Db.DBPoint(new Gem.Point3d(p2d.X,p2d.Y,0));
                        acP.SetDatabaseDefaults();
                        acBlkTblRec.AppendEntity(acP);
                        acTrans.AddNewlyCreatedDBObject(acP, true);                   
                        
                        if (down)
                        {
                            gcode = gcode + "\nG1 Z" + Z_feed + podachaZ;
                            down = false;
                        }


                        switch (acPl.GetSegmentType(i))
                        {
                            case Db.SegmentType.Arc:

                               Gem.CircularArc3d arc3D = acPl.GetArcSegmentAt(i);
                               double delParam = (double)0.1;

                                double angle_1 = arc3D.StartAngle * (180 / Math.PI);
                                double angle_2 = arc3D.EndAngle * (180 / Math.PI);
                                    //Сигменты с сектором не больше 5 градусов
                                delParam = 1 / (Math.Abs(angle_2 - angle_1) / arcAngleSegm);


                                for (Double curParam = i + delParam; (curParam + delParam) < i + 1; curParam += delParam)
                                    {

                                        Gem.Point3d curPt = acPl.GetPointAtParameter(curParam);
                                        x = Math.Round(curPt.X, 2).ToString().Replace(',', '.');
                                        y = Math.Round(curPt.Y, 2).ToString().Replace(',', '.');

                                        gcode = gcode + "\nG1 X" + x + " Y" + y + podachaXY;

                                        Db.DBPoint acPArc = new Db.DBPoint(curPt);
                                        acPArc.SetDatabaseDefaults();
                                        acBlkTblRec.AppendEntity(acPArc);
                                        acTrans.AddNewlyCreatedDBObject(acPArc, true);  
                                    
                                        //Gem.Point3d curPt =  acPl.GetPointAtParameter(curParam);
                                    
                                        //curParam += delParam;
                                    }
                                    break;
                            case Db.SegmentType.Line:
                                Gem.LineSegment3d line3D = acPl.GetLineSegmentAt(i);
                                //acEd.WriteMessage("\nCenter: {0}, radius: {1}", arc3D.Center, arc3D.Radius);
                                break;
                        }


                    }

                    gcode = gcode + "\nG1 Z" + Z_seed + podachaZ;
                    down = true;
                }


                acTrans.Commit();
            }
            //gcode = gcode + "\nG1 Z1" + podachaZ;
            gcode = gcode + "\nG0 X0 Y0" + podachaXY;
            gcode = gcode + "\nM30";

            //string filename = ".";
            Ed.PromptStringOptions optStr = new Ed.PromptStringOptions("\nИмя файла:");
            Ed.PromptResult rezStr = acEd.GetString(optStr);
            if (rezStr.Status != Ed.PromptStatus.OK)
                return;

            string filename = path + rezStr.StringResult + ".nc";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filename, true))
            {
                //file.WriteLine("Fourth line");
                file.Write(gcode);
            }
        }
    }
}
