namespace Conv2D
{
    using System;

    public abstract class AccumulationStrategy<T> : IAccumulationStrategy<T>
        where T: struct, IEquatable<T>, IFormattable
    {
        protected readonly Accumulator<T> accumulate;

        protected AccumulationStrategy(Accumulator<T> accumulate)
        {
            this.accumulate = accumulate;
        }

        public abstract T Accumulate(AccumulationState<T> state);
    }
}