using System;

namespace Models
{
    public static class Activation
    {
        public static float ReLU(float value) => Math.Max(0, value);
        public static float Sigmoid(float value) => (float)(1 / 1 + Math.Exp(-value));
    }
}

