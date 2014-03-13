using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AnalizatorNurkowaniaWFA
{
   public partial class MainForm
   {
      private void setSegmentToGrid(int tableRow, Data.DiveSegment segment)
      {
         DataGridViewCell defautCell = gridDiveProfil["initialD", tableRow];
         defautCell.Value = segment.InitialDepth;
         defautCell = gridDiveProfil["finialD", tableRow];
         defautCell.Value = segment.FinialDepth;
         defautCell = gridDiveProfil["time", tableRow];
         defautCell.Value = segment.Time;
         defautCell = gridDiveProfil["runTime", tableRow];
         defautCell.Value = diving.getRunTime(0);
         defautCell = gridDiveProfil["gasName", tableRow];
         defautCell.Value = segment.GasName;
      }
      private void updateChartDiveProfile()
      {
         System.Windows.Forms.DataVisualization.Charting.Series chartDiving = chartDiveProfile.Series[0];
         System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea = chartDiveProfile.ChartAreas.First();
         chartArea.AxisY.Crossing = 0;
         chartArea.AxisX.Title = AnalizatorNurkowaniaWFA.Properties.Resources.timeWithUnit;
         chartArea.AxisY.Title = AnalizatorNurkowaniaWFA.Properties.Resources.depthWithUnit;
         chartArea.AxisX.IsStartedFromZero = true;
         chartArea.AxisY.IsStartedFromZero = true;
         chartArea.AxisY.IsReversed = true;
         chartDiving.Points.Clear();
         int i = 0;
         foreach (Data.DiveSegment ds in diving.SegmentsList)
         {
            if (i == 0)
               chartDiving.Points.AddXY(0, ds.InitialDepth);
            chartDiving.Points.AddXY(diving.getRunTime(i), ds.FinialDepth);
            i++;
         }
      }

      private void updateGridDiveProfile(int selCol, int selRow)
      {
         if (gridDiveProfil.Rows.Count < diving.SegmentsList.Count)
         {
            gridDiveProfil.Rows.Add(diving.SegmentsList.Count - gridDiveProfil.Rows.Count);
         }
         else if (gridDiveProfil.Rows.Count > diving.SegmentsList.Count)
         {
            while (gridDiveProfil.Rows.Count > diving.SegmentsList.Count)
            {
               gridDiveProfil.Rows.RemoveAt(gridDiveProfil.Rows.Count - 1);
            }
         }
         int curentRow = 0;
         foreach (Data.DiveSegment ds in diving.SegmentsList)
         {
            setSegmentToGrid(curentRow, ds);
            curentRow++;
         }
         gridDiveProfil.Rows.Add();
         if (selCol > 0 && selRow > 0)
            gridDiveProfil.CurrentCell = gridDiveProfil[selCol, selRow];
         gridDiveProfil.Refresh();
      }
      private void updateChartAndGridDiveProfile(bool fullGridUpdate = false, int selCol = -1, int selRow = -1)
      {
         if (fullGridUpdate)
            updateGridDiveProfile(selCol, selRow);
         updateRunTime();
         updateChartDiveProfile();

      }
      private void gridDiveProfil_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
      {
         updateChartAndGridDiveProfile(true);
      }
      private void gridDiveProfil_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
      {
         diving.RemoveSegment(e.Row.Index);
      }
      private void gridDiveProfil_CellClick(object sender, DataGridViewCellEventArgs e)
      {
         NewRow(e.RowIndex);
      }
      private void gridDiveProfil_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
      {
         NewRow(e.RowIndex);
      }
      private void NewRow(int rowCount)
      {
         int lastEditableRow = gridDiveProfil.RowCount - 1;
         if (rowCount == lastEditableRow)
         {
            setDefault();
            DataGridViewCell defautCell = gridDiveProfil["finialD", lastEditableRow];
            gridDiveProfil.CurrentCell = defautCell;
            updateChartAndGridDiveProfile();
         }
      }
      private void gridDiveProfil_CellEndEdit(object sender, DataGridViewCellEventArgs e)
      {
         Data.DiveSegment segment = diving.SegmentsList[e.RowIndex];
         string colName = gridDiveProfil.Columns[e.ColumnIndex].Name;
         double iD = Double.Parse(gridDiveProfil["initialD", e.RowIndex].Value.ToString());
         double fD = Double.Parse(gridDiveProfil["finialD", e.RowIndex].Value.ToString());
         double t = Double.Parse(gridDiveProfil["time", e.RowIndex].Value.ToString());
         switch (colName)
         {
            case "gasName":
               string newGasName = (string)gridDiveProfil["gasName", e.RowIndex].Value;
               Data.GasMix newGas = availableGases.Find(g => g.Name == newGasName);
               if (newGas != null)
                  diving.ChangeSegment(segment.Name, iD, fD, t, newGas);
               else
                  throw new Exception("gridDiveProfil_CellEndEdit(object sender, DataGridViewCellEventArgs e): Invalid Gas");
               break;
            case "time":
               diving.ChangeSegment(segment.Name, iD, fD, t);
               updateChartAndGridDiveProfile();
               break;
            case "runTime":
               double rt = Double.Parse(gridDiveProfil["runTime", e.RowIndex].Value.ToString());
               if (e.RowIndex > 0)
               {
                  t = rt - diving.getRunTime(e.RowIndex - 1);
                  if (t < 0)
                     t = 0;
                  diving.ChangeSegment(segment.Name, iD, fD, t);
               }
               else
                  diving.ChangeSegment(segment.Name, iD, fD, rt);
               updateChartAndGridDiveProfile();
               break;
            case "finialD":
            case "initialD":
            default:
               {
                  string newName = diving.ChangeSegment(segment.Name, iD, fD, t);
                  int rowIndex = diving.GetSegmentIndex(newName);
                  updateChartAndGridDiveProfile(true, 0, 0);// e.ColumnIndex, rowIndex);
               }
               break;
         }

      }
      private void updateRunTime()
      {
         int count = gridDiveProfil.Rows.Count;
         for (int i = 0; i < count - 1; i++)
         {
            gridDiveProfil.Rows[i].Cells["runTime"].Value = Math.Ceiling(diving.getRunTime(i));
         }
      }
   }
}
