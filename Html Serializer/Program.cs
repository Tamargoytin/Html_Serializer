
using Html_Serializer;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

var html = await Load("https://hebrewbooks.org/beis");
var cleanHtml = new Regex("\\s{2,}").Replace(html, "");
var htmlLines = new Regex("<(.*?)>").Split((string)cleanHtml).Where(s => s.Length > 0);
HtmlHelper htmlHelper = HtmlHelper.Instance;
List<string> tags = htmlHelper.Tags;
List<string> voidTags = htmlHelper.VoidTags;
Console.WriteLine();
HtmlElement parentElement = null;
HtmlElement currentElement = null;
HtmlElement root = null;
string htmlQuery= "table tr td a img";
Selector selector = Selector.convertStringToSelectorObject(htmlQuery);
void rootElement(string line)
{
    root = new HtmlElement("html");
    var att = new Regex("([^\\s]?)=\"(.*?)\"").Matches(line);

    foreach (var item in att)
    {
        root.Attributes.Add(item.ToString());
    }
    parentElement = root;
    
}
void divideAttribute(HtmlElement element, string line)
{
    var att = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);
    foreach (var item in att)
    {
        List<string> divideAtt = item.ToString().Split("=").ToList();
        if (divideAtt[0] == "id")
            element.Id = divideAtt[1];
        else if (divideAtt[0] == "class")
            element.Classes = (divideAtt[1].Split(" ").ToList());
        else
            element.Attributes.Add(item.ToString());
    }
}
void selfClosingTag(string line,string tag)
{
    currentElement = new HtmlElement(tag);
    currentElement.Parent = parentElement;
    divideAttribute(currentElement, line);
    parentElement.Children.Add(currentElement);
    parentElement = currentElement;
}
void noneClosingTag(string line, string tag)
{
    HtmlElement noneClosingElement=new HtmlElement(tag);
    noneClosingElement.Parent = currentElement;
    divideAttribute(noneClosingElement,line);
    currentElement.Children.Add(noneClosingElement);

}
foreach (var line in htmlLines)
{
    //Took care of the end of the root element
    if (line == "/html")
        break;
    if (line.StartsWith("!")) { continue; }
    
    //Took care of the root element
    string[] linesplitted = line.Split(' ');
    if (linesplitted[0].Equals("html"))
    {
        rootElement(line);
        continue;
    }
    //Took care of all ends of objects
    else if (line.StartsWith("/"))
    {
        currentElement = parentElement;
        parentElement = parentElement.Parent;
    }
    //Took care o all other objects

    else if (tags.Contains(linesplitted[0]))
    {
        selfClosingTag(line, linesplitted[0]);
      
    }
    else if (voidTags.Contains(linesplitted[0]))
    {
        noneClosingTag(line,linesplitted[0]);
    }

    // check innerHtml if itsnt existin tags and voidtags
    else
    {
        currentElement.InnerHtml = line;
    }

}
HashSet<HtmlElement> elements = new HashSet<HtmlElement>();
elements.Add(currentElement);
elements = root.findElementBySelector(selector, elements);
foreach (var element in elements)
{
    Console.WriteLine(element.Name+ " "+element.Parent.Name);
}
bool flag = true;
async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
