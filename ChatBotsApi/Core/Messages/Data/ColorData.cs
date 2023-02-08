using System;

namespace ChatBotsApi.Core.Messages.Data
{
    [Serializable]
    public readonly struct ColorData
    {
        public int A { get; }
        public int R { get; }
        public int G { get; }
        public int B { get; }

        public ColorData(int r, int g, int b, int a = 1)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }
    }
}