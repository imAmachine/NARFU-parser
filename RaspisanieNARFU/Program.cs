using NARFUClassLib;

namespace RaspisanieNARFU
{
    public class Program
    {
        private const string url = @"https://ruz.narfu.ru/?timetable&group=15600";
        private static void LoadAllLectures() => LectureExtensions.lectures = LectureExtensions.GetLecturesList(new Uri(url));

        private static readonly Dictionary<string, string> filters = new()
        {
            { "Прикладная физическая культура и спорт", "Замараева М.П." },
            { "Русский язык как иностранный (факультатив)", string.Empty },
        };
        private static readonly string group_filter = "151115_1";
        internal static void Main()
        {
            LoadAllLectures();
            var fgLeactures = LectureExtensions.lectures.FilterLectures(filters, group_filter)
                                                        .GetGroupedLectureList();
            Console.WriteLine(fgLeactures.GetLecturesString());
            Console.ReadLine();
        }
    }
}