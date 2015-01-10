using System.Collections.Generic;
using System.Xml.Serialization;

namespace StatisticalAnalysis
{
    [XmlRoot("ProductsDatabase", Namespace = "http://www.cpandl.com", IsNullable = false)]
    public class ProductsDatabase : Database
    {
        [XmlArray("Products")]
        [XmlArrayItem("Product")]
        public List<Product> Products { get; set; }

        [XmlArray("Categories")]
        [XmlArrayItem("Category")]
        public List<Category> Categories { get; set; }

        public class Category
        {
            [XmlAttribute]
            public int Id { get; set; }
            [XmlAttribute]
            public string Name { get; set; }
            [XmlAttribute]
            public string Description { get; set; }
        }
        public class Product
        {
            [XmlAttribute]
            public int Id { get; set; }
            [XmlAttribute]
            public string Name { get; set; }
            [XmlAttribute]
            public int CategoryId { get; set; }
            [XmlAttribute]
            public string Description { get; set; }
        }
    }
}
