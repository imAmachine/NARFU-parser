using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using NARFUClassLib.Objects;
using NARFUClassLib.Tools;
using System.Text.RegularExpressions;

namespace NARFUClassLib.Extensions
{
    public static class LectureExtensions
    {
        public static List<Lecture> lectures = new();

        private static List<Lecture> GetParsedList(string? page, string week = "1")
        {
            if (page != null)
            {
                List<Lecture> list = new();
                HtmlParser parser = new();
                IDocument doc = parser.ParseDocument(page);

                foreach (var headEl in doc.GetElementById($"week_{week}").QuerySelectorAll(".list"))
                {
                    string dayOfWeek = new Regex(@"\s+").Replace(headEl.QuerySelector(".dayofweek").TextContent.Trim(), " ");
                    foreach (var time_table_sheet in headEl.QuerySelectorAll(".timetable_sheet"))
                    {
                        var span_list = time_table_sheet.QuerySelectorAll("span");
                        if (span_list.Length >= 5)
                        {
                            span_list = span_list.Where(item => item.ClassName != null).ToCollection();
                            Dictionary<string, string> span_dict = new Dictionary<string, string>();
                            foreach (var item in span_list)
                            {
                                string key = item.ClassName;
                                string val = new Regex(@"\s{2,}").Replace(item.TextContent.Trim(), " ");
                                if (span_dict.ContainsKey(key))
                                    span_dict[key] += "\n" + val;
                                else
                                    span_dict.Add(key, val);
                            }
                            Lecture lect;
                            if (span_list.Length == 5)
                                lect = new Lecture(dayOfWeek, span_dict["num_para"], span_dict["time_para"], span_dict["kindOfWork"],
                                                        span_dict["discipline"], "", span_dict["auditorium"]);
                            else
                                lect = new Lecture(dayOfWeek, span_dict["num_para"], span_dict["time_para"], span_dict["kindOfWork"],
                                                        span_dict["discipline"], span_dict["group"], span_dict["auditorium"]);
                            list.Add(lect);
                        }
                    }
                }
                return list;
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

        public static string GetLecturesString(this IEnumerable<IGrouping<string?, Lecture>> lecturesList)
        {
            string result = string.Empty;
            foreach (IGrouping<string, Lecture> pair in lecturesList)
            {
                result += $"============={pair.Key}=============\n";
                foreach (var lect in pair)
                    result += $"[{lect.num} пара] - {lect.time}\n{lect.discipline}\n{lect.kindOfWork}\n{lect.auditorium}\n{lect.group}\n\n";
            }
            return result;
        }

        public static List<Lecture> FilterLectures(this List<Lecture> lectures, Dictionary<string, string> filters, Dictionary<string, string> group)
        {
            return lectures.Where(lect =>
            {
                if (lect.group != null)
                    foreach (var filter in group)
                    {
                        KeyValuePair<string, string> pair = filter;
                        if (lect.group.Contains(pair.Key))
                            if (lect.group.Contains("_") && !lect.group.Contains("_" + pair.Value))
                                return false;
                    }
                if (lect.discipline != null)
                {
                    foreach (var filter in filters)
                    {
                        if (lect.discipline.Contains(filter.Key))
                        {
                            if (filter.Value == "" || filter.Value == null || filter.Value == string.Empty)
                                return false;
                            return lect.discipline.Contains(filter.Value);
                        }
                    }
                }
                return true;
            }).ToList();
        }
    }
}
