using FastColoredTextBoxNS;
using PgMulti.DataStructure;
using PgMulti.Tasks;
using System;
using System.Collections;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PgMulti
{
    public partial class ChartForm : Form
    {
        public DataGridView DataGridView;
        public Query Query;
        public List<DataGridViewColumn> Columns;

        public ChartForm(DataGridView gv, Query q, List<DataGridViewColumn> cols)
        {
            DataGridView = gv;
            Query = q;
            Columns = cols;

            InitializeComponent();
            InitializeText();

            tscbChartType.Items.Add(new ChartType(Properties.Text.columns, SeriesChartType.Column));
            tscbChartType.Items.Add(new ChartType(Properties.Text.bars, SeriesChartType.Bar));
            tscbChartType.Items.Add(new ChartType(Properties.Text.lines, SeriesChartType.Line));
            tscbChartType.Items.Add(new ChartType(Properties.Text.points, SeriesChartType.Point));
            tscbChartType.Items.Add(new ChartType(Properties.Text.points, SeriesChartType.Pie));

            tscbChartType.SelectedIndex = 0;
        }

        private void Plot(SeriesChartType chartType)
        {
            Query.QueryColumn? qCol0 = (Query.QueryColumn?)Columns[0].Tag;
            bool xAxisAsCategories = qCol0 == null || Query.DataTable.Columns[qCol0.Index].DataType == typeof(string);
            bool xAxisIsDateTime = xAxisAsCategories && qCol0 != null && (Column.DateTypes.Contains(qCol0.PostgreSqlTypeName) || Column.DateTimeTypes.Contains(qCol0.PostgreSqlTypeName));

            if (xAxisIsDateTime) xAxisAsCategories = false;

            chChart.Series.Clear();
            chChart.ChartAreas.Clear();
            chChart.Legends.Clear();

            ChartArea area = new ChartArea();

            area.AxisX.Title = Columns[0].HeaderText;

            chChart.ChartAreas.Add(area);

            Legend legend = new Legend();
            legend.Docking = Docking.Bottom;
            legend.Alignment = StringAlignment.Far;
            chChart.Legends.Add(legend);

            DateTime?[] dateTimes = new DateTime?[Query.DataTable.Rows.Count];
            if (xAxisIsDateTime)
            {
                Regex re = new Regex(@"^(\d+)\-(\d+)\-(\d+)( (\d+)\:(\d+)(\:(\d+)(\.(\d\d\d))?)?)?$");
                for (int i = 0; i < DataGridView.Rows.Count; i++)
                {
                    DataGridViewRow gvRow = DataGridView.Rows[i];
                    DataRow row = ((DataRowView)gvRow.DataBoundItem).Row;
                    
                    object o = (string)row[0];

                    if (o == DBNull.Value)
                    {
                        dateTimes[i] = null;
                    }
                    else
                    {
                        Match ma = re.Match((string)o);

                        int h = ma.Groups[5].Value == "" ? 0 : int.Parse(ma.Groups[5].Value);
                        int m = ma.Groups[6].Value == "" ? 0 : int.Parse(ma.Groups[6].Value);
                        int s = ma.Groups[8].Value == "" ? 0 : int.Parse(ma.Groups[8].Value);
                        int ms = ma.Groups[9].Value == "" ? 0 : int.Parse(ma.Groups[9].Value);

                        dateTimes[i] = new DateTime(int.Parse(ma.Groups[1].Value), int.Parse(ma.Groups[2].Value), int.Parse(ma.Groups[3].Value), h, m, s, ms);
                    }
                }
            }

            Dictionary<string, int> repeatedNames = new Dictionary<string, int>();

            for (int i = 1; i < Columns.Count; i++)
            {
                Query.QueryColumn qColi = (Query.QueryColumn)Columns[i].Tag!;

                string name;

                if (repeatedNames.ContainsKey(qColi.Title))
                {
                    int n = repeatedNames[qColi.Title] + 1;
                    name = qColi.Title + " " + n;
                    repeatedNames[qColi.Title] = n;
                }
                else
                {
                    name = qColi.Title;
                    repeatedNames[qColi.Title] = 1;
                }

                Series serie = new Series
                {
                    Name = name,
                    ChartType = chartType,
                    IsValueShownAsLabel = true,
                    LabelBackColor = Color.FromArgb(200, 255, 255, 255)
                };

                if (xAxisAsCategories)
                {
                    Dictionary<string, int> categories = new Dictionary<string, int>();
                    foreach (DataGridViewRow gvRow in DataGridView.Rows)
                    {
                        DataRow row = ((DataRowView)gvRow.DataBoundItem).Row;
                        string category = row[Columns[0].Index].ToString()!;
                        double value = Convert.ToDouble(row[qColi.Index]);

                        int index;

                        if (categories.ContainsKey(category))
                        {
                            index = categories[category];
                        }
                        else
                        {
                            index = categories.Keys.Count;
                            categories[category] = index;
                        }

                        DataPoint p = serie.Points[serie.Points.AddXY(index, value)];
                        p.AxisLabel = category;
                    }
                }
                else if (xAxisIsDateTime)
                {
                    for (int j = 0; j < DataGridView.Rows.Count; j++)
                    {
                        DataGridViewRow gvRow = DataGridView.Rows[j];
                        DataRow row = ((DataRowView)gvRow.DataBoundItem).Row;

                        serie.Points.AddXY(dateTimes[j], Convert.ToDouble(row[qColi.Index]));
                    }
                }
                else
                {
                    foreach (DataGridViewRow gvRow in DataGridView.Rows)
                    {
                        DataRow row = ((DataRowView)gvRow.DataBoundItem).Row;
                        serie.Points.AddXY(Convert.ToDouble(row[Columns[0].Index]), Convert.ToDouble(row[qColi.Index]));
                    }
                }

                chChart.Series.Add(serie);
            }
        }

        private void tscbChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Plot(((ChartType)tscbChartType.SelectedItem).Type);
        }

        #region TextI18n
        private void InitializeText()
        {
            this.Text = Properties.Text.chart;
        }
        #endregion

        public class ChartType
        {
            public readonly string Name;
            public readonly SeriesChartType Type;

            public ChartType(string name, SeriesChartType type)
            {
                this.Name = name;
                this.Type = type;
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
