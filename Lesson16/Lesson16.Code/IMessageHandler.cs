﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson16.Code
{
    public interface IMessageHandler
    {
        void HandleMessageJson(string dataJson);
    }
}
