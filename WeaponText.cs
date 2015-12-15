using System;

namespace WebCrawler
{
    [Serializable]
    public class WeaponText : Weapon
    {
        public WeaponText() { }

        public WeaponText(string name, WeaponType type)
            : base(name, type)
        {
        }

        public override string GetWeaponText()
        {
            return Name;
        }
    }
}
