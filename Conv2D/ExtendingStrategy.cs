namespace Conv2D
{
    using System;

    public class ExtendingStrategy<T> : AccumulationStrategy<T>
        where T: struct, IEquatable<T>, IFormattable
    {
        public ExtendingStrategy(Accumulator<T> accumulate) : base(accumulate)
        {
        }

        public override T Accumulate(AccumulationState<T> state)
        {
            var kcp = Convolution.GetKernelCenterPoint(state.Kernel);
            var ksum = Convolution.GetKernelSum(state.Kernel);
            var acc = default(T);

            for (var rk = -kcp.Item1; rk <= +kcp.Item1; rk++)
            {
                for (var ck = -kcp.Item2; ck <= +kcp.Item2; ck++)
                {
                    T b;

                    if (state.ImageRow + rk < 0 || state.ImageRow + rk >= state.Image.RowCount)
                    {
                        b = state.Image[state.ImageRow, state.ImageColumn];
                    }
                    else if (state.ImageColumn + ck < 0 || state.ImageColumn + ck >= state.Image.ColumnCount)
                    {
                        b = state.Image[state.ImageRow, state.ImageColumn];
                    }
                    else
                    {
                        b = state.Image[state.ImageRow + rk, state.ImageColumn + ck];
                    }

                    var w = state.Kernel[kcp.Item1 + rk, kcp.Item2 + ck] / ksum;
                    acc = this.accumulate(acc, b, w);						
                }
            }

            return acc;
        }
    }
}