﻿using System;
using System.Collections.Generic;
using System.Numerics.Tensors;
using System.Text;

namespace NnCase
{
    public static class TensorExtensions
    {
        public static DenseTensor<T> Transpose<T>(this DenseTensor<T> tensor, ReadOnlySpan<int> axes)
        {
            int inputExtSize = 4 - tensor.Rank;
            int outputExtSize = 4 - axes.Length;

            Span<int> extendedPerm = stackalloc int[4];
            for (int i = 0; i < outputExtSize; i++)
                extendedPerm[i] = i;
            for (int i = 0; i < axes.Length; i++)
                extendedPerm[i + outputExtSize] = axes[i] + inputExtSize;

            Span<int> outSizes = stackalloc int[4];
            Span<int> oldDims = stackalloc int[4]
            {
                1, 1, 1, 1
            };
            for (int i = 4 - tensor.Dimensions.Length; i < 4; i++)
                oldDims[i] = tensor.Dimensions[i - (4 - tensor.Dimensions.Length)];
            for (int i = 0; i < 4; i++)
                outSizes[i] = oldDims[extendedPerm[i]];

            Span<int> outp = stackalloc int[4];
            Span<int> inp = stackalloc int[4];
            var buffer = new T[tensor.Length];
            var destDimensions = new int[tensor.Dimensions.Length];
            for (int i = 0; i < axes.Length; i++)
                destDimensions[i] = tensor.Dimensions[axes[i]];
            var output = new DenseTensor<T>(buffer, outSizes);
            var reshapedTensor = tensor.Reshape(oldDims);

            for (outp[3] = 0; outp[3] < outSizes[3]; outp[3]++)
            {
                inp[extendedPerm[3]] = outp[3];
                for (outp[2] = 0; outp[2] < outSizes[2]; outp[2]++)
                {
                    inp[extendedPerm[2]] = outp[2];
                    for (outp[1] = 0; outp[1] < outSizes[1]; outp[1]++)
                    {
                        inp[extendedPerm[1]] = outp[1];
                        for (outp[0] = 0; outp[0] < outSizes[0]; outp[0]++)
                        {
                            inp[extendedPerm[0]] = outp[0];
                            output[outp] = reshapedTensor[inp];
                        }
                    }
                }
            }

            return output.Reshape(destDimensions).ToDenseTensor();
        }
    }
}
