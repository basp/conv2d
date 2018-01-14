# convo2d
Two dimensional discrete convolutions for .NET.

### quick start
First we need an `image`:
```
var image = Matrix<double>.Build.Dense(5, 5, new double[] {
    1.1, 1.2, 1.3, 1.4, 1.5,
    2.1. 2.2, 2.3, 2.4, 2.5,
    3.1, 3.2, 3.3, 3.4, 3.5,
    4.1, 4.2, 4.3, 4.4, 4.5,
    5.1, 5.2, 5.3, 5.4, 5.5,
});
```

Next we need a `kernel`:
```
var kernel = Matrix<double>.Build.Dense(3, 3, new double[] {
    0, 0, 0,
    0, 1, 0,
    0, 0, 0,
});
```

This is the **identity** kernel. It will return the same result as the original `image`.

We also need an `Accumulator<T>` instance:
```
Accumulator<double> acc = (a, b, w) => a + (w * b);
```

The **accumulator** has three arguments: 
1. the accumulated value of type `T` (`a`)
2. the value to be accumulated of type `T` (`b`) 
3. the weight as found in the kernel of type `double` (`w`)

The job of the accumulator function is to combine those three values into a single new value of type `T` to be used in the result.

Finally we have to decide on an edge handling strategy. In this case we'll use `EdgeHandling.Crop` and put it all together:

```
var conv = Convolution.Create<double>(kernel, acc, EdgeHandling.Crop);
```

We cun run the **convolution** by invoking the `Apply` method and passing in our `image` data:
```
conv.Apply(image).Dump();
```