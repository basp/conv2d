namespace Conv2D
{
    using System;

    public interface IAccumulationStrategy<T>
        where T: struct, IEquatable<T>, IFormattable
    {
        T Accumulate(AccumulationState<T> state);
    }
}