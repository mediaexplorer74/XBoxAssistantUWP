// *********************************************************
// Type: Xbox.Live.Phone.Utils.LiveAppMath
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll


namespace Xbox.Live.Phone.Utils
{
  public static class LiveAppMath
  {
    private const float TwoPi = 6.28318548f;

    public static float FindShortPathAround(float from, float to)
    {
      if ((double) to < (double) from)
        to += 6.28318548f;
      if ((double) to - (double) from > 3.1415927410125732)
        to -= 6.28318548f;
      return to - from;
    }

    public static float SanitizeAngle(float angle)
    {
      angle %= 6.28318548f;
      if ((double) angle < 0.0)
        angle += 6.28318548f;
      return angle;
    }

    public static bool IsPowerOfTwo(int value) => value > 0 && (value & value - 1) == 0;

    public static int Int32LowBit(int x)
    {
      if (x == 0)
        return 0;
      int num = 0;
      for (; (x & 1) == 0; x >>= 1)
        ++num;
      return 1 << num;
    }
  }
}
