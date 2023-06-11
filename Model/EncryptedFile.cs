using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace cybersecurity_encryption.Model
{
    [XmlType("EncryptedFile")]
    public class EncryptedFile
    {
        [XmlElement]
        public string EncryptionType{ get; set; }
        [XmlElement]
        public string HashingAlgorithm { get; set; }
        [XmlElement]
        public string PaddingMode { get; set; }
        [XmlElement]
        public byte[] IV{ get; set; }
        [XmlElement]
        public byte[] Content { get; set; }
        [XmlElement]
        public int PaddingLen { get; set; }
        public EncryptedFile() { }
        public void SaveToFile(string filename)
        {
            filename = filename + ".xml";
            string xmlFileLocation = System.IO.Path.Combine("../../../Resources/", filename);
            using (TextWriter filestream = new StreamWriter(xmlFileLocation))
            {
                XmlSerializer x = new(this.GetType(), new XmlRootAttribute(filename));
                x.Serialize(filestream, this);
            }
        }
        public static EncryptedFile ReadFromFile(string filename)
        {
            filename = filename + ".xml";
            string xmlFileLocation = System.IO.Path.Combine("../../../Resources/", filename);
            using (Stream reader = new FileStream(xmlFileLocation, FileMode.Open))
            {
                XmlSerializer x = new XmlSerializer(typeof(EncryptedFile), new XmlRootAttribute(filename));
                var output = x.Deserialize(reader);
                return output as EncryptedFile;
            }
        }
    }
}
