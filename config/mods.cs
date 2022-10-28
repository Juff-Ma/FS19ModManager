using System.Collections.Generic;
using System.Xml.Serialization;

namespace FS19ModManager.config
{
    [XmlRoot(ElementName = "mod")]
    public class Mod
    {
        [XmlAttribute(AttributeName = "hash")]
        public string? Hash { get; set; }
        [XmlAttribute(AttributeName = "filehash")]
        public string? Filehash { get; set; }
        [XmlText]
        public string? Name { get; set; }
    }

    [XmlRoot(ElementName = "mods")]
    public class Mods
    {
        [XmlElement(ElementName = "mod")]
        public List<Mod>? ModList { get; set; }
        [XmlAttribute(AttributeName = "active-profile")]
        public string? ActiveProfile { get; set; }

        public Mods(bool doinit)
        {
            if (doinit)
            {
                using (System.IO.StreamReader stream = new("config/mods.xml"))
                {
                    var serializer = new XmlSerializer(typeof(Mods));
                    var tmp = serializer.Deserialize(stream) as Mods;
                    ModList = tmp?.ModList;
                    ActiveProfile = tmp?.ActiveProfile;
                }
            }
        }

        public Mods() : this(false) { }

        public void Save()
        {
            using (System.IO.StreamWriter stream = new("config/mods.xml"))
            {
                var serializer = new XmlSerializer(typeof(Mods));
                serializer.Serialize(stream, this);
            }
        }
    }
}