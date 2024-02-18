using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Html_Serializer
{
    using System;
    using System.IO;
    using System.Text.Json;

    public class HtmlHelper
    {
        private static HtmlHelper instance;
        private static readonly object lockObject = new object();
        private List<string> _tags { get; set; }
        private List<string> _voidTags { get; set; }
        private HtmlHelper()
        {
            _tags = JsonSerializer.Deserialize<List<string>>(File.ReadAllText("HtmlTags.json"));
            _voidTags = JsonSerializer.Deserialize<List<string>>(File.ReadAllText("HtmlVoidTags.json"));
        }

        public static HtmlHelper Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new HtmlHelper();
                    }
                    return instance;
                }
            }
        }
        public List<string> Tags
        {
            get { return _tags; }
            private set { _tags = value; }
        }

        // Explicitly defined property with private backing field for voidTags
        public List<string> VoidTags
        {
            get { return _voidTags; }
            private set { _voidTags = value; }
        }
    }
}
