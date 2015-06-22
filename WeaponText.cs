namespace WebCrawler
{
    public class WeaponText : Weapon
    {
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
