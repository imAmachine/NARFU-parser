using NARFUClassLib.Structs;
using System.Xml.Linq;

namespace NARFUClassLib.Tools
{
    public class Filters
    {
        private string Path;
        public Filters(string Path)
        {
            this.Path = Path;
        }

        public Dictionary<string, Auditorium> GetFilters()
        {
            return GetFiltersFromFile();
        }

        private Dictionary<string, Auditorium> GetFiltersFromFile()
        {
            XDocument xDoc = null;
            if (!File.Exists(Path))
            {
                xDoc = new XDocument();
                xDoc.Add(new XElement("Filters"));
                xDoc.Save(Path);
            }
            else
            {
                xDoc = XDocument.Load(Path);
            }
            var filters = xDoc.Descendants("Filters").Elements("Filter");
            if (filters.Count() > 0)
            {
                var pairs = filters.Select(filter =>
                {
                    Auditorium aud_enum = Auditorium.None;
                    string aud = filter.Attribute(XName.Get("auditorium")).Value;
                    string key = filter.Attribute(XName.Get("discipline")).Value != null || filter.Attribute(XName.Get("discipline")).Value == ""
                                ? filter.Attribute(XName.Get("discipline")).Value : string.Empty;
                    if (aud == "All")
                        aud_enum = Auditorium.All;
                    if (aud == "Cabinet")
                        aud_enum = Auditorium.Cabinet;
                    if (aud == "Dist")
                        aud_enum = Auditorium.Dist;
                    return new KeyValuePair<string, Auditorium>(key, aud_enum);
                }).ToArray();

                return pairs.ToDictionary(p => p.Key, p => p.Value);
            }
            else return null;
        }
    }
}
