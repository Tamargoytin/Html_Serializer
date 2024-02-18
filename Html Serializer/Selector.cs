namespace Html_Serializer
{
    public class Selector
    {
        private string id { get; set; }
        private List<string>classes { get; set; }=new List<string>();
        private string tagName { get; set; }
        private Selector parent { get; set; }
        private Selector child { get; set; } 
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public List< string >Classes
        {
            get { return classes; }
            set { classes = value; }
        }
        public string TagName
        {
            get { return tagName; }
            set { tagName = value; }
        }
        public Selector Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        public Selector Child
        {
            get { return child; }   
            set {child = value;}
        }

        public static Selector  singleHierarchyTreatment(string htmlQuery )
        {
            Selector currentSelector = new Selector();
            //start with tagName
            if (!htmlQuery.StartsWith("#") && !htmlQuery.StartsWith("."))
            {
                List<string> tag = htmlQuery.Split("#").ToList();
                if (tag.Count > 1)
                {
                    HtmlHelper htmlHelper = HtmlHelper.Instance;
                    List<string> tags = htmlHelper.Tags;
                    List<string> voidTags = htmlHelper.VoidTags;
                    if (!tags.Contains(tag[0]) && !voidTags.Contains(tag[0]))
                        throw new Exception("invalid tag name!!!");
                    currentSelector.tagName = tag[0];
                    List<string> id = tag[1].Split(".").ToList();
                    currentSelector.id = id[0];
                    if (id.Count > 1)
                    {
                        currentSelector.classes = id.GetRange(1, id.Count);
                    }
                }
                else
                {
                     tag = htmlQuery.Split(".").ToList();
                    if(tag.Count > 1)
                    {
                        currentSelector.tagName = tag[0];
                        currentSelector.classes = tag.GetRange(1, tag.Count-1);
                    }
                    else
                        currentSelector.tagName = tag[0];
                }
            }
            //start with id
            else if (htmlQuery.StartsWith("#"))
            {
                List<string> checkClasses = htmlQuery.Split(".").ToList();
                currentSelector.id = checkClasses[0];
                if (checkClasses.Count > 1)
                {
                    currentSelector.classes = checkClasses.GetRange(1, checkClasses.Count);
                }

            }
            //just classes
            else
            {
                currentSelector.classes = htmlQuery.Split(".").ToList();
                currentSelector.classes.RemoveAt(0);

            }
            return currentSelector;
        }
        public static Selector convertStringToSelectorObject(string htmlQuery)
        {
            List<string> hierarchy = htmlQuery.Split(" ").ToList();
            Selector rootSeletor=new Selector();
            if(hierarchy.Count==1)
            {
                return singleHierarchyTreatment(hierarchy[0]);

            }
            else
            {
                
                Selector parentSelector = singleHierarchyTreatment(hierarchy[0]);
                Selector currentSelector = null;
                for (int i = 1; i < hierarchy.Count; i++)
                {
                    currentSelector = singleHierarchyTreatment(hierarchy[i]);
                    parentSelector.child = currentSelector;
                    currentSelector.parent = parentSelector;
                    if (i == 1)
                    {
                        rootSeletor = parentSelector;

                    }
                    parentSelector = currentSelector;
                }
                return rootSeletor;
            }
        }


    }
}
