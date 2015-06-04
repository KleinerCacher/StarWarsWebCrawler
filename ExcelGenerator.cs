using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

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
    }
}
