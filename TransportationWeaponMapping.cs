using System.Collections.Generic;

namespace WebCrawler
{
    public static class TransportationWeaponMapping
    {
        public static List<Weapon> GetWeaponsByTransportationName(string name)
        {
            switch (name)
            {
                case "4R3 Light Assault Transport":
                    return new List<Weapon>()
                    {
                        WeaponFactory.CreateWeapon("ReFo", "LCM", "Lin1"),
                        WeaponFactory.CreateWeapon("ReFo", "icl", "Lin1"),
                        WeaponFactory.CreateWeapon("Fo", "ptl")
                    };
                default:
                    return new List<Weapon>();
            }
        }
    }
}
