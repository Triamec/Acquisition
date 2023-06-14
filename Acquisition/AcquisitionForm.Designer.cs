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
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this._chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this._trackBarTriggerLevel = new System.Windows.Forms.TrackBar();
            this._trackBarDuration = new System.Windows.Forms.TrackBar();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._trackBarTriggerLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._trackBarDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(9, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(40, 13);
            label1.TabIndex = 3;
            label1.Text = "Trigger";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(60, 9);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(78, 13);
            label2.TabIndex = 4;
            label2.Text = "Recording time";
            // 
            // _chart
            // 
            this._chart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._chart.AntiAliasing = System.Windows.Forms.DataVisualization.Charting.AntiAliasingStyles.Text;
            chartArea1.AxisX.IsMarginVisible = false;
            chartArea1.AxisX.Title = "t [ms]";
            chartArea1.AxisY.Title = "Position";
            chartArea1.AxisY2.MajorGrid.Enabled = false;
            chartArea1.AxisY2.Title = "Position error × 1000";
            chartArea1.Name = "chartArea";
            this._chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            legend1.Title = "Signals";
            legend1.TitleAlignment = System.Drawing.StringAlignment.Near;
            this._chart.Legends.Add(legend1);
            this._chart.Location = new System.Drawing.Point(114, 0);
            this._chart.Name = "_chart";
            series1.BorderWidth = 3;
            series1.ChartArea = "chartArea";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Legend = "Legend1";
            series1.Name = "Position";
            series2.BorderColor = System.Drawing.Color.Blue;
            series2.ChartArea = "chartArea";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series2.Legend = "Legend1";
            series2.Name = "Position Error";
            series2.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            this._chart.Series.Add(series1);
            this._chart.Series.Add(series2);
            this._chart.Size = new System.Drawing.Size(804, 269);
            this._chart.TabIndex = 0;
            this._chart.TextAntiAliasingQuality = System.Windows.Forms.DataVisualization.Charting.TextAntiAliasingQuality.SystemDefault;
            // 
            // _trackBarTriggerLevel
            // 
            this._trackBarTriggerLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._trackBarTriggerLevel.Location = new System.Drawing.Point(12, 25);
            this._trackBarTriggerLevel.Maximum = 100;
            this._trackBarTriggerLevel.Name = "_trackBarTriggerLevel";
            this._trackBarTriggerLevel.Orientation = System.Windows.Forms.Orientation.Vertical;
            this._trackBarTriggerLevel.Size = new System.Drawing.Size(45, 231);
            this._trackBarTriggerLevel.TabIndex = 1;
            this._trackBarTriggerLevel.TickStyle = System.Windows.Forms.TickStyle.None;
            this._trackBarTriggerLevel.Value = 1;
            this._trackBarTriggerLevel.Scroll += new System.EventHandler(this.OnTrackBarTriggerLevelScroll);
            // 
            // _trackBarDuration
            // 
            this._trackBarDuration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._trackBarDuration.Location = new System.Drawing.Point(63, 25);
            this._trackBarDuration.Maximum = 100;
            this._trackBarDuration.Minimum = 1;
            this._trackBarDuration.Name = "_trackBarDuration";
            this._trackBarDuration.Orientation = System.Windows.Forms.Orientation.Vertical;
            this._trackBarDuration.Size = new System.Drawing.Size(45, 231);
            this._trackBarDuration.TabIndex = 2;
            this._trackBarDuration.TickStyle = System.Windows.Forms.TickStyle.None;
            this._trackBarDuration.Value = 10;
            // 
            // AcquisitionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(918, 268);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this._trackBarDuration);
            this.Controls.Add(this._trackBarTriggerLevel);
            this.Controls.Add(this._chart);
            this.Name = "AcquisitionForm";
            this.Text = "Form";
            ((System.ComponentModel.ISupportInitialize)(this._chart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._trackBarTriggerLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._trackBarDuration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TrackBar _trackBarTriggerLevel;
		private System.Windows.Forms.TrackBar _trackBarDuration;
		private System.Windows.Forms.DataVisualization.Charting.Chart _chart;
	}
}
