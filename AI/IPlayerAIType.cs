﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core;

namespace AI
{
    public interface IPlayerAIType
    {
        PlayerAIResult Determine(PropertyBag factor);
        void SetReporter(IReporter er);
    }
}
