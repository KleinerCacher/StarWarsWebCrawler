using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace WebCrawler
{
    public static class ExcelGenerator
    {
        public static void GenerateExcelFile(string filename, List<IndexData> data)
        {
            if (data.Count > 0)
            {
                FileInfo newFile = new FileInfo(filename);
                if (newFile.Exists)
                {
                    newFile.Delete();
                }

                ExcelPackage pck = new ExcelPackage(newFile);
                //Add the Content sheet
                var worksheet = pck.Workbook.Worksheets.Add("Content");

                // Header Columns
                worksheet.Cells["A1"].Value = "Name";
                PropertyInfo[] propertyInfos = data[0].GetType().GetProperties();

                int column = 1;
                foreach (PropertyInfo info in propertyInfos)
                {
                    // Header
                    if (!info.Name.Equals("Name"))
                    {
                        column++;
                        worksheet.Cells[1, column].Value = info.Name;
                    }
                }

                int row = 2;
                foreach (IndexData item in data)
                {
                    column = 2;
                    foreach (PropertyInfo info in propertyInfos)
                    {
                        if (info.Name.Equals("Name"))
                        {
                            worksheet.Cells[row, 1].Value = info.GetValue(item);
                        }
                        else
                        {
                            var value = info.GetValue(item);
                            if (value is int)
                            {
                                worksheet.Cells[row, column].Value = value;
                            }
                            else if (value is List<Weapon>)
                            {
                                string weaponListText = GetWeaponListText(value as List<Weapon>);
                                worksheet.Cells[row, column].Value = weaponListText;
                            }
                            else
                            {
                                int number;
                                string valueAsString = value as string;
                                if (int.TryParse(valueAsString, out number))
                                {
                                    worksheet.Cells[row, column].Value = number;
                                }
                                else
                                {
                                    worksheet.Cells[row, column].Value = valueAsString;
                                }
                            }

                            column++;
                        }
                    }

                    row++;
                }

                pck.Save();
            }
        }

        private static string GetWeaponListText(List<Weapon> list)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append(GetWeaponText(list[i]));

                if (i != list.Count - 1)
                {
                    sb.AppendLine();
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        private static string GetWeaponText(Weapon weapon)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} Range ", weapon.Range);
            sb.AppendFormat("({0} {1})", weapon.FireArcDescription, weapon.Name);
            sb.AppendLine();
            sb.AppendFormat("Fire Arc {0}; Damage {1}, Critical Hit {2}",
                                weapon.FireArc, weapon.Damage, weapon.Criticalhit);

            if (weapon.WeaponQualities.Count > 0)
            {
                sb.Append("; ");
            }

            for (int i = 0; i < weapon.WeaponQualities.Count; i++)
            {
                sb.Append(GetWeaponQualityText(weapon.WeaponQualities[i]));

                if (i != weapon.WeaponQualities.Count - 1)
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
        }

        private static string GetWeaponQualityText(WeaponQuality quality)
        {
            if (quality.Value == 0)
            {
                return quality.Name;
            }
            else
            {
                return quality.Name + " " + quality.Value;
            }
        }
    }
}
