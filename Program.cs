using System;

namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Webcrawler webcrawler = new Webcrawler(new Uri("http://swrpg.viluppo.net/"));
            webcrawler.CrawlWeb(true, true, false);
        }
    }
}
