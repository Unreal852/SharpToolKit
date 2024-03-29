﻿using SharpToolKit.Extensions;

namespace SharpToolKit.Tests;

public class RangeTest
{
    [Theory(DisplayName = "Loop over range"), InlineData(0, 10), InlineData(15, 100)]
    public void Theory_LoopOverRange(int min, int max)
    {
        // Act
        var index = 0;
        foreach (var i in min..max)
        {
            index = i;
        }

        // Assert
        Assert.Equal(max, index);
    }
}