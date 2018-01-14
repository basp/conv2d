namespace Conv2D
{
    using System;
    using MathNet.Numerics.LinearAlgebra;

    public interface IAccumulationStrategy<T>
        where T: struct, IEquatable<T>, IFormattable
    {
        T Accumulate(AccumulationState<T> state);
    }
}