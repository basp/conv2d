namespace Conv2D
{
    using System;

    public class CroppingStrategy<T> : AccumulationStrategy<T>
        where T: struct, IEquatable<T>, IFormattable
    {
        public CroppingStrategy(Accumulator<T> accumulate) : base(accumulate)
        {
        }

        public override T Accumulate(AccumulationState<T> state)
        {
            var kcp = Convolution.GetKernelCenterPoint(state.Kernel);
            var ksum = Convolution.GetKernelSum(state.Kernel);
            var acc = default(T);

            if (state.ImageRow < kcp.Item1 || state.ImageRow >= (state.Image.RowCount - kcp.Item1))
            {
                return acc;
            }

            if (state.ImageColumn < kcp.Item2 || state.ImageColumn >=(state.Image.ColumnCount - kcp.Item2))
            {
                return acc;
            }

            for (var rk = -kcp.Item1; rk <= +kcp.Item1; rk++)
            {
                for (var ck = -kcp.Item2; ck <= +kcp.Item2; ck++)
                {
                    var w = state.Kernel[kcp.Item1 + rk, kcp.Item2 + ck] / ksum;
                    acc = this.accumulate(acc, state.Image[state.ImageRow + rk, state.ImageColumn + ck], w);
                }
            }    

            return acc;
        }
    }
}