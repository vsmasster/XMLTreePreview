using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Tree {
	public class XmlTree {
		private static Node<Record> LoadElement(XElement elem) {
            Record data = new Record();
            IEnumerable<XAttribute> attList = from at in elem.Attributes() select at;
            
            foreach(XAttribute att in attList)
                data[att.Name.ToString()] = att.Value;
 
			return new Node<Record>(data, elem.Elements("node").Select(e => LoadElement(e)));
		}

		public static Node<Record> Load(string file) {
			XDocument xml = XDocument.Load(file);
			if (xml.Root.Name != "node") return null;
			return LoadElement(xml.Root);
		}

        private static XElement SaveElement(Node<Record> node) {
            XElement result = new XElement("node");

            IEnumerable<XAttribute> attList = node.Data.Attributes;
            foreach (XAttribute att in attList)
                result.SetAttributeValue(att.Name, att.Value);

            result.Add(node.Children.Select(e => SaveElement(e)));

            return result;
        }

        public static void Save(string file,Node<Record> root) {
            XDocument doc = new XDocument(SaveElement(root));
            doc.Save(file);
        }
	}

    public class Record {
        private Dictionary<String, String> content = new Dictionary<string, string>();

        public String this[String index] {
            get { return content[index]; }
            set { content[index] = value; }
        }

        public IEnumerable<XAttribute> Attributes{
            get{
                List<XAttribute> result = new List<XAttribute>();
  
                foreach (String key in content.Keys)
                    result.Add(new XAttribute(key, content[key]));

                return result;
            }
        }

        public override String ToString() {
            String result = "{";
            foreach (String key in content.Keys)
                result += "\"" + key + "\" = \"" + content[key] + "\", ";

            result = result.Substring(0, result.Length - 2);
            result += "}\n";
            return result;
        }
    }
}
