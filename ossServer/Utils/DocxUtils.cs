using GemBox.Document;
using GemBox.Document.Tables;
using System.Linq;

namespace ossServer.Utils
{
    public class DocxUtils
    {
        public static void SetCellValue(TableRow tr, int ci, string value)
        {
            Paragraph p = tr.Cells[ci].Blocks.Cast<Paragraph>().First();
            ((Run)p.Inlines[0]).Text = value;
        }

        public static void SetCellBorder(TableRow tr, MultipleBorderTypes bt, int istart, int istop)
        {
            for (var i = istart; i <= istop; i++)
                tr.Cells[i].CellFormat.Borders.SetBorders(bt, BorderStyle.Single, Color.Black, 1);
        }
    }
}
