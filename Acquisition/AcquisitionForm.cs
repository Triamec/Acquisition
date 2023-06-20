// Copyright © 2012 Triamec Motion AG

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Triamec.Acquisitions;
using Triamec.Tam.Acquisitions;
using Triamec.Tam.Registers;
using Triamec.TriaLink;
using Triamec.TriaLink.Adapter;
using Axis = Triamec.Tam.Rlid19.Axis;

namespace Triamec.Tam.Samples {
    internal sealed partial class AcquisitionForm : Form {
        #region Fields
        TamTopology _topology;
        TamAxis _axis;
        ITamAcquisition _acquisition;
        ITamVariable<double> _positionVariable, _positionErrorVariable;
        ITamTrigger _trigger;
        #endregion Fields

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="AcquisitionForm"/> class.
        /// </summary>
        public AcquisitionForm() {
            InitializeComponent();
            _chart.ChartAreas[0].AxisY.Minimum = PosMin;
            _chart.ChartAreas[0].AxisY.Maximum = PosMax;
        }

        #endregion Constructor

        #region Acquisition demo code
        /// <summary>
        /// The name of the axis this demo works with.
        /// </summary>
        const string AxisName = "X";

        /// <summary>
        /// The name of the network interface card the drive is connected to. Only relevant when the drive is connected
        /// via auxiliary Ethernet.
        /// </summary>
        const string NicName = "Ethernet 2";

        /// <summary>
        /// Whether to trigger the acquisition by letting the axis move.
        /// </summary>
        readonly bool _moveAxis = false;

        const double PosMin = -1;
        const double PosMax = 1;

        /// <summary>
        /// The maximal duration, in milliseconds, for axis activation and single moves.
        /// </summary>
        static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Sets up data acquisition.
        /// </summary>
        /// <exception cref="TamException">Startup failed.</exception>
        Task Startup() {

            // Create the root object representing the topology of the TAM hardware.
            // Note that we must dispose this object at the end in order to clean up resources.
            _topology = new TamTopology();

            // Access the drive via Auxiliary Ethernet. Consult application note AN123 for correct setup. In particular,
            // make sure to take into account the firewall. If you can connect to the drive but not acquire data, this
            // is likely due to the firewall.
            var access = DataLinkLayers.Network;

            // Access the drive via PCI card (not recommended when TwinCAT runs on the same system)
            //var access = DataLinkLayers.TriaLink;

            // Connect the drive with a USB cable to the PC (not recommended with harsh electromagnetic environments)
            // Also works with a Tria-Link PCI adapter connected via USB to the measuring PC.
            //var access = DataLinkLayers.TriaLinkUsb;

            ITamNode root;
            if (access == DataLinkLayers.Network) {
                root = _topology.ScanNetworkInterfaces(NicName)[0];
            } else {
                var system = _topology.AddLocalSystem(access);

                // Scan the Tria-Link in order to learn about connected stations.
                system.Identify();

                root = system;
            }

            // Get the axis with the predefined name
            _axis = root.AsDepthFirstLeaves<TamAxis>().FirstOrDefault(axis => axis.Name == AxisName);
            if (_axis == null) throw new TamException($"Axis {AxisName} not found.");

            // Most drives get integrated into a real time control system. Accessing them via TAM API like we do here is considered
            // a secondary use case. Tell the axis that we're going to take control. Otherwise, the axis might reject our commands.
            // You should not do this, though, when this application is about to access the drive via the PCI interface.
            _axis.ControlSystemTreatment.Override(enabled: false);

            var axisRegister = (Axis)_axis.Register;

            // Create two acquisition variables for position and position error.
            // Specify 0 for the sampling time, which will be rounded to the lowest possible sampling time.
            var samplingTime = TimeSpan.Zero;

            // Often, 10kHz suffices
            // var samplingTime = TimeSpan.FromTicks(TimeSpan.TicksPerMillisecond / 10);

            ITamReadonlyRegister posReg = axisRegister.Signals.PositionController.MasterPosition;
            _positionVariable = posReg.CreateVariable(samplingTime);
            ITamReadonlyRegister errorReg = axisRegister.Signals.PositionController.Controllers[0].PositionError;
            _positionErrorVariable = errorReg.CreateVariable(samplingTime);

            // As soon as multiple variables are to be recorded synchronized, create an acquisition object.
            // Otherwise, you may use the Acquire methods of the variable itself.
            _acquisition = TamAcquisition.Create(_positionVariable, _positionErrorVariable);

            // Prepare for the use of the WaitForSuccess method.
            _axis.Drive.AddStateObserver(this);

            return Task.CompletedTask;
        }

        /// <exception cref="TamException">Enabling failed.</exception>
        async Task EnableDriveAsync() {

            // Set the drive operational, i.e. switch the power section on.
            await _axis.Drive.SwitchOn().WaitForSuccessAsync(Timeout).ConfigureAwait(false);

            // Enable the axis controller.
            await _axis.Control(AxisControlCommands.Enable).WaitForSuccessAsync(Timeout).ConfigureAwait(false);
        }

        /// <exception cref="TamException">Disabling failed.</exception>
        async Task DisableDriveAsync() {
            if (_axis == null) return;

            // Disable the axis controller.
            await _axis.Control(AxisControlCommands.Disable).WaitForSuccessAsync(Timeout).ConfigureAwait(false);

            // Switch the power section off.
            await _axis.Drive.SwitchOff().WaitForSuccessAsync(Timeout).ConfigureAwait(false);
        }

        /// <summary>
        /// Acquires data repeatedly.
        /// </summary>
        async Task AcquireAsync() {

            // don't plot anymore if the form is already closed
            while (Visible) {
                var duration = TimeSpan.FromMilliseconds(_trackBarDuration.Value);
                try {
                    // Many applications can simply call Acquire, and won't use a trigger.
                    // Here, we start asynchronously in order to not block the main thread.
                    // We only pass the trigger if the axis is moving.
                    await _acquisition.AcquireAsync(duration, _moveAxis ? _trigger : null).ConfigureAwait(true);

                } catch (AcquisitionException ex) {
                    MessageBox.Show(ex.Message, "Failure during acquisition", MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);

                    break;
                }

                // use absolute time information if you want
                Console.WriteLine(_positionVariable.StartTime);

                // plot
                Fill(_chart.Series["Position"], _positionVariable, 1);
                Fill(_chart.Series["Position Error"], _positionErrorVariable, 1E3);
            }
        }

        /// <summary>
        /// Plots one data series.
        /// </summary>
        static void Fill(Series series, ITamVariable<double> variable, double scaling) {
            DataPointCollection points = series.Points;
            points.SuspendUpdates();
            points.Clear();
            var xStep = variable.SamplingTime.TotalMilliseconds;
            int index = 0;
            foreach (double value in variable) {
                points.AddXY(xStep * index++, value * scaling);
            }
            points.ResumeUpdates();
        }

        /// <summary>
        /// Recreates the trigger from a new value.
        /// </summary>
        /// <remarks>Must be called on the main thread.</remarks>
        void RefreshTrigger() =>

            // Create a hardware trigger on velocity with raising edge on the level dictated by the trigger level
            // track bar.
            _trigger = new TamTrigger(((Axis)_axis.Register).Signals.PathPlanner.Velocity,
                                      PublicationCommand.RaisingEdge,
                                      (float)_trackBarTriggerLevel.Value);

        /// <summary>
        /// Does some work with a drive.
        /// </summary>
        async void DoWork() {
            try {
                #region Preparation

                // create topology, boot system, find axis
                await Task.Run(Startup).ConfigureAwait(true);

                // Make the axis ready for movement
                if (_moveAxis) {
                    await EnableDriveAsync().ConfigureAwait(true);
                    await _axis.SetPosition(0).WaitForSuccessAsync(Timeout).ConfigureAwait(true);
                }

                RefreshTrigger();

                // Start acquiring in parallel
                var _ = AcquireAsync();

                #endregion Preparation

                // move forth and back
                // stop moving when the form is closed
                await Task.Run(Motion).ConfigureAwait(true);
            } catch (TamException ex) {
                MessageBox.Show(this, ex.FullMessage(), "Failure", 0, MessageBoxIcon.Error);
            } finally {
                #region Tear down
                await DisableDriveAsync().ConfigureAwait(false);

                // counterpart for AddStateObserver
                _axis?.Drive.RemoveStateObserver(this);

                _acquisition?.Dispose();
                _topology.Dispose();
                #endregion Tear down
            }
        }

        /// <summary>
        /// Repeatedly commands motion.
        /// </summary>
        Task Motion() {
            while (Visible) {
                if (_moveAxis) {

                    // command moves and wait until the moves are completed
                    // Note that using WaitForSuccessAsync would incur more context switches. This is not ideal for
                    // short moves.
                    _axis.MoveAbsolute(PosMax).WaitForSuccess(Timeout);
                    _axis.MoveAbsolute(PosMin).WaitForSuccess(Timeout);
                } else {
                    Thread.Sleep(TimeSpan.FromSeconds(0.1));
                }
            }
            return Task.CompletedTask;
        }
        #endregion Acquisition demo code

        #region GUI code
        void OnTrackBarTriggerLevelScroll(object sender, EventArgs e) => RefreshTrigger();

        /// <summary>
        /// Raises the <see cref="Form.Shown"/> event.
        /// </summary>
        protected override void OnShown(EventArgs e) {
            base.OnShown(e);
            DoWork();
        }
        #endregion GUI code
    }
}
