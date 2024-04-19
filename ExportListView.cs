using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Reflection;
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

        static public bool ExportToExcel(CArray Arr, string[] Header, List<KeyValuePair<int, string>> ColumnFormatList, string strTableName, out double dDuration)
        {
            dDuration = 0.0;
            DateTime st = DateTime.Now;
            bool bRet = true;
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
                    object[,] oArray = new object[Arr.RowLength + 1, Arr.ColLength];

                    int iCol = -1;
                    for (iCol = 0; iCol < Arr.ColLength; ++iCol)
                        oArray[0, iCol] = Header[iCol];

                    for (int iRow = 0; iRow < Arr.RowLength; ++iRow)
                        for (iCol = 0; iCol < Arr.ColLength; ++iCol)
                            oArray[iRow + 1, iCol] = Arr[iCol][iRow].ToString(ColumnFormatList[iCol].Key, ColumnFormatList[iCol].Value);

                    Range Column1 = oSheet.Cells[1, 1];
                    Range Column2 = oSheet.Cells[Arr.RowLength + 1, Arr.ColLength];
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