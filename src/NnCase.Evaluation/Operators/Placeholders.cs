﻿using System;
using System.Collections.Generic;
using System.Text;
using NnCase.IR;
using NnCase.IR.Operators;

namespace NnCase.Evaluation.Operators
{
    internal static partial class DefaultEvaulators
    {
        private static void RegisterPlaceholders(EvaluatorRegistry registry)
        {
            registry.Add<InputNode>((n, e) => { });
            registry.Add<OutputNode>((n, e) => { });
            registry.Add<Constant>((n, e) => { });
            registry.Add<IgnoreNode>((n, e) => { });
        }
    }
}
