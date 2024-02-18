
using System.Text.RegularExpressions;
using System.Xml;

//string url = "https://kcm.fm/";
//var html = await Load(url);

//var cleanHtml = new Regex("\\s").Replace(html, "");

//var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0);

//var htmlElement = "<div id=\"my-id\" class=\"my-class1 my-class2\" >text</div>";
//var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(htmlElement);

//bool k = true;
//async Task<string> Load(string url)
//{
//    HttpClient client = new HttpClient();
//    var response = await client.GetAsync(url);
//    var html = await response.Content.ReadAsStringAsync();
//    return html;
//}
Random random = new Random();   

List<string> allOptions = new List<string>();
string option = null;
do 
{
    Console.WriteLine( "enter option for stop entering write 'end'");
    option = Console.ReadLine();
    if (option == null)
        continue;
    if (option == "end")
        break;
    allOptions.Add( option );   

}while(option !="end");

Console.WriteLine( $"the selected meal is: {allOptions[random.Next(allOptions.Count)]}!!!!!!!!!!!"); 


