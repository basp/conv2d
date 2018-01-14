namespace Conv2D
{
    using System;
 
    internal class AccumulationStateBuilder<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        private readonly Matrix<double> kernel;
        private Matrix<T> image;
        private int row;
        private int col;

        public AccumulationStateBuilder(Matrix<double> kernel)
        {
            this.kernel = kernel;
        }

        public AccumulationStateBuilder<T> WithImage(Matrix<T> image)
        {
            this.image = image;
            return this;
        }

        public AccumulationStateBuilder<T> WithRow(int row)
        {
            this.row = row;
            return this;
        }

        public AccumulationStateBuilder<T> WithColumn(int col)
        {
            this.col = col;
            return this;
        }

        public AccumulationStateBuilder<T> WithRowAndColumn(int row, int col)
        {
            this.row = row;
            this.col = col;
            return this;
        }

        public AccumulationState<T> Build() => new AccumulationState<T>
        {
            Kernel = this.kernel,
            Image = this.image,
            ImageRow = this.row,
            ImageColumn = this.col,
        };
    }
}