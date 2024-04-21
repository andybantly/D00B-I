using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

// Add a COM reference to "Microsoft Excel ##.# Object"
namespace D00B
{
    static class ExportListView
    {
        static void Release(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to release the object " + ex.ToString());
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }

        static public bool ExportToExcel(CArray Arr, List<KeyValuePair<string, bool>> Header, List<int> m_ColumnAlignment, List<string> m_ColumnFormatString, List<IFormatProvider> m_ColumnFormatProvider, string strTableName, out double dDuration)
        {
            dDuration = 0.0;
            DateTime st = DateTime.Now;
            bool bRet = true;

            int nColLength = 0;
            int iCol = -1;
            for (iCol = 0; iCol < Arr.ColLength; ++iCol)
                if (Header[iCol].Value)
                    nColLength++;
            if (nColLength == 0)
                return true;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Microsoft.Office.Interop.Excel.Application oXL;
                Microsoft.Office.Interop.Excel._Workbook oWB;
                Microsoft.Office.Interop.Excel._Worksheet oSheet;

                oXL = new Microsoft.Office.Interop.Excel.Application { SheetsInNewWorkbook = 1 };
                oWB = oXL.Workbooks.Add(Missing.Value);
                oXL.ScreenUpdating = false;
                oXL.Calculation = Microsoft.Office.Interop.Excel.XlCalculation.xlCalculationManual;
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.Sheets[1];

                try
                {
                    int idx;
                    object[,] oArray = new object[Arr.RowLength + 1, nColLength];
                    for (iCol = 0, idx = 0; iCol < Arr.ColLength; ++iCol)
                        if (Header[iCol].Value)
                            oArray[0, idx++] = Header[iCol].Key;

                    for (int iRow = 0; iRow < Arr.RowLength; ++iRow)
                        for (iCol = 0, idx = 0; iCol < Arr.ColLength; ++iCol)
                            if (Header[iCol].Value)
                                oArray[iRow + 1, idx++] = Arr[iCol][iRow].ToString(m_ColumnAlignment[iCol], m_ColumnFormatString[iCol], m_ColumnFormatProvider[iCol]);

                    Range Column1 = oSheet.Cells[1, 1];
                    Range Column2 = oSheet.Cells[Arr.RowLength + 1, nColLength];
                    Range Rng = oSheet.Range[Column1, Column2];
                    Rng.Value = oArray;

                    try { oWB.Worksheets[1].Delete(); } catch { Console.WriteLine("Error Deleting Sheet1"); }
                    for (int iSheet = 1; iSheet <= oWB.Sheets.Count; iSheet++)
                        oWB.Sheets[iSheet].Columns.AutoFit();
                    string strSheetName = strTableName;
                    strSheetName = strSheetName.Substring(0, Math.Min(strSheetName.Length, 31));
                    oWB.Sheets[1].Name = strSheetName;
                    oWB.Worksheets[1].Activate();
                    oSheet.Cells[1, 1].Select();

                    oXL.ScreenUpdating = true;
                    oXL.Visible = true;
                    oXL.UserControl = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Release(oSheet);
                    Release(oWB);
                    Release(oXL);
                    bRet = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bRet = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

            DateTime et = DateTime.Now;
            dDuration = (et - st).TotalSeconds;

            return bRet;
        }
    }
}