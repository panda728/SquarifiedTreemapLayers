using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PopulationDataProvider.Demo;
using SquarifiedTreemapShared;

namespace SquarifiedTreemapForge.WinForms.Demo
{
    public partial class FormMain : Form
    {
        const string SETTING_FLIE = "treemapsettings.{0}.json";

        readonly bool _isInit = true;
        readonly string[] _defaultGroupColumns;
        readonly string _defaultWeightColumn;
        readonly Semaphore _semaphore = new(1, 1);
        readonly IHostEnvironment _hostEnvironment;
        readonly IReadOnlyList<PopulationData> _populationPivots;
        readonly TreemapGdiDriver<PopulationData> _driver;
        readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true, WriteIndented = true };

        public FormMain(
            IHostEnvironment hostEnvironment,
            TreemapGdiDriver<PopulationData> driver,
            PopulationDataProvider.Demo.PopulationDataProvider provider)
        {
            InitializeComponent();

            _hostEnvironment = hostEnvironment;

            toolStripStatusLabel1.Text = "";
            radioLT.Checked = true;
            radioD.Checked = true;
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.SplitterDistance = this.splitContainer1.Width;

            _driver = driver;
            _driver.FuncNodeText = GetTitle;
            _driver.FuncNodeColor = GetColor;
            _driver.TreemapControl = this.treemapControl1;
            _driver.OnMouseMoveAction += treemapControl1_MouseMove;
            _driver.OnMouseLeaveAction += treemapControl1_MouseLeave; ;

            _defaultGroupColumns = _driver.LayoutSettings.GroupColumns;
            _defaultWeightColumn = _driver.LayoutSettings.WeightColumn;

            this.checkShow.Checked = _driver.LegendSettings.IsShowLegend;
            this.checkShowPlusSign.Checked = _driver.LegendSettings.IsShowPlusSign;
            this.checkLegendOrder.Checked = _driver.LegendSettings.IsOrderAsc;
            this.numericLegendSteps.Value = _driver.LegendSettings.StepCount;
            this.numericLegendFontSize.Value = (decimal)_driver.TreemapSettings.LegendFontSize;
            this.numericLegendWidth.Value = _driver.LegendSettings.Width;
            this.numericLegendHeight.Value = _driver.LegendSettings.Height;
            this.numericMinPer.Value = (decimal)(_driver.LegendSettings.MinPer * 100);
            this.numericMaxPer.Value = (decimal)(_driver.LegendSettings.MaxPer * 100);
            this.numericMinBri.Value = (decimal)(_driver.LegendSettings.MinBrightness * 100);
            this.numericMaxBri.Value = (decimal)(_driver.LegendSettings.MaxBrightness * 100);
            this.numericSat.Value = (decimal)(_driver.LegendSettings.Saturation * 100);
            this.numericHuePositive.Value = (decimal)_driver.LegendSettings.HuePositive;
            this.numericHueNegative.Value = (decimal)_driver.LegendSettings.HueNegative;

            this.textBoxTitle.Text = _driver.LayoutSettings.TitleText;
            this.textBoxRootName.Text = _driver.LayoutSettings.RootNodeTitle;

            this.listBoxGroupSelected.Items.Clear();
            this.listBoxGroupSelected.Items.AddRange(_driver.LayoutSettings.GroupColumns);
            this.listBoxGroupSelectable.Items.Clear();

            _populationPivots = provider.GetPopulationPivotsFromJson();
            _isInit = false;
        }

        string GetTitle(string k, IEnumerable<PopulationData> d)
        {
            var total = (double)d.Sum(d => d.TotalPopulation);
            var purchaseTotal = (double)d.Sum(d => d.PopChange5Y);
            var per = purchaseTotal / total;
            return $"{k}({d.Sum(d => d.TotalPopulation) / 1000:#,##0} {per:+0.0%;-0.0%}) ";
        }

        Color GetColor(IEnumerable<PopulationData> d)
        {
            var total = (double)d.Sum(d => d.TotalPopulation);
            var diff = (double)d.Sum(d => d.PopChange5Y);
            var percentage = Math.Round(diff / total * 1000) / 1000;
            return _driver.GetPercentageColor(percentage);
        }

        void Form1_Shown(object sender, EventArgs e)
        {
            _driver.Invalidate(_populationPivots);
        }

        async Task RedrawTreemapAsync()
        {
            if (_isInit || _driver == null) return;
            if (_semaphore.WaitOne(0) == false) return;
            try
            {
                _driver.TreemapSettings = _driver.TreemapSettings with
                {
                    LegendFontSize = (float)numericLegendFontSize.Value,
                };

                _driver.LayoutSettings = _driver.LayoutSettings with
                {
                    TitleText = this.textBoxTitle.Text,
                    LayoutAlign = GetLayout(),
                    IsSourceOrderDec = IsOrderDec(),
                    MaxDepth = (int)numericDepth.Value,
                    RootNodeTitle = this.textBoxRootName.Text,
                    WeightColumn = _defaultWeightColumn,
                    GroupColumns = GetSelectedColumnNames(),
                };

                _driver.LegendSettings = _driver.LegendSettings with
                {
                    MinPer = (double)(this.numericMinPer.Value / 100),
                    MaxPer = (double)(this.numericMaxPer.Value / 100),
                    MinBrightness = (double)(this.numericMinBri.Value / 100),
                    MaxBrightness = (double)(this.numericMaxBri.Value / 100),
                    Saturation = (double)(this.numericSat.Value / 100),
                    HuePositive = (int)this.numericHuePositive.Value,
                    HueNegative = (int)this.numericHueNegative.Value,
                    IsShowLegend = this.checkShow.Checked,
                    IsShowPlusSign = this.checkShowPlusSign.Checked,
                    IsOrderAsc = this.checkLegendOrder.Checked,
                    StepCount = (int)numericLegendSteps.Value,
                    Width = (int)numericLegendWidth.Value,
                    Height = (int)numericLegendHeight.Value,
                };

                _driver.Invalidate(_populationPivots);

                if (!string.IsNullOrEmpty(_driver.LayoutSettings.TitleText))
                {
                    this.Text = _driver.LayoutSettings.TitleText;
                }

                SaveSettingsJson(
                    _driver.TreemapSettings,
                    _driver.LayoutSettings,
                    _driver.LegendSettings);

                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.StackTrace);
                MessageBox.Show(sb.ToString());
            }
            finally
            {
                _semaphore.Release();
            }
        }

        void SaveSettingsJson(
            TreemapSettings treemapSettings,
            TreemapLayoutSettings treemapLayoutSettings,
            LegendSettings legendSettings)
        {
            var json = JsonSerializer.Serialize(new { treemapSettings, treemapLayoutSettings, legendSettings }, _options);
            File.WriteAllText(string.Format(SETTING_FLIE, _hostEnvironment.EnvironmentName), json);
        }

        void treemapControl1_MouseMove(object? sender, MouseEventArgs e)
        {
            var cp = treemapControl1.PointToClient(Cursor.Position);
            var node = _driver.GetContainsItem(cp);
            if (node == null)
            {
                this.toolStripStatusLabel1.Text = $"x={Cursor.Position.X}:y={Cursor.Position.Y}";
            }
            else
            {
                var pathText = string.Join(" -> ", node.GetAllPathTexts().Skip(1));
                this.toolStripStatusLabel1.Text = $"{pathText} (debug info. x={Cursor.Position.X}:y={Cursor.Position.Y} depth:{node.Depth} src:{node.Nodes.Count})";
            }
        }

        void treemapControl1_MouseLeave(object? sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = "";
        }

        #region settings
        bool IsOrderDec() => radioD.Checked;

        LayoutAlign GetLayout()
        {
            if (radioAT.Checked) return LayoutAlign.Alternating;
            if (radioLB.Checked) return LayoutAlign.LeftBottom;
            if (radioRT.Checked) return LayoutAlign.RightTop;
            if (radioRB.Checked) return LayoutAlign.RightBottom;
            return LayoutAlign.LeftTop;
        }

        async void toolStripOpen_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2Collapsed = false;
            this.splitContainer1.SplitterDistance = (int)(this.splitContainer1.Width * 0.55);
            await Task.Delay(100);
            await RedrawTreemapAsync();
        }

        async void toolStripClose_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.SplitterDistance = this.splitContainer1.Width;
            await Task.Delay(100);
            await RedrawTreemapAsync();
        }

        string[] GetSelectedColumnNames()
        {
            var list = new List<string>();
            foreach (object item in listBoxGroupSelected.Items)
            {
                list.Add($"{item}");
            }
            return list.Count == 0 ? _defaultGroupColumns : [.. list];
        }

        async void numeric_ValueChanged(object sender, EventArgs e) => await RedrawTreemapAsync();
        async void radio_CheckedChanged(object sender, EventArgs e) => await RedrawTreemapAsync();
        async void check_CheckedChanged(object sender, EventArgs e) => await RedrawTreemapAsync();

        void buttonSave_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "PNG Files (*.png)|*.png|All Files (*.*)|*.*";
            saveFileDialog1.DefaultExt = "png";
            saveFileDialog1.FileName = "treemap.png";
            saveFileDialog1.AddExtension = true;

            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            try
            {
                using var bmp = _driver.Render(
                    (int)numericWidth.Value,
                    (int)numericHeight.Value
                ) ?? throw new ApplicationException("no data.");
                bmp.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
                MessageBox.Show("saved.", "finish.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"error!! {ex.Message}", "error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static string[] GetSelectedItems(ListBox listBox)
        {
            var list = new List<string>();
            foreach (object item in listBox.SelectedItems)
            {
                list.Add($"{item}");
            }
            return [.. list];
        }

        async void buttonGroupAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var selectables = GetSelectedItems(listBoxGroupSelectable);
                if (selectables.Length == 0) { return; }
                foreach (var s in selectables)
                {
                    if (listBoxGroupSelected.Items.Contains(s) == false)
                    {
                        listBoxGroupSelected.Items.Add(s);
                        listBoxGroupSelectable.Items.Remove(s);
                    }
                }
                await RedrawTreemapAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        async void buttonGroupDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedItems = GetSelectedItems(listBoxGroupSelected);
                if (selectedItems.Length == 0) { return; }
                foreach (var item in selectedItems)
                {
                    listBoxGroupSelected.Items.Remove(item);
                    if (!listBoxGroupSelectable.Items.Contains(item))
                    {
                        listBoxGroupSelectable.Items.Add(item);
                        listBoxGroupSelected.Items.Remove(item);
                    }
                }
                await RedrawTreemapAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        async void buttonGroupUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxGroupSelected.SelectedItem == null || listBoxGroupSelected.SelectedIndex <= 0) return;

                int selectedIndex = listBoxGroupSelected.SelectedIndex;
                var selectedItem = listBoxGroupSelected.SelectedItem;

                listBoxGroupSelected.Items.RemoveAt(selectedIndex);
                listBoxGroupSelected.Items.Insert(selectedIndex - 1, selectedItem);
                listBoxGroupSelected.SelectedIndex = selectedIndex - 1;
                await RedrawTreemapAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        async void buttonGroupDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxGroupSelected.SelectedItem == null || listBoxGroupSelected.SelectedIndex < 0 || listBoxGroupSelected.SelectedIndex >= listBoxGroupSelected.Items.Count - 1) return;

                int selectedIndex = listBoxGroupSelected.SelectedIndex;
                var selectedItem = listBoxGroupSelected.SelectedItem;

                listBoxGroupSelected.Items.RemoveAt(selectedIndex);
                listBoxGroupSelected.Items.Insert(selectedIndex + 1, selectedItem);
                listBoxGroupSelected.SelectedIndex = selectedIndex + 1;
                await RedrawTreemapAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}