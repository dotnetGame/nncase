﻿using System;
using System.Collections.Generic;
using System.Numerics.Tensors;
using System.Text;
using NnCase.IR;

namespace NnCase.Targets.CPU.IR.Operators
{
    public class CPUDepthwiseConv2D : Node
    {
        public InputConnector Input { get; }

        public OutputConnector Output { get; }

        public DenseTensor<float> Weights { get; }

        public DenseTensor<float> Bias { get; }

        public Padding PaddingH { get; }

        public Padding PaddingW { get; }

        public int StrideH { get; }

        public int StrideW { get; }

        public int DilationH { get; }

        public int DilationW { get; }

        public ValueRange<float> FusedActivation { get; }

        public CPUDepthwiseConv2D(Shape inputShape, DenseTensor<float> weights, DenseTensor<float> bias, Padding paddingH, Padding paddingW, int strideH, int strideW, int dilationH, int dilationW, ValueRange<float> fusedActivation)
        {
            Weights = weights;
            Bias = bias;
            PaddingH = paddingH;
            PaddingW = paddingW;
            StrideH = strideH;
            StrideW = strideW;
            DilationH = dilationH;
            DilationW = dilationW;
            FusedActivation = fusedActivation;

            var outputShape = new Shape(
                inputShape[0],
                ShapeUtility.GetWindowedOutputSize(inputShape[1] + paddingH.Sum, weights.Dimensions[1], strideH, dilationH, false),
                ShapeUtility.GetWindowedOutputSize(inputShape[2] + paddingW.Sum, weights.Dimensions[2], strideW, dilationW, false),
                weights.Dimensions[0]);

            Input = AddInput("input", DataType.Float32, inputShape);
            Output = AddOutput("output", DataType.Float32, outputShape);
        }
    }
}
