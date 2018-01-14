namespace Conv2D
{
    using System;
    using System.Linq;

    public class Matrix
    {
        public static Matrix<T> Create<T>(
            int rows,
            int cols,
            Func<int, int, T> factory)
        {
            throw new NotImplementedException();
        }

        public static Matrix<T> FromMatrix<T>(Matrix<T> image)
        {
            throw new NotImplementedException();
        }
    }

    public class Matrix<T>
    {
        private Matrix(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
        }

        private readonly T[] storage;
        private readonly int rows;
        private readonly int cols;

        private int GetIndex(int row, int col) => row * this.cols + col;

        public T this[int row, int col]
        {
            get
            {
                var i = this.GetIndex(row, cols);
                return this.storage[i];
            }
            set
            {
                var i = this.GetIndex(row, col);
                this.storage[i] = value;
            }
        }

        public int RowCount => this.rows;

        public int ColumnCount => this.cols;

        public T Sum(Func<T, T, T> f)
        {
            var acc = default(T);
            Array.ForEach(this.storage, x => acc = f(acc, x));
            return acc;
        }
    }
}
