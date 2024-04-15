using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace KFA.SubSystem.Services.Services;

  internal static class MsExcel
  {
      public static DataSet ExcelToDataTable(string path, ref List<string> errorList, bool hasHeaderRow = true)
      {
          DataSet ds = new();
          errorList = new List<string>();

          //create a new Excel package           
          using var excelPackage = new ExcelPackage();
          using var stream = File.OpenRead(path);
          excelPackage.Load(stream);

          foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
          {
              var dt = ds.Tables.Add(worksheet.Name);
              //check if the worksheet is completely empty
              if (worksheet.Dimension == null)
                  continue;

              //add the columns to the datatable
              for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
              {
                  string columnName = "Column " + j;


                  //Build hashset with all types in the row
                  var columnTypes = new HashSet<Type>();
                  for (int i = worksheet.Dimension.Start.Row + Convert.ToInt32(hasHeaderRow); i <= worksheet.Dimension.End.Row; i++)
                  {
                      //Only add type if cell value not empty
                      if (worksheet.Cells[i, j].Value != null)
                      {
                          columnTypes.Add(worksheet.Cells[i, j].Value.GetType());
                      }
                  }

                  var excelCell = worksheet.Cells[1, j].Value;


                  if (excelCell != null)
                  {
                      Type? excelCellDataType = null;

                      //if there is a headerrow, set the next cell for the datatype and set the column name
                      if (hasHeaderRow == true)
                      {

                          columnName = excelCell?.ToString() ?? string.Empty;

                          //check if the column name already exists in the datatable, if so make a unique name
                          if (dt.Columns.Contains(columnName) == true)
                          {
                              columnName = columnName + "_" + j;
                          }
                      }

                      //Select  input type for the column
                      if (columnTypes.Count == 1)
                      {
                          excelCellDataType = columnTypes.First();
                      }
                      else
                      {
                          excelCellDataType = typeof(string);
                      }

                      //try to determine the datatype for the column (by looking at the next column if there is a header row)
                      if (excelCellDataType == typeof(DateTime))
                      {
                          dt.Columns.Add(columnName, typeof(DateTime));
                      }
                      else if (excelCellDataType == typeof(bool))
                      {
                          dt.Columns.Add(columnName, typeof(bool));
                      }
                      else if (excelCellDataType == typeof(double))
                      {
                          //determine if the value is a decimal or int by looking for a decimal separator
                          //not the cleanest of solutions but it works since excel always gives a double
                          if (excelCellDataType.ToString().Contains(',') || excelCellDataType.ToString().Contains(','))
                          {
                              dt.Columns.Add(columnName, typeof(decimal));
                          }
                          else
                          {
                              dt.Columns.Add(columnName, typeof(long));
                          }
                      }
                      else
                      {
                          dt.Columns.Add(columnName, typeof(string));
                      }
                  }
                  else
                  {
                      dt.Columns.Add(columnName, typeof(string));
                  }
              }

              //start adding data the datatable here by looping all rows and columns
              for (int i = worksheet.Dimension.Start.Row + Convert.ToInt32(hasHeaderRow); i <= worksheet.Dimension.End.Row; i++)
              {
                  //create a new datatable row
                  DataRow row = dt.NewRow();

                  //loop all columns
                  for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                  {
                      var excelCell = worksheet.Cells[i, j].Value;

                      //add cell value to the datatable
                      if (excelCell != null)
                      {
                          try
                          {
                              row[j - 1] = excelCell;
                          }
                          catch
                          {
                              errorList.Add("Row " + (i - 1) + ", Column " + j + ". Invalid " + dt.Columns[j - 1].DataType.ToString().Replace("System.", "") + " value:  " + excelCell.ToString());
                          }
                      }
                  }
                  //add the new row to the datatable
                  dt.Rows.Add(row);
              }
          }

          return ds;
      }
  }
