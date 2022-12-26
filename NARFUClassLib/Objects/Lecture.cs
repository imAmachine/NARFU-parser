using NARFUClassLib.Structs;

namespace NARFUClassLib.Objects
{
    public class Lecture
    {
        public string num;
        public string? time;
        public string dayofweek;
        public string? kindOfWork;
        public string? discipline;
        public string? group;
        public string? auditorium_str;
        public Auditorium auditorium;
        public Lecture(string dayofweek, string num, string? time = null, string? kindOfWork = null,
                       string? discipline = null, string? group = null, string? auditorium_str = null, Auditorium auditorium = Auditorium.None)
        {
            this.dayofweek = dayofweek;
            this.num = num;
            this.time = time;
            this.kindOfWork = kindOfWork;
            this.discipline = discipline;
            this.group = group;
            this.auditorium_str = auditorium_str;
            this.auditorium = auditorium;
        }
    }
}
