namespace Conv2D
{
    using System;
    using MathNet.Numerics.LinearAlgebra;

    public class AccumulationState<T>
        where T: struct, IEquatable<T>, IFormattable
    {
        public Matrix<T> Image { get; set; }

        public Matrix<double> Kernel { get; set; }

        public int ImageRow { get; set; }

        public int ImageColumn { get; set; }
    }
}