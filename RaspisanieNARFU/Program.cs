using NARFUClassLib.Extensions;

namespace RaspisanieNARFU
{
    public class Program
    {
        private const string url = @"https://ruz.narfu.ru/?timetable&group=15600";
        private static void LoadAllLectures(string? week) => LectureExtensions.lectures = LectureExtensions.GetLecturesList(new Uri(url), week);

        private static readonly Dictionary<string, string> filters = new()
        {
            { "Прикладная физическая культура и спорт", "Замараева М.П." },
            { "Русский язык как иностранный (факультатив)", string.Empty },
        };
        private static readonly string group_filter = "151115_1";
        internal static void Main()
        {
            Console.Write("Номер недели (1-6) начиная с текущей: ");
            LoadAllLectures(Console.ReadLine());
            Console.Clear();
            ConsoleKeyInfo key;
            do
            {
                Console.WriteLine("===========МЕНЮ ВЫБОРА===========" +
                    "\nВведите клавишу:" +
                    "\n1) Клавишу ESC, чтобы выйти," +
                    "\n2) Клавишу A, чтобы получить расписание недели," +
                    "\n3) Клавишу T, чтобы получить расписание на сегодня," +
                    "\n4) Клавишу N, чтобы получить насписание на завтра.");
                key = Console.ReadKey();
                Console.Clear();
                var fgLeactures = LectureExtensions.lectures.FilterLectures(filters, group_filter);
                if (key.Key == ConsoleKey.A)
                {
                    Console.WriteLine(fgLeactures.GetGroupedLectureList().GetLecturesString());
                }
                else if (key.Key == ConsoleKey.T)
                {
                    Console.WriteLine(fgLeactures.Where(x => DateTime.Parse(x.dayofweek).DayOfWeek == DateTime.Now.DayOfWeek).ToList()
                                             .GetGroupedLectureList()
                                             .GetLecturesString());
                }
                else if (key.Key == ConsoleKey.N)
                {
                    Console.WriteLine(fgLeactures.Where(x => DateTime.Parse(x.dayofweek).DayOfWeek == DateTime.Now.AddDays(1).DayOfWeek).ToList()
                                             .GetGroupedLectureList()
                                             .GetLecturesString());
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
                Console.WriteLine("=======> Введите любую клавишу, чтобы вернуться в меню <=======");
                Console.ReadKey();
                Console.Clear();
            } while (true);
        }
    }
}