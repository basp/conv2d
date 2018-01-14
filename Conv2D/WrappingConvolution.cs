namespace Conv2D
{
    using System;
    using MathNet.Numerics.LinearAlgebra;    

    internal class WrappingConvolution<T> : Convolution<T>
        where T: struct, IEquatable<T>, IFormattable
    {
        public WrappingConvolution(
            Matrix<double> kernel,
            Accumulator<T> accumulate)
            : base(kernel, accumulate)
        {
        }

        public override Matrix<T> Apply(Matrix<T> image)
        {
            var result = Matrix<T>.Build.DenseOfMatrix(image);
            for (var rimg = 0; rimg < image.RowCount; rimg++)
            {
                for (var cimg = 0; cimg < image.ColumnCount; cimg++)
                {
                    var acc = default(T);
                    for (var rk = -this.kcp.Item1; rk <= +this.kcp.Item1; rk++)
                    {
                        for (var ck = -this.kcp.Item2; ck <= +this.kcp.Item2; ck++)
                        {
                            var rt = rimg + rk;
                            var ct = cimg + ck;

                            if (rt < 0)
                            {
                                rt = image.RowCount + rk;
                            }

                            if (rt >= image.RowCount)
                            {
                                rt = rk;
                            }

                            if (ct < 0)
                            {
                                ct = image.ColumnCount + ck;
                            }

                            if (ct >= image.ColumnCount)
                            {
                                ct = ck;
                            }

                            var w = this.kernel[this.kcp.Item1 + rk, this.kcp.Item2 + ck] / this.ksum;
                            acc = this.accumulate(acc, image[rt, ct], w);
                        }
                    }

                    result[rimg, cimg] = acc;
                }
            }

            return result;
        }
    }
}