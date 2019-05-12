using GemBox.Spreadsheet;
using System;

namespace ossServer.Utils
{
    public class XlsUtils
    {
        public static void Mezo(ExcelWorksheet sheet, int sor, int oszlop, object value)
        {
            switch (value)
            {
                case null:
                    sheet.Cells[sor, oszlop].Value = string.Empty;
                    break;
                case string _:
                    sheet.Cells[sor, oszlop].Value = (string)value;
                    break;
                case double _:
                    sheet.Cells[sor, oszlop].Value = (double)value;
                    break;
                case decimal _:
                    sheet.Cells[sor, oszlop].Value = Calc.RealRound((decimal)value, (decimal)0.01);
                    break;
                case int _:
                    sheet.Cells[sor, oszlop].Value = (int)value;
                    break;
                case DateTime _:
                    var datetime = (DateTime)value;
                    if (datetime.Hour == 0 && datetime.Minute == 0 && datetime.Second == 0)
                        sheet.Cells[sor, oszlop].Value = datetime.ToShortDateString();
                    else
                        sheet.Cells[sor, oszlop].Value = datetime.ToString();
                    break;
                case bool _:
                    sheet.Cells[sor, oszlop].Value = (bool)value;
                    break;
                default:
                    throw new NotSupportedException("Mezo");
            }
        }
    }
}
