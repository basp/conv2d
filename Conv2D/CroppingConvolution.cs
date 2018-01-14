namespace Conv2D
{
    using System;
    using MathNet.Numerics.LinearAlgebra;    

    internal class CroppingConvolution<T> : Convolution<T>
        where T: struct, IEquatable<T>, IFormattable
    {
        public CroppingConvolution(
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
                    if (rimg < this.kcp.Item1 || rimg >= (image.RowCount - this.kcp.Item1))
                    {
                        result[rimg, cimg] = default(T);
                        continue;
                    }

                    if (cimg < this.kcp.Item2 || (cimg >= image.ColumnCount - this.kcp.Item2))
                    {
                        result[rimg, cimg] = default(T);
                        continue;
                    }

                    var acc = default(T);
                    for (var rk = -this.kcp.Item1; rk <= +this.kcp.Item1; rk++)
                    {
                        for (var ck = -this.kcp.Item2; ck <= +this.kcp.Item2; ck++)
                        {

                            var w = this.kernel[this.kcp.Item1 + rk, this.kcp.Item2 + ck] / this.ksum;
                            acc = this.accumulate(acc, image[rimg + rk, cimg + ck], w);
                        }
                    }

                    result[rimg, cimg] = acc;
                }
            }

            return result;
        }
    }
}