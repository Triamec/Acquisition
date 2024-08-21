// Copyright © 2012 Triamec Motion AG

using System;
using System.Collections.Generic;
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
        ITamVariable<double> _positionVariable, _positionErrorVariable, _xVariable, _yVariable;
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
        const string AxisName = "Axis 1";
        const bool UseSpecificAxis = false;

        /// <summary>
        /// The name of the network interface card the drive is connected to. Only relevant when the drive is connected
        /// via auxiliary Ethernet.
        /// </summary>
        const string NicName = "Ethernet 2";
        const bool UseSpecificNic = false;

        /// <summary>
        /// Whether to trigger the acquisition by letting the axis move.
        /// </summary>
        readonly bool _moveAxis = true;

        const double PosMin = 1;
        const double PosMax = 2;

        /// <summary>
        /// The maximal duration for axis activation and single moves.
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

            ITamNode root;
            DataLinkLayers access;
            TamSystem system;
            // TODO: [E4.1] Use specific Nic to find your drive faster
            if (UseSpecificNic) {
                // Access the drive via Auxiliary Ethernet. Consult application note AN123 for correct setup. In particular,
                // make sure to take into account the firewall. If you can connect to the drive but not acquire data, this
                // is likely due to the firewall.
                access = DataLinkLayers.Network;

                // Access the drive via PCI card (not recommended when TwinCAT runs on the same system)
                //var access = DataLinkLayers.TriaLink;

                // Connect the drive with a USB cable to the PC (not recommended with harsh electromagnetic environments)
                // Also works with a Tria-Link PCI adapter connected via USB to the measuring PC.
                //var access = DataLinkLayers.TriaLinkUsb;


                // AddLocalSystem also works perfectly for network access. However, it might perform better when we
                // explicitly specify the network interface card connected to the device.
                if (access == DataLinkLayers.Network) {
                    root = _topology.ScanNetworkInterfaces(NicName)[0];
                } else {
                    system = _topology.AddLocalSystem(access);

                    // Scan the Tria-Link in order to learn about connected stations.
                    system.Identify();

                    root = system;
                }
            } else {
                system = _topology.AddLocalSystem();

                // Scan the Tria-Link in order to learn about connected stations.
                system.Identify();

                root = system;
            }

            // Get the axis with the predefined name
            // TODO: [E5.1] Search specifically for your axis instead of just picking the first one
            if (UseSpecificAxis) {
                _axis = root.AsDepthFirstLeaves<TamAxis>().FirstOrDefault(axis => axis.Name == AxisName);
                if (_axis == null) throw new TamException($"Axis {AxisName} not found.");
            } else {
                _axis = root.AsDepthFirstLeaves<TamAxis>().FirstOrDefault();
            }

            // Most drives get integrated into a real time control system. Accessing them via TAM API like we do here is considered
            // a secondary use case. Tell the axis that we're going to take control. Otherwise, the axis might reject our commands.
            // You should not do this, though, when this application is about to access the drive via the PCI interface.
            _axis.ControlSystemTreatment.Override(enabled: _moveAxis);

            var axisRegister = (Axis)_axis.Register;

            // Create two acquisition variables for position and position error.
            // Specify 0 for the sampling time, which will be rounded to the lowest possible sampling time.
            var samplingTime = TimeSpan.Zero;

            // Often, 10kHz suffices
            samplingTime = TimeSpan.FromTicks(TimeSpan.TicksPerMillisecond / 10);

            ITamReadonlyRegister posReg = axisRegister.Signals.PositionController.MasterPosition;

            // TODO: [E2.1] Whoops! Seems like the below register does not show the correct position error.
            // Correct the Uri in order to find the correct position error register.
            // Hints: Use "/" as separator, not ".". Also, start from the "Axis" and not the most upper "Register" node.
            var errorReg = (ITamReadonlyRegister)axisRegister.FindNode(new Uri(
                "Signals/...?", UriKind.Relative));

            ITamReadonlyRegister xReg = posReg;
            // TODO: [E3.1] Choose two registers to plot one against the other, for example the encoder phases
            ITamReadonlyRegister yReg;

            _positionVariable = posReg.CreateVariable(samplingTime);
            // TODO: [E2.2] CreateVariable from errorReg
            _positionErrorVariable = _positionVariable;
            _xVariable = xReg.CreateVariable(samplingTime);
            // TODO: [E3.2] Create yVariable with yReg
            _yVariable = _xVariable;

            // As soon as multiple variables are to be recorded synchronized, create an acquisition object.
            // Otherwise, you may use the Acquire methods of the variable itself.
            // Note that the function takes an array of variables.
            _acquisition = TamAcquisition.Create(_positionVariable, _positionErrorVariable, _xVariable, _yVariable);

            // Prepare for the use of the WaitForSuccess method.
            _axis.Drive.AddStateObserver(this);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Acquires data repeatedly.
        /// </summary>
        async Task AcquireAndPlotAsync() {

            // don't plot anymore if the form is already closed
            while (!_cts.IsCancellationRequested) {
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
                // TODO: [E2.3] Plot the position error, label it "Position Error" and use a scale of 1E3
                // Fill(_chart ...
                FillPolar(_chart.Series["Phase"], _xVariable, _yVariable);
            }
        }

        /// <exception cref="TamException">Enabling failed.</exception>
        async Task EnableDriveAsync() {

            // Reset any axis error.
            await _axis.Control(AxisControlCommands.ResetError).WaitForSuccessAsync(Timeout).ConfigureAwait(true);

            // Enable the axis controller.
            if (_axis.ReadAxisState() <= AxisState.Disabled) { // Workaround for communication issue
                await _axis.Control(AxisControlCommands.Enable).WaitForSuccessAsync(Timeout).ConfigureAwait(true);
            }
        }

        /// <exception cref="TamException">Disabling failed.</exception>
        Task DisableDriveAsync() =>
            _axis?.Control(AxisControlCommands.Disable).WaitForSuccessAsync(Timeout) ?? Task.CompletedTask;

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
        /// Plots one data series in polar coordinates.
        /// </summary>
        static void FillPolar(Series series, IEnumerable<double> xValues, IEnumerable<double> yValues) {
            DataPointCollection points = series.Points;
            points.SuspendUpdates();
            points.Clear();
            foreach (var (x, y) in xValues.Zip(yValues, (x, y) => (x, y))) {
                points.AddXY(x, y);
            }
            points.ResumeUpdates();
        }

        /// <summary>
        /// Recreates the trigger from a new value.
        /// </summary>
        /// <remarks>Must be called on the main thread.</remarks>
        void RefreshTrigger() {

            // Create a hardware trigger on velocity with raising edge on the level dictated by the trigger level
            // track bar.
            _trigger = new TamTrigger(((Axis)_axis.Register).Signals.PathPlanner.Velocity,
                                      PublicationCommand.RaisingEdge,
                                      (float)_trackBarTriggerLevel.Value);

            // If the acquisition was waiting for a trigger condition, disable it to start a new try.
            _acquisition.DisableAsync(null);
        }

        async Task DoOneTimeSetup() {
            // create topology, boot system, find axis
            // Use Task.Run to offload synchronous task to another thread
            await Task.Run(Startup).ConfigureAwait(true);

            // Make the axis ready for movement
            if (_moveAxis) {
                await EnableDriveAsync().ConfigureAwait(true);
                await _axis.SetPosition(0).WaitForSuccessAsync(Timeout).ConfigureAwait(true);
            }
            RefreshTrigger();
        }

        /// <summary>
        /// Executes the setup as well as the continous tasks.
        /// </summary>
        // What are those async and await keywords?
        // See https://learn.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap
        async Task DoWork() {
            Task loggingTask = null;
            try {
                // Do not start before everything is set up
                await DoOneTimeSetup().ConfigureAwait(true);
                // Start acquiring in parallel
                loggingTask = AcquireAndPlotAsync();
                // Move forth and back
                var motionTask = ContinousMotionAsync();
                // Do not close form before running move is done
                await motionTask.ConfigureAwait(true);

            } catch (TamException ex) {
                MessageBox.Show(this, ex.FullMessage(), "Failure", 0, MessageBoxIcon.Error);
            } finally {
                #region Tear down
                await DisableDriveAsync().ConfigureAwait(false);

                // counterpart for AddStateObserver
                _axis?.Drive.RemoveStateObserver(this);

                _acquisition?.Dispose();
                if (loggingTask != null) {
                    await loggingTask.ConfigureAwait(false);
                }

                _topology.Dispose();
                #endregion Tear down
            }
        }

        /// <summary>
        /// Repeatedly commands motion.
        /// </summary>
        async Task ContinousMotionAsync() {
            while (!_cts.IsCancellationRequested) {
                if (_moveAxis) {

                    // TODO: [E1.1] These synchronous MoveAbsolute commands are blocking our GUI thread!
                    // Let's make them asynchronous so we can do other stuff while waiting.
                    _axis.MoveAbsolute(PosMax).WaitForSuccess(Timeout);

                    if (_cts.IsCancellationRequested) break;

                    _axis.MoveAbsolute(PosMin).WaitForSuccess(Timeout);

                    // TODO: [E1.2] If you made the above moves async, you can remove the pause below.
                    // If the GUI stays responsive, you fixed the bug :)
                    await Task.Delay(TimeSpan.FromSeconds(0.001)).ConfigureAwait(true);



                } else {
                    await Task.Delay(TimeSpan.FromSeconds(0.1)).ConfigureAwait(true);
                }
            }
        }
        #endregion Acquisition demo code

        #region GUI code
        readonly CancellationTokenSource _cts = new CancellationTokenSource();
        Task _workTask;

        void OnTrackBarTriggerLevelScroll(object sender, EventArgs e) => RefreshTrigger();

        /// <summary>
        /// Raises the <see cref="Form.Shown"/> event.
        /// </summary>
        protected override void OnShown(EventArgs e) {
            base.OnShown(e);
            _workTask = DoWork();
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            base.OnFormClosing(e);
            _cts.Cancel();
            while (!_workTask.Wait(TimeSpan.FromMilliseconds(20))) {
                Application.DoEvents();
            }
        }
        #endregion GUI code
    }
}
