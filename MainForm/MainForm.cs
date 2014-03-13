using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AnalizatorNurkowaniaWFA
{
    public partial class MainForm : Form
    {
        public List<Data.GasMix> availableGases;
        Data.Diving diving;
        InputOutput.IO io;
        internal userProperties userProp;
        Calculation calculation;
        FormParams formParams;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Initialize();
        }
        private void Initialize()
        {
            formParams = new FormParams();
            userProp = new userProperties();
            availableGases = new List<Data.GasMix>();
            io = new InputOutput.IO();
            availableGases = io.LoadGases();
            foreach (Data.GasMix gas in availableGases)
            {
                gasName.Items.Add(gas.Name);
            }
            diving = new Data.Diving();
            diving.AddGas(availableGases[0]);
            UserToDivigProperties();
            gridDiveProfil.Rows.Add(2);
            setDefault();
            System.Windows.Forms.DataVisualization.Charting.Series chartDiving = chartDiveProfile.Series[0];
            chartDiving.Name = "nurkowanie";
            chartDiving.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            updateChartDiveProfile();

        }
        internal void UserToDivigProperties()
        {
            diving.AscentSpeed = userProp.AscentSpeed;
            diving.AtmosphericPressure = userProp.AtmosphericPressure;
            diving.DescentSpeed = userProp.DescentSpeed;
            diving.WaterSpecificWeight = userProp.WaterSpecificWeight;
        }
        private void setDefault()
        {
           diving.AddDefaultSegment();
           updateGridDiveProfile(-1, -1);
        }
        private void setSegmentToGrid(int tableRow, Data.DiveSegment segment)
        {
           System.Windows.Forms.DataGridViewCell defautCell = gridDiveProfil["initialD", tableRow];
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
        private void gridDiveProfil_UserDeletedRow(object sender, System.Windows.Forms.DataGridViewRowEventArgs e)
        {
           updateChartAndGridDiveProfile(true);
        }
        private void gridDiveProfil_UserDeletingRow(object sender, System.Windows.Forms.DataGridViewRowCancelEventArgs e)
        {
           diving.RemoveSegment(e.Row.Index);
        }
        private void gridDiveProfil_CellClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
           NewRow(e.RowIndex);
        }
        private void gridDiveProfil_CellBeginEdit(object sender, System.Windows.Forms.DataGridViewCellCancelEventArgs e)
        {
           NewRow(e.RowIndex);
        }
        private void NewRow(int rowCount)
        {
           int lastEditableRow = gridDiveProfil.RowCount - 1;
           if (rowCount == lastEditableRow)
           {
              setDefault();
              System.Windows.Forms.DataGridViewCell defautCell = gridDiveProfil["finialD", lastEditableRow];
              gridDiveProfil.CurrentCell = defautCell;
              updateChartAndGridDiveProfile();
           }
        }
        private void gridDiveProfil_CellEndEdit(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
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
        private void zapiszNurkowanieToolStripMenuItem_Click(object sender, EventArgs e)
        {
           SaveToXML(InputOutput.IO.DataType.All);
        }
        private void SaveToXML(InputOutput.IO.DataType dataType)
        {
           bool save = false;
           object toSave = null;
           switch (dataType)
           {
              case InputOutput.IO.DataType.All:
                 saveFile.Filter = "Plik danych nurkowania (*.dxml)|*.dxml |Plik xml (*.xml)|*.xml";
                 toSave = diving;
                 save = true;
                 break;
              case InputOutput.IO.DataType.Gases:
                 saveFile.Filter = "Plik listy gazów (*.gxml)|*.gxml |Plik xml (*.xml)|*.xml";
                 toSave = availableGases;
                 save = true;
                 break;
              case InputOutput.IO.DataType.Compartments:
                 saveFile.Filter = "Plik tkanek (*.cxml)|*.cxml |Plik xml (*.xml)|*.xml";
                 save = true;
                 toSave = calculation.compartments;
                 break;
              default:
                 break;
           }
           if(save)
           {
               saveFile.ShowDialog();
               string path = saveFile.FileName;
               if (path.Length != 0)
               {
                  if (!io.SaveToXML(path, toSave))
                    MessageBox.Show("Błąd podczas zapisu pliku");
               }
           }
        }
        private void wczytajNurkowanieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile.Filter = "Plik danych nurkowania (*.dxml)|*.dxml";
            openFile.ShowDialog();
            string path = openFile.FileName;
            if (path.Length != 0)
            {
                if (!io.OpenDivingXML(path, ref diving))
                    MessageBox.Show("Błąd podczas zapisu pliku");
                else
                    updateChartAndGridDiveProfile(true,0,0);
            }
        }

        private void tabControl1_Selected(object sender, EventArgs e)
        {
            string curentTab = ((System.Windows.Forms.TabControl)(sender)).SelectedTab.Text;
            switch (curentTab)
            {
                default:
                    throw new Exception("Zła nazwa zakładki !");
                case "Profil nurkowania":
                    break;
                case "Opis tkanek":
                    calculation = new Calculation(ref diving, userProp.compartmentType);
                    modifCompartmentsDefinition();
                    break;
                case "Wynurzenie":
                    break;
                case "Nasycenia tkanek":
                    calculation = new Calculation(ref diving, userProp.compartmentType);
                    calculation.Run();
                    modifChartCompartments();
                    break;
            }
        }
        private void modifCompartmentsDefinition()
        {
            int i=0;
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea = chartCompartmentsDefinition.ChartAreas.First();
            chartArea.AxisY.Crossing = 0;
            chartArea.AxisX.Title = AnalizatorNurkowaniaWFA.Properties.Resources.MValue;
            chartArea.AxisY.Title = AnalizatorNurkowaniaWFA.Properties.Resources.depthWithUnit;
            chartArea.AxisY.IsStartedFromZero = true;
            chartArea.AxisY.IsReversed = true;
            System.Windows.Forms.DataVisualization.Charting.Series series;
            chartCompartmentsDefinition.Series.Clear();
            double maxD = diving.DepthMax();
            double minD = diving.DepthAtPresure(diving.AtmosphericPressure);
            dataGridCompartmentsDefinition.Rows.Clear();
            chartCompartmentsDefinition.Series.Clear();
            chartCompartmentsDefinition.Titles.Clear();
            double maxX = 0;
            double minX = Double.MaxValue;
            foreach (Compartments.Compartment c in calculation.compartments.compartment )
            {
                dataGridCompartmentsDefinition.Rows.Add();
                dataGridCompartmentsDefinition.Rows[i].Cells["CompartmentName"].Value = c.Name;
                dataGridCompartmentsDefinition.Rows[i].Cells["CompartmentM0"].Value = c.M0N2;
                dataGridCompartmentsDefinition.Rows[i].Cells["CompartmentdM"].Value = c.dMN2;
                dataGridCompartmentsDefinition.Rows[i].Cells["CompartmentHT"].Value = c.HalfTimeN2;
                dataGridCompartmentsDefinition.Rows[i].Cells["CompartmentRemark"].Value = "";
                chartCompartmentsDefinition.Series.Add(c.Name);  
                series = chartCompartmentsDefinition.Series.Last();
                series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                series.Points.AddXY(c.dMN2 * maxD + c.M0N2, maxD);
                series.Points.AddXY(c.dMN2 * minD + c.M0N2, minD);
                maxX = Math.Max(maxX, c.dMN2 * maxD + c.M0N2);
                minX = Math.Min(minX, c.dMN2 * minD + c.M0N2);
                i++;
            }
            chartArea.AxisX.IsLabelAutoFit = false;
            chartCompartmentsDefinition.Titles.Add("1").Text = calculation.compartments.Description;
            chartArea.AxisX.Maximum = (Math.Floor(maxX) + 1.0);
            chartArea.AxisX.Minimum = (Math.Ceiling(minX) - 1.0);
            chartArea.AxisX.Crossing = chartArea.AxisX.Minimum;
            chartArea.AxisX.MajorGrid.Interval = Math.Ceiling((0.2 * (chartArea.AxisX.Maximum - chartArea.AxisX.Minimum)));
            chartArea.AxisX.LabelStyle.Interval=chartArea.AxisX.MajorGrid.Interval;
            chartArea.AxisX.Interval = chartArea.AxisX.MajorGrid.Interval;
        }
        private void modifChartCompartments()
        {
           List<Result.PPraeasureForCompartment> calculationResult = calculation.result;
            Compartments.ListOfCompartment compartments = calculation.compartments;
            chartCompartments.Series.Clear();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea = chartCompartments.ChartAreas.First();
            chartArea.AxisY.Crossing = 0;
            chartArea.AxisX.IsStartedFromZero = true;
            chartArea.AxisY.IsReversed = true;
            System.Windows.Forms.DataVisualization.Charting.Series series;
            int i = 0;
            checkPoints.Enabled = false;
            foreach (Compartments.Compartment c in compartments.compartment)
            {
                chartCompartments.Series.Add(c.Name);
                series = chartCompartments.Series.Last();
                series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                foreach (Result.PPraeasureForCompartment ppfc in calculationResult)
                {
                    series.Points.AddXY(ppfc.Time, ppfc.getN2(i));
                }
                i++;
            }
            if (checkPPN2.Checked)
            {
                checkPoints.Enabled = true;
                chartCompartments.Series.Add("pPN2");
                series = chartCompartments.Series.Last();
                if(checkPoints.Checked)
                    series.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Star10;
                else
                    series.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.None;
                series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                foreach (Result.PPraeasureForCompartment ppfc in calculationResult)
                {
                    series.Points.AddXY(ppfc.Time, ppfc.PreasureN2);
                }
            }
            if (checkAP.Checked)
            {
                checkPoints.Enabled = true;
                chartCompartments.Series.Add("aP");
                series = chartCompartments.Series.Last();
                if (checkPoints.Checked)
                    series.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Star5;
                else
                    series.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.None;
                series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                foreach (Result.PPraeasureForCompartment ppfc in calculationResult)
                {
                    series.Points.AddXY(ppfc.Time, ppfc.AmbientPreasure);
                }
            }
            if (checkCeiling.Checked)
            {
                checkPoints.Enabled = true;
                chartCompartments.Series.Add("sufit dekompresyjny");
                series = chartCompartments.Series.Last();
                if (checkPoints.Checked)
                    series.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Cross;
                else
                    series.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.None;
                series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                foreach (Result.PPraeasureForCompartment ppfc in calculationResult)
                {
                    double minCeiling = -Double.MaxValue;
                    i =0;
                    foreach (Compartments.Compartment c in compartments.compartment)
                    {
                        minCeiling = Math.Max(minCeiling,ppfc.getCeiling(i));
                        i++;
                    }
                    series.Points.AddXY(ppfc.Time, minCeiling);
                }
            }
            numericUpDownCzas.Maximum = (decimal)calculationResult.Last().Time;
            if (numericUpDownCzas.Maximum < 1)
            {
                numericUpDownCzas.Value = 0;
                numericUpDownCzas.Enabled = false;
            }
            else
            {
                numericUpDownCzas.Value = 1;
                numericUpDownCzas.Enabled = true;
            }
            compartmentChart();
            chartArea.AxisX.IsLabelAutoFit = false;
            chartCompartments.Titles.Clear();
            chartCompartments.Titles.Add("0").Text = calculation.compartments.Description;
            chartArea.AxisX.Maximum = (Math.Floor(calculationResult.Last().Time) + 1.0);
            chartArea.AxisX.Minimum = 0;
            chartArea.AxisX.Crossing = 0;
            chartArea.AxisX.MajorGrid.Interval = Math.Ceiling((0.1 * (chartArea.AxisX.Maximum - chartArea.AxisX.Minimum)));
            chartArea.AxisX.LabelStyle.Interval = chartArea.AxisX.MajorGrid.Interval;
            chartArea.AxisX.Interval = chartArea.AxisX.MajorGrid.Interval;
            chartArea.AxisX.Title = AnalizatorNurkowaniaWFA.Properties.Resources.timeWithUnit;
            chartArea.AxisY.Title = AnalizatorNurkowaniaWFA.Properties.Resources.preasureBar;
        }
        private void ustawieniaNurkowaniaToolStripMenuItem_Click(object sender, EventArgs e)
        {
           formParams.SetActiveTab(FormParams.tabs.dive);
           if (formParams.UpdateUserData(ref userProp))
           {
              diving.AscentSpeed =  userProp.AscentSpeed;
              diving.DescentSpeed = userProp.DescentSpeed;
              diving.AtmosphericPressure =  userProp.AtmosphericPressure;
              diving.WaterSpecificWeight =  userProp.WaterSpecificWeight;
              calculation = new Calculation(ref diving, userProp.compartmentType);
              modifCompartmentsDefinition();
           }
        }
        private void parametryDomyślneToolStripMenuItem_Click(object sender, EventArgs e)
        {
           formParams.SetActiveTab(FormParams.tabs.deco);
           if (formParams.UpdateUserData(ref userProp))
           {
              diving.AscentSpeed = userProp.AscentSpeed;
              diving.DescentSpeed = userProp.DescentSpeed;
              diving.AtmosphericPressure = userProp.AtmosphericPressure;
              diving.WaterSpecificWeight = userProp.WaterSpecificWeight;
              calculation = new Calculation(ref diving, userProp.compartmentType);
              modifCompartmentsDefinition();
           }
        }
        // Excel XML z menu
        private void nasyceniaTkanekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveToExcel(InputOutput.IO.DataType.Compartments);
        }
        private void gazyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveToExcel(InputOutput.IO.DataType.Gases);
        }
        private void profilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveToExcel(InputOutput.IO.DataType.Profile);
        }
        private void wszystkoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveToExcel(InputOutput.IO.DataType.All);
        }
        private void SaveToExcel(InputOutput.IO.DataType dataType)
        {
            saveFile.Filter = "Arkusz kalkulacyjny XMl 2003 (*.xml)|*.xml";
            saveFile.ShowDialog();
            string path = saveFile.FileName;
            if (path.Length != 0)
            {
                if (diving != null && calculation != null)
                {
                    if (!io.SaveToExcel(path, dataType, diving, calculation.compartments, calculation.result))
                        MessageBox.Show("Błąd podczas zapisu pliku");
                }
                else
                    MessageBox.Show("Eksport do Excel możliwy po obliczeniach. Przejdz na zakładkę z rezulatatami dla tkanek a nastepnie wyeksportuj dane");
            }
        }
        // Koniec *Excel XML z menu*
        private void numericUpDownCzas_ValueChanged(object sender, EventArgs e)
        {
            compartmentChart();
        } 
        private void compartmentChart()
        {
            double czas = (double)numericUpDownCzas.Value;
            Result.PPraeasureForCompartment ppfc = calculation.GetMaxValues(false);
            if (czas > ppfc.Time)
            {
               numericUpDownCzas.Value = (decimal)Math.Floor(ppfc.Time);
               czas = (double)numericUpDownCzas.Value;
            }
            double max = 0;
            int count = ppfc.Count();
            for (int i = 0; i < count; i++)
            {
               max = Math.Max(max, ppfc.getN2(i));
            }
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea = CompartmrntAtTime.ChartAreas.First();
            chartArea.AxisY.Maximum = max;
            chartArea.AxisY.Crossing = 0;
            ppfc = calculation.GetValusAtTime(czas, false);
            textBoxCisnienie.Text = ppfc.AmbientPreasure.ToString("0.0");
            textBoxGlebokosc.Text = ppfc.Depth.ToString("0.0");
            textBoxPPN2.Text = ppfc.PreasureN2.ToString("0.0");
            int compartmentNo = ppfc.Count();
            CompartmrntAtTime.Series.Clear();
            CompartmrntAtTime.Series.Add("Cisnienia w tkankach. " + calculation.compartments.Description );
            System.Windows.Forms.DataVisualization.Charting.Series series = CompartmrntAtTime.Series.First();
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            for(int i=0; i<compartmentNo; i++)
            {
                series.Points.AddY(ppfc.getN2(i));
                series.Points[i].AxisLabel = calculation.compartments.compartment[i].HalfTimeN2.ToString("0.0");

            }
        }

        private void checkPoints_CheckedChanged(object sender, EventArgs e)
        {
            modifChartCompartments();
        }

        private void checkPPN2_CheckedChanged(object sender, EventArgs e)
        {
            modifChartCompartments();
        }

        private void checkAP_CheckedChanged(object sender, EventArgs e)
        {
            modifChartCompartments();
        }

        private void checkCeiling_CheckedChanged(object sender, EventArgs e)
        {
            modifChartCompartments();
        }

    }
    public class userProperties
    {
        public Compartments.CompartmentsType compartmentType {get; set;}
        public double WaterSpecificWeight { get; set; }
        public double AtmosphericPressure { get; set; }
        public double DescentSpeed { get; set; }
        public double AscentSpeed { get; set; }
        public userProperties()
        {
            compartmentType = Compartments.CompartmentsType.ZHL16B;
            AtmosphericPressure = 1.0; 
            WaterSpecificWeight = 1000.0; 
            DescentSpeed = 20.0;
            AscentSpeed = 10.0;
        }
    }
}
