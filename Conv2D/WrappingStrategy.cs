namespace Conv2D
{
    using System;

    public class WrappingStrategy<T> : AccumulationStrategy<T>
        where T: struct, IEquatable<T>, IFormattable
    {
        public WrappingStrategy(Accumulator<T> accumulate) : base(accumulate)
        {
        }

        public override T Accumulate(AccumulationState<T> state)
        {
            throw new NotImplementedException();
        }
    }
}