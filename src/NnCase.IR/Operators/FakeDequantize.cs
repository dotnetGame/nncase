﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NnCase.IR.Operators
{
    public class FakeDequantize : Node
    {
        public InputConnector Input { get; }

        public OutputConnector Output { get; }

        public FakeDequantize(Shape inputShape)
        {
            Input = AddInput("input", DataType.Float32, inputShape);
            Output = AddOutput("output", DataType.Float32, inputShape);
        }
    }
}
