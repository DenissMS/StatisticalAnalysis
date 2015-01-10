using System.IO;
using System.Xml.Serialization;

namespace StatisticalAnalysis
{
    public class StaticticsManager
    {
        public const string DEFAULT_DATABASE_PATH = "Database.xml";
        public const string DEFAULT_PRODUCTS_PATH = "Products.xml";
        public string VisitorsPath = DEFAULT_DATABASE_PATH;
        public string ProductsPath = DEFAULT_PRODUCTS_PATH;
        public VisitorsBase VisitorsBase;
        public ProductsDatabase ProductsBase;
        public StaticticsManager()
        {
            if (File.Exists(ProductsPath))
            {
                ProductsBase = Database.LoadDatabase<ProductsDatabase>(ProductsPath);
                if (File.Exists(VisitorsPath))
                {
                    VisitorsBase = Database.LoadDatabase<VisitorsBase>(VisitorsPath);
                }
            }
        }

        public bool IsDatabaseLoaded
        {
            get { return VisitorsBase != null; }
        }

        public void SaveDatabases()
        {
            if(VisitorsBase != null)
                VisitorsBase.SaveDatabase<VisitorsBase>(VisitorsPath);
            if(ProductsBase != null)
                ProductsBase.SaveDatabase<ProductsDatabase>(ProductsPath);
        }

        public void CreateVisitorsDatabase(string path)
        {
            const string regex = "([1-3]?[0-9]\\.[01][0-9]\\.[12][0-9]{3}\\s[012][0-9]:[0-5][0-9])" +
                                 "\\s((?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))" +
                                 "\\r\\n((?:http|https)(?::\\/{2}[\\w]+)(?:[\\/|\\.]?)(?:[^\\s\\\"]*))";
            using (var reader = new StreamReader(path))
            {
                VisitorsBase = new VisitorsBase
                {
                    Visitors = VisitorsBase.GetVisitorsFromText(reader.ReadToEnd(), regex)
                };  
            }
        }
    }
}
