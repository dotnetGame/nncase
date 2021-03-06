﻿using System;
using System.Collections.Generic;
using System.Numerics.Tensors;
using System.Text;

namespace NnCase.IR.Operators
{
    public class ReduceWindow2D : Node
    {
        public InputConnector Input { get; }

        public OutputConnector Output { get; }

        public ReduceOperator ReduceOperator { get; }

        public float InitialValue { get; }

        public int FilterH { get; }

        public int FilterW { get; }

        public Padding PaddingH { get; }

        public Padding PaddingW { get; }

        public int StrideH { get; }

        public int StrideW { get; }

        public int DilationH { get; }

        public int DilationW { get; }

        public ValueRange<float> FusedActivation { get; }

        public ReduceWindow2D(ReduceOperator reduceOperator, float initialValue, Shape inputShape, int filterH, int filterW, Padding paddingH, Padding paddingW, int strideH, int strideW, int dilationH, int dilationW, ValueRange<float> fusedActivation)
        {
            ReduceOperator = reduceOperator;
            InitialValue = initialValue;
            FilterH = filterH;
            FilterW = filterW;
            PaddingH = paddingH;
            PaddingW = paddingW;
            StrideH = strideH;
            StrideW = strideW;
            DilationH = dilationH;
            DilationW = dilationW;
            FusedActivation = fusedActivation;

            var outputShape = new Shape(
                inputShape[0],
                inputShape[1],
                ShapeUtility.GetWindowedOutputSize(inputShape[2] + paddingH.Sum, filterH, strideH, dilationH, false),
                ShapeUtility.GetWindowedOutputSize(inputShape[3] + paddingW.Sum, filterW, strideW, dilationW, false));

            Input = AddInput("input", DataType.Float32, inputShape);
            Output = AddOutput("output", DataType.Float32, outputShape);
        }
    }
}
