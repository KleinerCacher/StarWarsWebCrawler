﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Webcrawler webcrawler = new Webcrawler(new Uri("http://swrpg.viluppo.net/"));
            webcrawler.CrawlWeb();
        }
    }
}
