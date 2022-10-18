using System.Xml.Serialization;

namespace FS19ModManager.config
{
    [XmlRoot(ElementName = "window")]
    public class Window
    {
        [XmlAttribute(AttributeName = "height")]
        public int Height { get; set; }
        [XmlAttribute(AttributeName = "width")]
        public int Width { get; set; }
    }

    [XmlRoot(ElementName = "folders")]
    public class Folders
    {
        [XmlElement(ElementName = "mods")]
        public string? Mods { get; set; }
        [XmlElement(ElementName = "active")]
        public string? Active { get; set; }
        [XmlAttribute(AttributeName = "game_root")]
        public string? GameRoot { get; set; }
    }

    [XmlRoot(ElementName = "config")]
    public class Config
    {
        [XmlElement(ElementName = "window")]
        public Window? Window { get; set; }
        [XmlElement(ElementName = "folders")]
        public Folders? Folders { get; set; }
        [XmlAttribute(AttributeName = "initialised")]
        public bool Initialised { get; set; }


        public Config(bool doinit)
        {
            if (doinit)
            {
                using (System.IO.StreamReader stream = new("config/config.xml"))
                {
                    var serializer = new XmlSerializer(typeof(Config));
                    var tmp = serializer.Deserialize(stream) as Config;
                    Window = tmp?.Window;
                    Folders = tmp?.Folders;
                    Initialised = tmp?.Initialised == true;
                }
            }
        }

        public Config() : this(false) { }

        public void Save()
        {
            using (System.IO.StreamWriter stream = new("config/config.xml"))
            {
                var serializer = new XmlSerializer(typeof(Config));
                serializer.Serialize(stream, this);
            }
        }
    }

}
