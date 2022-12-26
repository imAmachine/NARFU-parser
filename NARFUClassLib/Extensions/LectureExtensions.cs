using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using NARFUClassLib.Objects;
using NARFUClassLib.Structs;
using NARFUClassLib.Tools;
using System.Text.RegularExpressions;

namespace NARFUClassLib.Extensions
{
    public static class LectureExtensions
    {
        public static List<Lecture> lectures = new();

        private static List<Lecture>? GetParsedList(string? page, string week = "1")
        {
            if (page != null)
            {
                List<Lecture> list = new();
                HtmlParser parser = new();
                IDocument doc = parser.ParseDocument(page);

                if (doc != null)
                {
                    foreach (IElement? headEl in doc.GetElementById($"week_{week}").QuerySelectorAll(".list"))
                    {
                        string? dayOfWeek = new Regex(@"\s+").Replace(headEl.QuerySelector(".dayofweek").TextContent.Trim(), " ");
                        foreach (var time_table_sheet in headEl.QuerySelectorAll(".timetable_sheet"))
                        {
                            var span_list = time_table_sheet.QuerySelectorAll("span");
                            if (span_list.Length >= 5)
                            {
                                span_list = span_list.Where(item => item.ClassName != null).ToCollection();
                                Dictionary<string, string> span_dict = new Dictionary<string, string>();
                                foreach (var item in span_list)
                                {
                                    string? key = item.ClassName;
                                    string? val = new Regex(@"\s{2,}").Replace(item.TextContent.Trim(), " ");
                                    if (key != null && val != null)
                                    {
                                        if (span_dict.ContainsKey(key))
                                            span_dict[key] += "\n" + val;
                                        else
                                            span_dict.Add(key, val);
                                    }
                                }
                                Lecture lect;
                                Auditorium aud = span_dict["auditorium"].Contains("Дистанционное обучение") ? Auditorium.Dist : Auditorium.Cabinet;

                                if (span_list.Length == 5)
                                    lect = new Lecture(dayOfWeek, span_dict["num_para"], span_dict["time_para"], span_dict["kindOfWork"],
                                                            span_dict["discipline"], "", span_dict["auditorium"], aud);
                                else
                                    lect = new Lecture(dayOfWeek, span_dict["num_para"], span_dict["time_para"], span_dict["kindOfWork"],
                                                            span_dict["discipline"], span_dict["group"], span_dict["auditorium"], aud);
                                list.Add(lect);
                            }
                        }
                    }
                    return list;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public static List<Lecture> GetLecturesList(Uri url, string? week)
        {
            string page = WebTools.GetHtmlPage(url);
            return GetParsedList(page, week);
        }

        public static IEnumerable<IGrouping<string?, Lecture>> GetGroupedLectureList(this List<Lecture> lectures)
        {
            return lectures.OrderBy(x => DateTime.Parse(x.dayofweek)).GroupBy(x => x.dayofweek);
        }

        public static string GetLecturesString(this IEnumerable<IGrouping<string?, Lecture>> lecturesList, bool short_format)
        {
            string result = string.Empty;
            Console.WriteLine("Компактный режим отображения: " + short_format);
            foreach (IGrouping<string, Lecture> pair in lecturesList)
            {
                result += $"============={pair.Key}=============\n";
                if (!short_format)
                {
                    foreach (var lect in pair)
                        result += $"[{lect.num} пара] - {lect.time}\n{lect.discipline}\n{lect.kindOfWork}\n{lect.auditorium_str}\n{lect.group}\n\n";
                }
                else
                {
                    foreach (var lect in pair)
                        result += $"[{lect.num} пара] {lect.time} - {lect.discipline} - {lect.auditorium_str}\n";
                }
            }
            return result;
        }

        public static List<Lecture> FilterLectures(this List<Lecture> lectures, Dictionary<string, Auditorium> filters)
        {
            List<Lecture> result = lectures;
            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    if (filter.Key == string.Empty)
                    {
                        if (filter.Value == Auditorium.All)
                            result = result.Where(lect => lect.auditorium != Auditorium.Cabinet && lect.auditorium != Auditorium.Dist).ToList();
                        else
                            result = result.Where(lect => lect.auditorium != filter.Value).ToList();
                    }
                    else
                    {
                        if (filter.Value == Auditorium.All)
                            result = result.Where(lect => !lect.discipline.Contains(filter.Key)).ToList();
                        else
                            result = result.Where(lect => !lect.discipline.Contains(filter.Key) && lect.auditorium != filter.Value).ToList();
                    }
                }
            }
            return result;
        }
    }
}
