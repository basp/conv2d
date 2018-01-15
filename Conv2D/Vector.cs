namespace Conv2D
{
    using System;

    public class Vector
    {
        public static Vector<T> Create<T>(T[] storage) 
            where T: struct, IEquatable<T>, IFormattable  => new Vector<T>(storage);
    }

    public class Vector<T>
        where T: struct, IEquatable<T>, IFormattable
    {
        private readonly T[] storage;

        public Vector(T[] storage)
        {
            this.storage = storage;
        }
    }
}