using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SquarifiedTreemapForge.WinForms;
using SquarifiedTreemapForge.Shared;

namespace SquarifiedTreemapWinForms
{
    public partial class FormMain : Form
    {
        const string SETTING_FLIE = "treemapsettings.{0}.json";

        readonly bool _isInit = true;
        readonly AppSettings _appSettings;
        readonly string[] _defaultGroupColumns;
        readonly string _defaultWeightColumn;
        readonly Semaphore _semaphore = new(1, 1);
        readonly IHostEnvironment _hostEnvironment;
        readonly TreemapGdiDriver<PivotDataSource> _driver;
        readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true, WriteIndented = true };

        public FormMain(
            IHostEnvironment hostEnvironment,
            IOptions<AppSettings> appSettingsOp,
            TreemapGdiDriver<PivotDataSource> driver)
        {
            InitializeComponent();

            _hostEnvironment = hostEnvironment;
            _appSettings = appSettingsOp.Value;

            toolStripStatusLabel1.Text = "";
            radioLT.Checked = true;
            radioD.Checked = true;
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.SplitterDistance = this.splitContainer1.Width;

            _driver = driver;
            _driver.FuncNodeText = PivotDataSource.GetTitle;
            _driver.FuncPercentage = PivotDataSource.GetPercentage;
            _driver.DrawLeafNode = DrawLeafNode;
            _driver.OnMouseMoveAction += treemapControl1_MouseMove;
            _driver.OnMouseLeaveAction += treemapControl1_MouseLeave;
            _driver.TreemapControl = this.treemapControl1;

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
            this.numericDepth.Value = _driver.LayoutSettings.MaxDepth;
            this.numericDispDepth.Value = _driver.LayoutSettings.DisplayMaxDepth;
            this.numericExplode.Value = _driver.LayoutSettings.ExplodeGaps.FirstOrDefault();
            this.numericSat.Value = (decimal)(_driver.LegendSettings.Saturation * 100);
            this.numericHuePositive.Value = (decimal)_driver.LegendSettings.HuePositive;
            this.numericHueNegative.Value = (decimal)_driver.LegendSettings.HueNegative;

            this.textBoxTitle.Text = _driver.LayoutSettings.TitleText;
            this.textBoxRootName.Text = _driver.LayoutSettings.RootNodeTitle;

            this.listBoxGroupSelected.Items.Clear();
            this.listBoxGroupSelected.Items.AddRange(_driver.LayoutSettings.GroupColumns);
            this.listBoxGroupSelectable.Items.Clear();
            this.treemapControl1.Visible = false;
            this.panel1.Visible = true;
            _isInit = false;
        }

        List<PivotDataSource> DataSource { get; set; } = [];

        async void Form1_Shown(object sender, EventArgs e)
        {
            try
            {
                DataSource = !_appSettings.IsAutoLoad || !File.Exists(_appSettings.AutoLoadFilePath)
                        ? [] : await LoadJsonDataAsync(_appSettings.AutoLoadFilePath);

                SetDataSource(DataSource);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void SetDataSource(List<PivotDataSource> dataSources)
        {
            var showTreemap = dataSources.Count > 0;
            this.treemapControl1.Visible = showTreemap;
            this.panel1.Visible = !showTreemap;
            _driver.Invalidate(dataSources);
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
                    DisplayMaxDepth = (int)numericDispDepth.Value,
                    RootNodeTitle = this.textBoxRootName.Text,
                    WeightColumn = _defaultWeightColumn,
                    GroupColumns = GetSelectedColumnNames(),
                    ExplodeGaps = [(int)this.numericExplode.Value],
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

                _driver.Invalidate(DataSource);

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
            var json = JsonSerializer.Serialize(
                new { treemapSettings, treemapLayoutSettings, legendSettings }, _options);

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
#if DEBUG
            if (node != null)
            {
                var sources = _driver.GetSources(cp);
                var sb = new StringBuilder();
                foreach (var s in sources)
                {
                    sb.AppendLine(s.ToString());
                }
                this.toolStripStatusLabel1.Text += Environment.NewLine + sb.ToString();
            }
#endif
        }

        void DrawLeaxfNode(IEnumerable<PivotDataSource> sources, Rectangle? bounds)
        {

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

        async Task<List<PivotDataSource>> LoadJsonDataAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found.", filePath);
            }
            using FileStream fs = File.OpenRead(filePath);
            return await JsonSerializer.DeserializeAsync<List<PivotDataSource>>(fs, _options) ?? [];
        }

        async void toolStripLoad_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
                openFileDialog1.DefaultExt = "json";
                openFileDialog1.FileName = "data.json";
                openFileDialog1.AddExtension = true;

                if (openFileDialog1.ShowDialog() != DialogResult.OK) { return; }

                DataSource = await LoadJsonDataAsync(openFileDialog1.FileName);
                SetDataSource(DataSource);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading JSON data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Change the method signature to match the delegate
        void DrawLeafNode(Graphics g, TreemapNode node, IEnumerable<PivotDataSource> sources)
        {
            var bounds = node.Bounds;
            if (bounds.Width <= 0 || bounds.Height <= 0) return;
            using var backBrush = new SolidBrush(node.Format.BackColor);
            g.FillRectangle(backBrush, bounds);
            using var borderPen = new Pen(Color.Black, 1);
            var text = sources.FirstOrDefault()?.Group1 ?? "No Data";
            using var font = new Font("Arial", 10);
            using var textBrush = new SolidBrush(Color.White);
            using var drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;

            var total = (double)sources.Sum(d => d.Weight);
            var purchaseTotal = (double)sources.Sum(d => d.RelativeWeight);
            var per = purchaseTotal / total;
            var s = $"{text}({sources.Sum(d => d.Weight) / 1000:#,##0} {per:+0.0%;-0.0%}) "; 
            
            g.DrawString(s, font, textBrush, bounds, drawFormat);
        }
        #endregion
    }
}