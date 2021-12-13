namespace NARFUClassLib.Objects
{
    public class Lecture
    {
        public string num;
        public string time;
        public string dayofweek;
        public string? kindOfWork;
        public string? discipline;
        public string? group;
        public string? auditorium;
        public Lecture(string dayofweek, string num, string time = "Нет пары", string? kindOfWork = null,
                       string? discipline = null, string? group = null, string? auditorium = null)
        {
            this.dayofweek = dayofweek;
            this.num = num;
            this.time = time;
            this.kindOfWork = kindOfWork;
            this.discipline = discipline;
            this.group = group;
            this.auditorium = auditorium;
        }
    }
}
