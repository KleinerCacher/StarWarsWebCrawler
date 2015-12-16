using System.Collections.Generic;

namespace WebCrawler
{
    public class RangeComparer : IComparer<Weapon>
    {
        public int Compare(Weapon x, Weapon y)
        {
            int rangex = GetNumberForRange(x.Range);
            int rangey = GetNumberForRange(y.Range);

            return rangex.CompareTo(rangey);
        }

        private int GetNumberForRange(string range)
        {
            switch (range)
            {
                case "Close":
                    return 1;
                case "Short":
                    return 2;
                case "Medium":
                    return 3;
                case "Long":
                    return 4;
                case "Extreme":
                    return 5;
                default:
                    return 10;
            }
        }
    }
}
