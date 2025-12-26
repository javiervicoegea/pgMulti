using FastColoredTextBoxNS;
using PgMulti.DataStructure;
using PgMulti.Tasks;
using System;
using System.Collections;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms.DataVisualization.Charting;

namespace PgMulti
{
    public partial class ChartForm : Form
    {
        public Query Query;
        public List<Query.QueryColumn> Columns;

        public ChartForm(Query q, List<Query.QueryColumn> cols)
        {
            Query = q;
            Columns = cols;

            InitializeComponent();
            InitializeText();

            tscbChartType.Items.Add(new ChartType(Properties.Text.columns, SeriesChartType.Column));
            tscbChartType.Items.Add(new ChartType(Properties.Text.bars, SeriesChartType.Bar));
            tscbChartType.Items.Add(new ChartType(Properties.Text.lines, SeriesChartType.Line));
            tscbChartType.Items.Add(new ChartType(Properties.Text.points, SeriesChartType.Point));

            tscbChartType.SelectedIndex = 0;
        }

        private void Plot(SeriesChartType chartType)
        {
            bool xAxisAsCategories = Query.DataTable.Columns[Columns[0].Index].DataType == typeof(string);
            bool xAxisIsDateTime = xAxisAsCategories && (Column.DateTypes.Contains(Columns[0].PostgreSqlTypeName) || Column.DateTimeTypes.Contains(Columns[0].PostgreSqlTypeName));

            if (xAxisIsDateTime) xAxisAsCategories = false;

            chChart.Series.Clear();
            chChart.ChartAreas.Clear();
            chChart.Legends.Clear();

            ChartArea area = new ChartArea();

            area.AxisX.Title = Columns[0].Title;

            chChart.ChartAreas.Add(area);

            Legend legend = new Legend();
            legend.Docking = Docking.Bottom;
            legend.Alignment = StringAlignment.Far;
            chChart.Legends.Add(legend);

            DateTime?[] dateTimes = new DateTime?[Query.DataTable.Rows.Count];
            if (xAxisIsDateTime)
            {
                Regex re = new Regex(@"^(\d+)\-(\d+)\-(\d+)( (\d+)\:(\d+)(\:(\d+)(\.(\d\d\d))?)?)?$");
                for (int i = 0; i < Query.DataTable.Rows.Count; i++)
                {
                    object o = (string)Query.DataTable.Rows[i][0];

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

            for (int i = 1; i < Columns.Count; i++)
            {
                Series serie = new Series
                {
                    Name = Columns[i].Title,
                    ChartType = chartType,
                    XValueMember = Query.DataTable.Columns[Columns[0].Index].ColumnName,
                    YValueMembers = Query.DataTable.Columns[Columns[i].Index].ColumnName,
                    IsValueShownAsLabel = true,
                    LabelBackColor = Color.FromArgb(200, 255, 255, 255)
                };

                if (xAxisAsCategories)
                {
                    Dictionary<string, int> categories = new Dictionary<string, int>();
                    foreach (DataRow row in Query.DataTable.Rows)
                    {
                        string category = row[Columns[0].Index].ToString()!;
                        double value = Convert.ToDouble(row[Columns[i].Index]);

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
                    for (int j = 0; j < Query.DataTable.Rows.Count; j++)
                    {
                        DataPoint p = serie.Points[serie.Points.AddXY(dateTimes[j], Convert.ToDouble(Query.DataTable.Rows[j][Columns[i].Index]))];
                    }
                }

                chChart.Series.Add(serie);
            }

            if (!xAxisAsCategories && !xAxisIsDateTime)
            {
                chChart.DataSource = Query.DataTable;
                chChart.DataBind();
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
