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

namespace SimpleCAM
{
    class CAM_Main
    {

        /// <summary>
        /// Главный метод с которого стартует программа
        /// Тут лежит некоторый пример простейшего CAM:
        /// http://www.makercam.com/
        /// </summary>
        [Rtm.CommandMethod("bx_cam")]
        static public void bx_cam()
        {

        }


        /// <summary>
        /// Метод фрезерования оставляя контур контуру
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static string profileOperation(Db.Polyline curve)
        {
            string returnGCode="";

            //Итак нам понадобится
            // 1. Имя операции
            // 2. Диаметр инструмента (мм)
            // 3. Глубина на которую будем фрезеровать (мм)
            // 4. С какой стороны от профиля будем работать (снаружи/внутри)
            // 5. Безопасная высота инструмента (мм)
            // 6. Начальная рабочая плоскость (мм)
            // 7. Глубина прорезания за один проход (мм)
            // 8. Скоросто подачи в холостую (мм/мин)
            // 9. Скорость подачи при работе (мм/мин)

            return returnGCode;
        }

        /// <summary>
        /// Метод фрезерования по площади
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static string pocketOperation(Db.Polyline curve)
        {
            string returnGCode="";

            //Итак нам понадобится
            // 1. Имя операции
            // 2. Диаметр инструмента (мм)
            // 3. Глубина на которую будем фрезеровать (мм)
            // 4. Безопасная высота инструмента (мм)
            // 5. Начальная рабочая плоскость (мм)
            // 6. Шаг смещения в процентах (зависит от диаметра фрезы) Step over (% = 40)
            // 7. Глубина прорезания за один проход (мм)
            // 8. Толщина слоя подчистки (мм)
            // 9. Скоросто подачи в холостую (мм/мин)
            // 10. Скорость подачи при работе (мм/мин)
            // 11. Раправление прохода (по часовой / против часовой)

            return returnGCode;
        }

        /// <summary>
        /// Метод фрезерования следуя по кривой без смещения
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static string followPathOperation(Db.Polyline curve)
        {
            string returnGCode = "";
            
            //Итак нам понадобится
            // 1. Имя операции
            // 2. Диаметр инструмента (мм)
            // 3. Глубина на которую будем фрезеровать (мм)
            //4. С какой стороны от профиля будем работать (центр, идем строго по линии)
            // 5. Безопасная высота инструмента (мм)
            // 6. Начальная рабочая плоскость (мм)
            // 7. Глубина прорезания за один проход (мм)
            // 8. Скоросто подачи в холостую (мм/мин)
            // 9. Скорость подачи при работе (мм/мин)

            return returnGCode;
        }

        /// <summary>
        /// Метод сверления
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static string drillOperation(Db.Polyline curve)
        {
            string returnGCode = "";
            //Итак нам понадобится
            // 1. Имя операции
            // 2. Диаметр инструмента (мм)
            // 3. Глубина на которую будем фрезеровать (мм)
            // 4. Расположение отверстия (в центре выбранной кривой / вдоль кривой)
            // 5. hole spacing (мм)
            // 6. Начальная рабочая плоскость (мм)
            // 7. Глубина сверления за один проход (мм)
            // 8. Скорость подачи при свердении (мм/мин)

            return returnGCode;
        }

    }
}
