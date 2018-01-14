namespace Conv2D
{
    using System;
    using MathNet.Numerics.LinearAlgebra;	

	internal class ExtendingConvolution<T> : Convolution<T>
        where T: struct, IEquatable<T>, IFormattable
	{
		public ExtendingConvolution(
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
							T b;

							if (rimg + rk < 0 || rimg + rk >= image.RowCount)
							{
								b = image[rimg, cimg];
							}
							else if (cimg + ck < 0 || cimg + ck >= image.ColumnCount)
							{
								b = image[rimg, cimg];
							}
							else
							{
								b = image[rimg + rk, cimg + ck];
							}

							var w = this.kernel[this.kcp.Item1 + rk, this.kcp.Item2 + ck] / this.ksum;
							acc = this.accumulate(acc, b, w);						}
					}

					result[rimg, cimg] = acc;
				}
			}

			return result;
		}
	}
}