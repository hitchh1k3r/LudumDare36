public static class Ease
{

  public static float QuadInOut(float a, float b, float t)
  {
    if (t < 0)
    {
      t = 0;
    }
    else if (t > 1)
    {
      t = 1;
    }

    t *= 2;
    if (t < 1)
    {
      // QUAD IN
      return (b - a) / 2 * t * t + a;
    }

    // QUAD OUT
    t -= 1;
    return -(b - a) / 2 * (t * (t - 2) - 1) + a;
  }

}
