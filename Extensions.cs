using System;
using System.Collections.Generic;

namespace MapleShark
{
    public static class Extensions
    {
        public static byte RollLeft(this byte pThis, int pCount)
        {
            uint overflow = ((uint)pThis) << (pCount % 8);
            return (byte)((overflow & 0xFF) | (overflow >> 8));
        }

        public static byte RollRight(this byte pThis, int pCount)
        {
            uint overflow = (((uint)pThis) << 8) >> (pCount % 8);
            return (byte)((overflow & 0xFF) | (overflow >> 8));
        }

        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> pThis, TKey pKey, TValue pDefault)
        {
            TValue result;
            return pThis.TryGetValue(pKey, out result) ? result : pDefault;
        }
    }
}
