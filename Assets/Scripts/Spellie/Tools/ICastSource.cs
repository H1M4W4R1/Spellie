﻿using UnityEngine;

namespace Spellie.Tools
{
    public interface ICastSource
    {
        Transform GetTransform();
        Vector3 GetPosition();
    }
}