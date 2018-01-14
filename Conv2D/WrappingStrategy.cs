namespace Conv2D
{
    using System;

    public class WrappingStrategy<T> : AccumulationStrategy<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        public WrappingStrategy(Accumulator<T> accumulate) : base(accumulate)
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
                    var rt = state.ImageRow + rk;
                    var ct = state.ImageColumn + ck;

                    if (rt < 0)
                    {
                        rt = state.Image.RowCount + rk;
                    }

                    if (rt >= state.Image.RowCount)
                    {
                        rt = rk;
                    }

                    if (ct < 0)
                    {
                        ct = state.Image.ColumnCount + ck;
                    }

                    if (ct >= state.Image.ColumnCount)
                    {
                        ct = ck;
                    }

                    var w = state.Kernel[kcp.Item1 + rk, kcp.Item2 + ck] / ksum;
                    acc = this.accumulate(acc, state.Image[rt, ct], w);
                }
            }

            return acc;
        }
    }
}