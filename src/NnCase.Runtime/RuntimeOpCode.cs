﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NnCase.Runtime
{
    public enum RuntimeOpCode
    {
        Binary = 0,
        Concat = 1,
        Conv2D = 2,
        Dequantize = 3,
        MatMul = 4,
        Pad = 5,
        Quantize = 6,
        Reduce = 7,
        ReduceWindow2D = 8,
        MemoryCopy = 9,
        ResizeBilinear = 10,
        ResizeNearestNeighbor = 11,
        Softmax = 12,
        Transpose = 13,
        StridedSlice = 14,

        CPU_CPUConv2D = 1001,
        CPU_CPUDepthwiseConv2D = 1002,
        CPU_CPUReduceWindow2D = 1003,
        CPU_CPUQuantizedConv2D = 1004,
        CPU_CPUQuantizedDepthwiseConv2D = 1005,

        K210_KPUUpload = 2001,
        K210_KPUConv2D = 2002
    }
}
