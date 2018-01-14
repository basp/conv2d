namespace Conv2D
{
    using System;
    using MathNet.Numerics.LinearAlgebra;

    public delegate T Accumulator<T>(T a, T b, double w);

    public abstract class Convolution<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        protected readonly Tuple<int, int> kcp;
        protected readonly double ksum;
        protected readonly Matrix<double> kernel;
        protected readonly Accumulator<T> accumulate;

        protected Convolution(
            Matrix<double> kernel,
            Accumulator<T> accumulate)
        {
            this.kernel = kernel;
            this.ksum = kernel.ColumnSums().Sum();
            this.accumulate = accumulate;
            this.kcp = Tuple.Create(
                (kernel.RowCount - 1) / 2,
                (kernel.ColumnCount - 1) / 2);
        }

        public abstract Matrix<T> Apply(Matrix<T> image);

        public static Convolution<T> Create(
            Matrix<double> kernel,
            Accumulator<T> accumulate,
            EdgeHandling edgeHandling)
        {
            if (kernel.RowCount % 2 == 0)
            {
                throw new ArgumentException();
            }

            if (kernel.ColumnCount % 2 == 0)
            {
                throw new ArgumentException();
            }

            switch (edgeHandling)
            {
                case EdgeHandling.Crop:
                    return new CroppingConvolution<T>(kernel, accumulate);
                case EdgeHandling.Extend:
                    return new ExtendingConvolution<T>(kernel, accumulate);
                case EdgeHandling.Mirror:
                    return new MirroringConvolution<T>(kernel, accumulate);
                case EdgeHandling.Wrap:
                    return new WrappingConvolution<T>(kernel, accumulate);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}