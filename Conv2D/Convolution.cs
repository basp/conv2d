namespace Conv2D
{
    using System;
    using MathNet.Numerics.LinearAlgebra;

    public delegate T Accumulator<T>(T a, T b, double w);

    public class Convolution
    {
        protected Convolution()
        {
        }

        public static Tuple<int, int> GetKernelCenterPoint(Matrix<double> kernel) =>
            Tuple.Create((kernel.RowCount - 1) / 2, (kernel.ColumnCount - 1) / 2);

        public static double GetKernelSum(Matrix<double> kernel) =>
            kernel.ColumnSums().Sum();

        public static Convolution<T> Create<T>(
            Matrix<double> kernel,
            Accumulator<T> accumulate,
            EdgeHandling edgeHandling) where T : struct, IEquatable<T>, IFormattable
        {
            if (kernel.RowCount % 2 == 0)
            {
                const string err = "Kernel must have an odd number of rows.";
                throw new ArgumentException(err, nameof(kernel));
            }

            if (kernel.ColumnCount % 2 == 0)
            {
                const string err = "Kernel must have an odd number of columns.";
                throw new ArgumentException(err, nameof(kernel));
            }

            IAccumulationStrategy<T> strategy;
            switch (edgeHandling)
            {
                case EdgeHandling.Crop:
                    strategy = new CroppingStrategy<T>(accumulate);
                    break;
                case EdgeHandling.Extend:
                    strategy = new ExtendingStrategy<T>(accumulate);
                    break;
                case EdgeHandling.Mirror:
                    strategy = new MirroringStrategy<T>(accumulate);
                    break;
                case EdgeHandling.Wrap:
                    strategy = new WrappingStrategy<T>(accumulate);
                    break;
                default:
                    const string err = "Please specify an edge handling strategy.";
                    throw new ArgumentException(err, nameof(edgeHandling));
            }

            return new Convolution<T>(kernel, strategy);
        }
    }

    public class Convolution<T> : Convolution
        where T : struct, IEquatable<T>, IFormattable
    {
        protected readonly Matrix<double> kernel;
        protected readonly IAccumulationStrategy<T> strategy;

        public Convolution(
            Matrix<double> kernel,
            IAccumulationStrategy<T> strategy)
        {
            this.kernel = kernel;
            this.strategy = strategy;
        }

        public virtual Matrix<T> Apply(Matrix<T> image)
        {
            var result = Matrix<T>.Build.DenseOfMatrix(image);
            for (var row = 0; row < image.RowCount; row++)
            {
                for (var col = 0; col < image.ColumnCount; col++)
                {
                    var state = new AccumulationState<T>
                    {
                        Kernel = this.kernel,
                        Image = image,
                        ImageRow = row,
                        ImageColumn = col,
                    };

                    result[row, col] = this.strategy.Accumulate(state);
                }
            }

            return result;
        }
    }
}