using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace WebCrawler
{
    [Serializable]
    public class Mount
    {
        private string[] unitsMap = new[] { "Zero", "One", "Two", "Three", "Four",
                                            "Five", "Six", "Seven", "Eight", "Nine"};

        private int numberOfWeapons = -1;
        private int numberOfLinkedWeapons = -1;
        private List<MountFlags> directionFlags = new List<MountFlags>();
        private List<MountFlags> otherFlags = new List<MountFlags>();

        public string NumberOfWeaponsText
        {
            get { return GetTextFromNumberOfWeapons(); }
        }

        public string NumberOfLinkedWeaponsText
        {
            get { return GetTextFromNumberOfLinkedWeapons(); }
        }

        [Flags]
        public enum MountFlags
        {
            Turret,
            Retractable,
            Dorsal,
            Ventral,
            Port,
            Starbord,
            Forward,
            Aft,
            HardPoint,
            Wing,
            All,
            Hull,
            Top,
            Down
        }

        public Mount() { }

        public Mount(string mountShortcut)
        {
            if (Char.IsDigit(mountShortcut[0]))
            {
                var number = Regex.Match(mountShortcut, @"([0-9]+)[a-zA-Z]").Groups[1].Value;
                mountShortcut = Regex.Match(mountShortcut, @"[0-9]+(.*)").Groups[1].Value;

                numberOfWeapons = int.Parse(number.ToString());
            }

            if (Char.IsDigit(mountShortcut[mountShortcut.Length - 1]))
            {
                numberOfLinkedWeapons = int.Parse(mountShortcut[mountShortcut.Length - 1].ToString());
                mountShortcut = mountShortcut.Remove(mountShortcut.Length - 1);
            }

            InitializeFlagsFromRemainingShortcut(mountShortcut);
        }

        public static string GetDescriptiveMountText(List<Mount> mounts)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < mounts.Count; i++)
            {
                Mount mount = mounts[i];
                sb.Append(mount.GetTextFromNumberOfWeapons());

                if (i == 0 && mount.otherFlags.Contains(MountFlags.Retractable))
                {
                    sb.Append("Retractable ");
                }

                if (mount.otherFlags.Contains(MountFlags.HardPoint))
                {
                    sb.Append("Hardpoint ");
                }
                else if (mount.otherFlags.Contains(MountFlags.Wing))
                {
                    sb.Append("Wingtip ");
                }
                else if (mount.otherFlags.Contains(MountFlags.Hull))
                {
                    sb.Append("Hull ");
                }
                else if (mount.otherFlags.Contains(MountFlags.Top))
                {
                    sb.Append("Top ");
                }
                else
                {
                    for (int j = 0; j < mount.directionFlags.Count; j++)
                    {
                        sb.Append(Enum.GetName(typeof(MountFlags), mount.directionFlags[j]));
                        string separatedString = j != mount.directionFlags.Count - 1 ? " and " : " ";
                        sb.Append(separatedString);
                    }
                }

                if (i == mounts.Count - 1 && mount.otherFlags.Contains(MountFlags.Turret))
                {
                    sb.Append("Turret ");
                }

                if (i == mounts.Count - 1)
                {
                    sb.Append("mounted");
                }
                else
                {
                    sb.Append(", ");
                }

                sb.Append(mount.GetTextFromNumberOfLinkedWeapons());
            }

            return sb.ToString();
        }

        public static string GetFireArcAsText(List<Mount> mounts)
        {
            return GetFireArc(mounts, true);
        }

        public static string GetFireArcAsIcon(List<Mount> mounts)
        {
            if (mounts.Count > 0 && MountsHaveDifferentNumberOfWeapons(mounts))
            {
                StringBuilder sb = new StringBuilder();
                foreach (var mount in mounts)
                {
                    sb.AppendFormat("{0};", GetFireArc(new List<Mount>() { mount }, false));
                }

                sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }

            return GetFireArc(mounts, false);
        }

        private static bool MountsHaveDifferentNumberOfWeapons(List<Mount> mounts)
        {
            int numberOfLastMountsWeapons = int.MinValue;
            foreach (var mount in mounts)
            {
                if (numberOfLastMountsWeapons != int.MinValue && numberOfLastMountsWeapons != mount.numberOfWeapons)
                {
                    return true;
                }

                numberOfLastMountsWeapons = mount.numberOfWeapons;
            }

            return false;
        }

        private static string GetFireArc(List<Mount> mounts, bool asText)
        {
            string text = string.Empty;

            List<MountFlags> allOtherFlags = mounts.SelectMany(x => x.otherFlags).Distinct().ToList();
            List<MountFlags> allDirectionFlags = mounts.SelectMany(x => x.directionFlags).Distinct().ToList();

            if (allOtherFlags.Contains(MountFlags.Hull)
                || allOtherFlags.Contains(MountFlags.Turret) && allOtherFlags.Contains(MountFlags.All)
                || allOtherFlags.Contains(MountFlags.Turret) && allDirectionFlags.Contains(MountFlags.Dorsal)
                || allOtherFlags.Contains(MountFlags.Turret) && allDirectionFlags.Contains(MountFlags.Ventral)
                || allDirectionFlags.Contains(MountFlags.Dorsal)
                || allDirectionFlags.Contains(MountFlags.Ventral)
                || allOtherFlags.Contains(MountFlags.Top)
                || (allDirectionFlags.Contains(MountFlags.Ventral)
                    && allDirectionFlags.Contains(MountFlags.Port)
                    && allDirectionFlags.Contains(MountFlags.Starbord)))
            {
                text = asText ? "All" : "(all)";

                if (asText)
                {
                    text = "All";
                }
                else
                {
                    if (allDirectionFlags.Contains(MountFlags.Dorsal)
                        && allDirectionFlags.Contains(MountFlags.Ventral))
                    {
                        text = "(all)2";
                    }
                    else
                    {
                        text = "(all)";
                    }
                }
            }
            else if (allOtherFlags.Contains(MountFlags.Down))
            {
                text = "Down";
            }
            else if (allOtherFlags.Contains(MountFlags.Turret) && allDirectionFlags.Contains(MountFlags.Forward)
                && !allDirectionFlags.Contains(MountFlags.Ventral)
                && !allDirectionFlags.Contains(MountFlags.Aft)
                && !allDirectionFlags.Contains(MountFlags.Port)
                && !allDirectionFlags.Contains(MountFlags.Starbord)
                && !allDirectionFlags.Contains(MountFlags.Dorsal))
            {
                text = asText ? "Forward, Port, Starbord" : "(forwardportstarbord)";
            }
            else if (allOtherFlags.Contains(MountFlags.Turret) && allDirectionFlags.Contains(MountFlags.Aft)
               && !allDirectionFlags.Contains(MountFlags.Ventral)
               && !allDirectionFlags.Contains(MountFlags.Forward)
               && !allDirectionFlags.Contains(MountFlags.Port)
               && !allDirectionFlags.Contains(MountFlags.Starbord)
               && !allDirectionFlags.Contains(MountFlags.Dorsal))
            {
                text = asText ? "Aft, Port, Starbord" : "(portstarbordaft)";
            }
            else if (allOtherFlags.Contains(MountFlags.Wing))
            {
                text = asText ? "Forward" : "(forward)";
            }
            else
            {
                if (asText)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int j = 0; j < allDirectionFlags.Count; j++)
                    {
                        sb.Append(Enum.GetName(typeof(MountFlags), allDirectionFlags[j]));
                        string separatedString = j != allDirectionFlags.Count - 1 ? ", " : string.Empty;
                        sb.Append(separatedString);
                    }

                    text = sb.ToString();
                }
                else
                {
                    if (allDirectionFlags.Contains(MountFlags.Forward)
                        && allDirectionFlags.Contains(MountFlags.Port)
                        && allDirectionFlags.Contains(MountFlags.Starbord)
                        && allDirectionFlags.Contains(MountFlags.Aft))
                    {
                        text = "(all)";
                    }
                    else if (allDirectionFlags.Contains(MountFlags.Forward)
                         && allDirectionFlags.Contains(MountFlags.Port)
                         && allDirectionFlags.Contains(MountFlags.Starbord))
                    {
                        text = "(forwardportstarbord)";
                    }
                    else if (allDirectionFlags.Contains(MountFlags.Forward)
                         && allDirectionFlags.Contains(MountFlags.Port)
                         && allDirectionFlags.Contains(MountFlags.Aft))
                    {
                        text = "(forwardportaft)";
                    }
                    else if (allDirectionFlags.Contains(MountFlags.Forward)
                         && allDirectionFlags.Contains(MountFlags.Starbord)
                         && allDirectionFlags.Contains(MountFlags.Aft))
                    {
                        text = "(forwardstarbordaft)";
                    }
                    else if (allDirectionFlags.Contains(MountFlags.Forward)
                         && allDirectionFlags.Contains(MountFlags.Aft))
                    {
                        text = "(forwardaft)";
                    }
                    else if (allDirectionFlags.Contains(MountFlags.Forward))
                    {
                        text = "(forward)";
                    }
                    else if (allDirectionFlags.Contains(MountFlags.Aft)
                         && allDirectionFlags.Contains(MountFlags.Port)
                         && allDirectionFlags.Contains(MountFlags.Starbord))
                    {
                        text = "(portstarbordaft)";
                    }
                    else if (allDirectionFlags.Contains(MountFlags.Port)
                         && allDirectionFlags.Contains(MountFlags.Starbord))
                    {
                        text = "(portstarbord)";
                    }
                    else if (allDirectionFlags.Contains(MountFlags.Port))
                    {
                        text = "(port)";
                    }
                    else if (allDirectionFlags.Contains(MountFlags.Starbord))
                    {
                        text = "(starbord)";
                    }
                    else if (allDirectionFlags.Contains(MountFlags.Aft))
                    {
                        text = "(aft)";
                    }
                    else
                    {
                        text = "Ist noch nicht integriert";
                    }
                }
            }

            if (mounts[0].numberOfWeapons > 1)
            {
                text += mounts[0].numberOfWeapons;
            }

            return text;
        }

        private void InitializeFlagsFromRemainingShortcut(string mountShortcut)
        {
            mountShortcut = mountShortcut.ToUpper();
            if (mountShortcut.Contains("TU"))
            {
                otherFlags.Add(MountFlags.Turret);
            }

            if (mountShortcut.Contains("RE"))
            {
                otherFlags.Add(MountFlags.Retractable);
            }

            if (mountShortcut.Contains("FO"))
            {
                directionFlags.Add(MountFlags.Forward);
            }

            if (mountShortcut.Contains("AF"))
            {
                directionFlags.Add(MountFlags.Aft);
            }

            if (mountShortcut.Contains("PO"))
            {
                directionFlags.Add(MountFlags.Port);
            }

            if (mountShortcut.Contains("ST"))
            {
                directionFlags.Add(MountFlags.Starbord);
            }

            if (mountShortcut.Contains("DO"))
            {
                directionFlags.Add(MountFlags.Dorsal);
            }

            if (mountShortcut.Contains("VEN"))
            {
                directionFlags.Add(MountFlags.Ventral);
            }

            if (mountShortcut.Contains("ALL"))
            {
                otherFlags.Add(MountFlags.All);
            }

            if (mountShortcut.Contains("HA"))
            {
                otherFlags.Add(MountFlags.HardPoint);
            }

            if (mountShortcut.Contains("WIN"))
            {
                otherFlags.Add(MountFlags.Wing);
            }

            if (mountShortcut.Contains("HU"))
            {
                otherFlags.Add(MountFlags.Hull);
            }

            if (mountShortcut.Contains("TOP"))
            {
                otherFlags.Add(MountFlags.Top);
            }

            if (mountShortcut.Contains("DWN"))
            {
                otherFlags.Add(MountFlags.Down);
            }
        }

        private string GetTextFromNumberOfWeapons()
        {
            string numberOfWeaponsString = string.Empty;
            if (numberOfWeapons != -1)
            {
                if (numberOfWeapons > unitsMap.Length - 1)
                {
                    numberOfWeaponsString = numberOfWeapons + " ";
                }
                else
                {
                    numberOfWeaponsString = unitsMap[numberOfWeapons] + " ";
                }
            }

            return numberOfWeaponsString;
        }

        private string GetTextFromNumberOfLinkedWeapons()
        {
            string numberOfLinkedWeaponsString = string.Empty;
            if (numberOfLinkedWeapons != -1)
            {
                switch (numberOfLinkedWeapons)
                {
                    case 2:
                        numberOfLinkedWeaponsString += " twin";
                        break;
                    case 3:
                        numberOfLinkedWeaponsString += " triple";
                        break;
                    case 4:
                        numberOfLinkedWeaponsString += " quad";
                        break;
                    default:
                        throw new ArgumentException("Number Of Linked Weapons Not Available");
                }
            }

            return numberOfLinkedWeaponsString;
        }
    }
}
