using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace StatisticalAnalysis
{
    [XmlRoot("VisitorsBase", Namespace = "http://www.cpandl.com", IsNullable = false)]
    public class VisitorsBase : Database
    {
        public List<Visitor> Visitors { get; set; }

        public static List<Visitor> GetVisitorsFromText(string text, string pattern)
        {
            var r = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var matches = r.Matches(text);
            var visitors = new List<Visitor>();
            const string format = "d.MM.yyyy HH:mm";
            foreach (Match match in matches)
            {
                DateTime time;
                DateTime.TryParseExact(match.Groups[1].ToString(), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out time);
                IPAddress address;
                IPAddress.TryParse(match.Groups[2].ToString(), out address);
                Uri url;
                Uri.TryCreate(match.Groups[3].ToString(), UriKind.Absolute, out url);
                visitors.Add(new Visitor
                {
                    Time = time,
                    Address = address,
                    Uri = url,
                });
            }
            return visitors;
        }

        public class Visitor
        {
            [XmlAttribute]
            public DateTime Time { get; set; }
            [XmlIgnore]
            public IPAddress Address { get; set; }
            [XmlAttribute("Address")]
            public string AddressStr
            {
                get { return Address.ToString(); }
                set { Address = IPAddress.Parse(value); }
            }
            [XmlIgnore]
            public Uri Uri { get; set; }
            [XmlAttribute("Uri")]
            public string UriStr
            {
                get { return Uri.AbsoluteUri; }
                set { Uri = new Uri(value); }
            }
            [XmlAttribute]
            public int ProductId 
            {
                get { return Convert.ToInt32(UriStr.Split('/')[3]); }
            }
        }
    }
}
