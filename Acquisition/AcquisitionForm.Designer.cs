// Copyright © 2012 Triamec Motion AG

namespace Triamec.Tam.Samples {
    partial class AcquisitionForm {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this._chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this._trackBarTriggerLevel = new System.Windows.Forms.TrackBar();
            this._trackBarDuration = new System.Windows.Forms.TrackBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._trackBarTriggerLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._trackBarDuration)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = System.Windows.Forms.DockStyle.Left;
            label1.Location = new System.Drawing.Point(0, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(40, 13);
            label1.TabIndex = 3;
            label1.Text = "Trigger";
            // 
            // label2
            // 
            label2.Dock = System.Windows.Forms.DockStyle.Fill;
            label2.Location = new System.Drawing.Point(40, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(60, 33);
            label2.TabIndex = 4;
            label2.Text = "Recording time";
            // 
            // _chart
            // 
            this._chart.AntiAliasing = System.Windows.Forms.DataVisualization.Charting.AntiAliasingStyles.Text;
            chartArea1.AxisX.IsMarginVisible = false;
            chartArea1.AxisX.Title = "t [ms]";
            chartArea1.AxisY.Title = "Position";
            chartArea1.AxisY2.MajorGrid.Enabled = false;
            chartArea1.AxisY2.Title = "Position error × 1000";
            chartArea1.Name = "chartAreaTop";
            chartArea2.AxisX.LabelStyle.Enabled = false;
            chartArea2.AxisX.MajorGrid.Enabled = false;
            chartArea2.AxisX.MajorTickMark.Enabled = false;
            chartArea2.AxisY.LabelStyle.Enabled = false;
            chartArea2.AxisY.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            chartArea2.AxisY.MajorGrid.Enabled = false;
            chartArea2.AxisY.MajorTickMark.Enabled = false;
            chartArea2.Name = "chartAreaBottom";
            this._chart.ChartAreas.Add(chartArea1);
            this._chart.ChartAreas.Add(chartArea2);
            this._chart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            legend1.Title = "Signals";
            legend1.TitleAlignment = System.Drawing.StringAlignment.Near;
            this._chart.Legends.Add(legend1);
            this._chart.Location = new System.Drawing.Point(100, 0);
            this._chart.Name = "_chart";
            series1.BorderWidth = 3;
            series1.ChartArea = "chartAreaTop";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Legend = "Legend1";
            series1.Name = "Position";
            series2.BorderColor = System.Drawing.Color.Blue;
            series2.ChartArea = "chartAreaTop";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series2.Legend = "Legend1";
            series2.Name = "Position Error";
            series2.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series3.ChartArea = "chartAreaBottom";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series3.Legend = "Legend1";
            series3.MarkerSize = 3;
            series3.Name = "Phase";
            this._chart.Series.Add(series1);
            this._chart.Series.Add(series2);
            this._chart.Series.Add(series3);
            this._chart.Size = new System.Drawing.Size(818, 575);
            this._chart.TabIndex = 0;
            this._chart.TextAntiAliasingQuality = System.Windows.Forms.DataVisualization.Charting.TextAntiAliasingQuality.SystemDefault;
            // 
            // _trackBarTriggerLevel
            // 
            this._trackBarTriggerLevel.Dock = System.Windows.Forms.DockStyle.Left;
            this._trackBarTriggerLevel.Location = new System.Drawing.Point(0, 33);
            this._trackBarTriggerLevel.Maximum = 100;
            this._trackBarTriggerLevel.Name = "_trackBarTriggerLevel";
            this._trackBarTriggerLevel.Orientation = System.Windows.Forms.Orientation.Vertical;
            this._trackBarTriggerLevel.Size = new System.Drawing.Size(45, 542);
            this._trackBarTriggerLevel.TabIndex = 1;
            this._trackBarTriggerLevel.TickStyle = System.Windows.Forms.TickStyle.None;
            this._trackBarTriggerLevel.Value = 1;
            this._trackBarTriggerLevel.Scroll += new System.EventHandler(this.OnTrackBarTriggerLevelScroll);
            // 
            // _trackBarDuration
            // 
            this._trackBarDuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this._trackBarDuration.Location = new System.Drawing.Point(45, 33);
            this._trackBarDuration.Maximum = 100;
            this._trackBarDuration.Minimum = 1;
            this._trackBarDuration.Name = "_trackBarDuration";
            this._trackBarDuration.Orientation = System.Windows.Forms.Orientation.Vertical;
            this._trackBarDuration.Size = new System.Drawing.Size(55, 542);
            this._trackBarDuration.TabIndex = 2;
            this._trackBarDuration.TickStyle = System.Windows.Forms.TickStyle.None;
            this._trackBarDuration.Value = 10;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._trackBarDuration);
            this.panel1.Controls.Add(this._trackBarTriggerLevel);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(100, 575);
            this.panel1.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(label2);
            this.panel2.Controls.Add(label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(100, 33);
            this.panel2.TabIndex = 3;
            // 
            // AcquisitionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(918, 575);
            this.Controls.Add(this._chart);
            this.Controls.Add(this.panel1);
            this.Name = "AcquisitionForm";
            this.Text = "Form";
            ((System.ComponentModel.ISupportInitialize)(this._chart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._trackBarTriggerLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._trackBarDuration)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TrackBar _trackBarTriggerLevel;
        private System.Windows.Forms.TrackBar _trackBarDuration;
        private System.Windows.Forms.DataVisualization.Charting.Chart _chart;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}
