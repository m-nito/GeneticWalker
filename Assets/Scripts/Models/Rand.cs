using System;

namespace Models
{
    public static class Rand
    {
        private static Random _Random = new Random(DateTime.Now.Millisecond + DateTime.Now.Second + DateTime.Now.Minute);
        public static int GetRandom(int from, int to) => _Random.Next(from, to);
        public static float GetRandomFloat() => (float)_Random.NextDouble();
    }
}
