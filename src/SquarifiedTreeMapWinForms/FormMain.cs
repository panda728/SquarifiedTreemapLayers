using System.Text;
using SquarifiedTreeMapShared;
using SquarifiedTreeMapForge.WinForms;
using System.Text.Json;

namespace SquarifiedTreeMapWinForms
{
    public partial class FormMain : Form
    {
        readonly Semaphore _semaphore = new(1, 1);

        readonly TreeMapGdiDriver<PivotDataSource> _driver;
        readonly string[] _defaultAggrColumns;
        readonly string _defaultWeightColumn;

        readonly bool _isInit = true;

        public FormMain(TreeMapGdiDriver<PivotDataSource> driver)
        {
            InitializeComponent();

            toolStripStatusLabel1.Text = "";
            radioLT.Checked = true;
            radioD.Checked = true;
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.SplitterDistance = this.splitContainer1.Width;

            _driver = driver;
            _driver.FuncNodeText = GetTitle;
            _driver.FuncNodeColor = GetColor;
            _driver.TreeMapControl = this.treeMapControl1;
            _driver.OnMouseMoveAction += treeMapControl1_MouseMove;
            _driver.OnMouseLeaveAction += treeMapControl1_MouseLeave; ;

            _defaultAggrColumns = _driver.LayoutSettings.AggregateColumns;
            _defaultWeightColumn = _driver.LayoutSettings.WeightColumn;

            this.checkShow.Checked = _driver.LegendSettings.IsShowLegend;
            this.checkShowPlusSign.Checked = _driver.LegendSettings.IsShowPlusSign;
            this.checkLegendOrder.Checked = _driver.LegendSettings.IsOrderAsc;
            this.numericLegendSteps.Value = _driver.LegendSettings.StepCount;
            this.numericLegendFontSize.Value = (decimal)_driver.TreeMapSettings.LegendFontSize;
            this.numericLegendWidth.Value = _driver.LegendSettings.Width;
            this.numericLegendHeight.Value = _driver.LegendSettings.Height;
            this.numericMinPer.Value = (decimal)(_driver.LegendSettings.MinValue * 100);
            this.numericMaxPer.Value = (decimal)(_driver.LegendSettings.MaxValue * 100);
            this.numericMinBri.Value = (decimal)(_driver.LegendSettings.MinBrightness * 100);
            this.numericMaxBri.Value = (decimal)(_driver.LegendSettings.MaxBrightness * 100);
            this.numericSat.Value = (decimal)(_driver.LegendSettings.Saturation * 100);
            this.numericHuePositive.Value = (decimal)_driver.LegendSettings.HuePositive;
            this.numericHueNegative.Value = (decimal)_driver.LegendSettings.HueNegative;

            this.textBoxTitle.Text = _driver.LayoutSettings.TitleText;
            this.textBoxRootName.Text = _driver.LayoutSettings.RootNodeTitle;

            this.listBoxAggrSelected.Items.Clear();
            this.listBoxAggrSelected.Items.AddRange(_driver.LayoutSettings.AggregateColumns);
            this.listBoxAggrSelectable.Items.Clear();
            this.treeMapControl1.Visible = false;
            this.panel1.Visible = true;
            _isInit = false;
        }

        List<PivotDataSource> PivotDataSource { get; set; } = [];

        string GetTitle(string k, IEnumerable<PivotDataSource> d)
        {
            var total = (double)d.Sum(d => d.Weight);
            var purchaseTotal = (double)d.Sum(d => d.RelativeWeight);
            var per = purchaseTotal / total;
            return $"{k}({d.Sum(d => d.Weight) / 1000:#,##0} {per:+0.0%;-0.0%}) ";
        }

        Color GetColor(IEnumerable<PivotDataSource> d)
        {
            var total = (double)d.Sum(d => d.Weight);
            var diff = (double)d.Sum(d => d.RelativeWeight);
            var percentage = Math.Round(diff / total * 1000) / 1000;
            return _driver.GetPercentageColor(percentage);
        }

        void Form1_Shown(object sender, EventArgs e)
        {
            _driver.Invalidate(PivotDataSource);
        }

        async Task RedrawTreemapAsync()
        {
            if (_isInit || _driver == null) return;
            if (_semaphore.WaitOne(0) == false) return;
            try
            {
                _driver.LayoutSettings.TitleText = this.textBoxTitle.Text;

                _driver.LayoutSettings.LayoutAlign = GetLayout();
                _driver.LayoutSettings.IsSourceOrderDec = IsOrderDec();
                _driver.LayoutSettings.MaxDepth = (int)numericDepth.Value;
                _driver.LayoutSettings.RootNodeTitle = this.textBoxRootName.Text;
                _driver.LayoutSettings.WeightColumn = _defaultWeightColumn;

                var cols = GetSelectedColumnNames();
                _driver.LayoutSettings.AggregateColumns =
                    cols.Length > 0 ? cols : _defaultAggrColumns;

                _driver.LegendSettings.MinValue = (double)(this.numericMinPer.Value / 100);
                _driver.LegendSettings.MaxValue = (double)(this.numericMaxPer.Value / 100);
                _driver.LegendSettings.MinBrightness = (double)(this.numericMinBri.Value / 100);
                _driver.LegendSettings.MaxBrightness = (double)(this.numericMaxBri.Value / 100);
                _driver.LegendSettings.Saturation = (double)(this.numericSat.Value / 100);
                _driver.LegendSettings.HuePositive = (int)this.numericHuePositive.Value;
                _driver.LegendSettings.HueNegative = (int)this.numericHueNegative.Value;
                _driver.LegendSettings.IsShowLegend = this.checkShow.Checked;
                _driver.LegendSettings.IsShowPlusSign = this.checkShowPlusSign.Checked;
                _driver.LegendSettings.IsOrderAsc = this.checkLegendOrder.Checked;
                _driver.LegendSettings.StepCount = (int)numericLegendSteps.Value;
                _driver.LegendSettings.Width = (int)numericLegendWidth.Value;
                _driver.LegendSettings.Height = (int)numericLegendHeight.Value;

                _driver.TreeMapSettings.LegendFontSize = (float)numericLegendFontSize.Value;

                _driver.Invalidate(PivotDataSource);

                if (!string.IsNullOrEmpty(_driver.LayoutSettings.TitleText))
                {
                    this.Text = _driver.LayoutSettings.TitleText;
                }


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

        void treeMapControl1_MouseMove(object? sender, MouseEventArgs e)
        {
            var cp = treeMapControl1.PointToClient(Cursor.Position);
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

        void treeMapControl1_MouseLeave(object? sender, EventArgs e)
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
            foreach (object item in listBoxAggrSelected.Items)
            {
                list.Add($"{item}");
            }
            return [.. list];
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

        async void buttonAggrAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var selectables = GetSelectedItems(listBoxAggrSelectable);
                if (selectables.Length == 0) { return; }
                foreach (var s in selectables)
                {
                    if (listBoxAggrSelected.Items.Contains(s) == false)
                    {
                        listBoxAggrSelected.Items.Add(s);
                        listBoxAggrSelectable.Items.Remove(s);
                    }
                }
                await RedrawTreemapAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        async void buttonAggrDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedItems = GetSelectedItems(listBoxAggrSelected);
                if (selectedItems.Length == 0) { return; }
                foreach (var item in selectedItems)
                {
                    listBoxAggrSelected.Items.Remove(item);
                    if (!listBoxAggrSelectable.Items.Contains(item))
                    {
                        listBoxAggrSelectable.Items.Add(item);
                        listBoxAggrSelected.Items.Remove(item);
                    }
                }
                await RedrawTreemapAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        async void buttonAggerUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxAggrSelected.SelectedItem == null || listBoxAggrSelected.SelectedIndex <= 0) return;

                int selectedIndex = listBoxAggrSelected.SelectedIndex;
                var selectedItem = listBoxAggrSelected.SelectedItem;

                listBoxAggrSelected.Items.RemoveAt(selectedIndex);
                listBoxAggrSelected.Items.Insert(selectedIndex - 1, selectedItem);
                listBoxAggrSelected.SelectedIndex = selectedIndex - 1;
                await RedrawTreemapAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        async void buttonAggrDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxAggrSelected.SelectedItem == null || listBoxAggrSelected.SelectedIndex < 0 || listBoxAggrSelected.SelectedIndex >= listBoxAggrSelected.Items.Count - 1) return;

                int selectedIndex = listBoxAggrSelected.SelectedIndex;
                var selectedItem = listBoxAggrSelected.SelectedItem;

                listBoxAggrSelected.Items.RemoveAt(selectedIndex);
                listBoxAggrSelected.Items.Insert(selectedIndex + 1, selectedItem);
                listBoxAggrSelected.SelectedIndex = selectedIndex + 1;
                await RedrawTreemapAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        async Task LoadJsonDataAsync(string filePath)
        {
            try
            {
                using FileStream openStream = File.OpenRead(filePath);
                PivotDataSource = await JsonSerializer.DeserializeAsync<List<PivotDataSource>>(openStream) ?? [];
                await RedrawTreemapAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading JSON data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        async void toolStripLoad_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
            openFileDialog1.DefaultExt = "json";
            openFileDialog1.FileName = "data.json";
            openFileDialog1.AddExtension = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                await LoadJsonDataAsync(openFileDialog1.FileName);
                if(PivotDataSource.Count > 0)
                {
                    this.treeMapControl1.Visible = true;
                    this.panel1.Visible = false;
                }
            }
        }
        #endregion
    }
}