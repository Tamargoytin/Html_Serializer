using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Html_Serializer
{
    internal class HtmlElement
    {
        private string id { get; set; }
        private string name { get; set; }
        private List<string> attributes { get; set; } = new List<string>();
        private List<string> classes { get; set; } = new List<string>();
        private string innerHtml { get; set; }
        private HtmlElement parent { get; set; }
        private List<HtmlElement> children { get; set; } = new List< HtmlElement>();


        //public HtmlElement(string id, string name, string innerHtml, string parent, string[] children, string[] attributes, string[] classes) {
        public HtmlElement(string name)
        {
            this.name = name;
        }


        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
             set { name = value; }
        }
        public List<string> Attributes
        {
            get { return attributes; }
             set { attributes = value; }
        }
        public List<string> Classes
        {
            get { return classes; }
             set { classes = value; }
        }
        public string InnerHtml
        {
            get { return innerHtml; }
             set { innerHtml = value; }
        }
        public HtmlElement Parent
        {
            get { return parent; }
             set { parent = value; }
        }
        public List<HtmlElement> Children
        {
            get { return children; }
             set { children = value; }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement curruentElemnt = this;
            while (curruentElemnt!=null)
            {
                yield return curruentElemnt;
                curruentElemnt = curruentElemnt.Parent;
            }

        }
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> elems = new Queue<HtmlElement>();
            elems.Enqueue(this);
            while (elems.Count>0)
            {
                HtmlElement cur = elems.Dequeue();
                yield return cur;
                foreach (HtmlElement child in cur.Children)
                {
                    elems.Enqueue(child);
                }
            }
        }
        public List<HtmlElement> search(List<HtmlElement> list,Selector selector)
        {
            List < HtmlElement > sortedList=new List < HtmlElement >();
            foreach (var item in list)
            {
                if (selector.TagName != null && selector.Id != null && selector.Classes.Count > 0)
                {
                    if (item.Name == selector.TagName && item.id == selector.Id && selector.Classes.All(i => item.Classes.Contains(i)))
                    {
                        sortedList.Add(item);
                    }
                }
                else if (selector.TagName != null && selector.Classes.Count > 0 )
                {
                    if (item.Name == selector.TagName && selector.Classes.All(i => item.Classes.Contains(i)))
                    {
                        sortedList.Add(item);
                    }
                }

                else if (selector.TagName != null && selector.Id!=null)
                {
                    if (item.Name == selector.TagName && item.id==selector.Id)
                    {
                        sortedList.Add(item);
                    }
                }
                else if (selector.Id != null && selector.Classes.Count > 0)
                {
                    if (item.id == selector.Id && selector.Classes.All(i => item.Classes.Contains(i)))
                    {
                        sortedList.Add(item);
                    }
                }
                else if (selector.TagName != null )
                {
                    if (item.Name == selector.TagName)
                    {
                        sortedList.Add(item);
                    }
                }
                else if (selector.Id != null)
                {
                    if (item.id == selector.Id)
                    {
                        sortedList.Add(item);
                    }
                }
                else if (selector.Classes.Count > 0)
                {
                    if (selector.Classes.All(i => item.Classes.Contains(i)))
                    {
                        sortedList.Add(item);
                    }
                }
            }
            return sortedList;  
        }
        public HashSet<HtmlElement> findElementBySelector(Selector selector, HashSet<HtmlElement> list)
        {
            if (selector == null)
                return list;
            HashSet<HtmlElement> sortedList = new HashSet<HtmlElement>();
            foreach (var item in list)
            {
                sortedList.UnionWith(search(item.Descendants().ToList(), selector));

            }
            return findElementBySelector(selector.Child, sortedList);
        }
    }
}
