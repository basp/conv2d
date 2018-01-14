namespace Conv2D
{
    using System;
    using MathNet.Numerics.LinearAlgebra;

    public class AccumulationState<T>
        where T: struct, IEquatable<T>, IFormattable
    {
        public Matrix<T> Image { get; internal set; }

        public Matrix<double> Kernel { get; internal set; }

        public int ImageRow { get; internal set; }

        public int ImageColumn { get; internal set; }
    }
}