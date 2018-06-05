using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows.Shapes;

namespace RoundProgressBar
{
    /// <summary>
    /// A circular type progress bar, that is simliar to popular web based
    /// progress bars
    /// </summary>
    public partial class CircularProgressBar
    {
        public static readonly DependencyProperty CircleSizeProperty = DependencyProperty.Register(
            "CircleSize", typeof(double), typeof(CircularProgressBar), new PropertyMetadata(7.0));

        public double CircleSize
        {
            get { return (double)GetValue(CircleSizeProperty); }
            set
            {
                SetValue(CircleSizeProperty, value);
                HandleLoaded(this, new RoutedEventArgs());
            }
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Brush), typeof(CircularProgressBar), new PropertyMetadata(default(Brush)));

        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty CircleDiameterProperty = DependencyProperty.Register(
            "CircleDiameter", typeof(double), typeof(CircularProgressBar), new PropertyMetadata(20.0));

        public double CircleDiameter
        {
            get { return (double)GetValue(CircleDiameterProperty); }
            set
            {
                SetValue(CircleDiameterProperty, value);
                HandleLoaded(this, new RoutedEventArgs());
            }
        }

        #region Data
        private readonly DispatcherTimer animationTimer;
        #endregion

        #region Constructor
        public CircularProgressBar()
        {
            InitializeComponent();

            animationTimer = new DispatcherTimer(
                DispatcherPriority.ContextIdle, Dispatcher);
            animationTimer.Interval = new TimeSpan(0, 0, 0, 0, 75);
        }
        #endregion

        #region Private Methods
        private void Start()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            animationTimer.Tick += HandleAnimationTick;
            animationTimer.Start();
        }

        private void Stop()
        {
            animationTimer.Stop();
            Mouse.OverrideCursor = Cursors.Arrow;
            animationTimer.Tick -= HandleAnimationTick;
        }

        private void HandleAnimationTick(object sender, EventArgs e)
        {
            SpinnerRotate.Angle = (SpinnerRotate.Angle + 36) % 360;
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            const double offset = Math.PI;
            const double step = Math.PI * 2 / 10.0;

            SetPosition(C0, offset, 0.0, step);
            SetPosition(C1, offset, 1.0, step);
            SetPosition(C2, offset, 2.0, step);
            SetPosition(C3, offset, 3.0, step);
            SetPosition(C4, offset, 4.0, step);
            SetPosition(C5, offset, 5.0, step);
            SetPosition(C6, offset, 6.0, step);
            SetPosition(C7, offset, 7.0, step);
            SetPosition(C8, offset, 8.0, step);
            if (CircleDiameter == 20.0)
                CircleDiameter = Width / 3;
        }

        private void SetPosition(Ellipse ellipse, double offset,
            double posOffSet, double step)
        {
            double left = ((Width - CircleSize) / 2) + (Math.Sin(offset + (posOffSet * step)) * CircleDiameter);
            double top = ((Width - CircleSize) / 2) + (Math.Cos(offset + (posOffSet * step)) * CircleDiameter);

            ellipse.SetValue(Canvas.LeftProperty, left);
            ellipse.SetValue(Canvas.TopProperty, top);
        }

        private void HandleUnloaded(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void HandleVisibleChanged(object sender,
            DependencyPropertyChangedEventArgs e)
        {
            bool isVisible = (bool)e.NewValue;

            if (isVisible)
                Start();
            else
                Stop();
        }
        #endregion
    }
}
