public static class Ease
{

  public static float QuadInOut(float a, float b, float t)
  {
    t *= 2;
    if (t < 1)
    {
      return QuadIn(a, ((b - a) / 2) + a, t);
    }

    t -= 1;
    return QuadOut(((b - a) / 2) + a, b, t);
  }

  public static float QuadPop(float a, float b, float t)
  {
    t *= 2;
    if (t < 1)
    {
      return QuadOut(a, ((b - a) / 2) + a, t);
    }

    t -= 1;
    return QuadIn(((b - a) / 2) + a, b, t);
  }

  public static float QuadIn(float a, float b, float t)
  {
    if (t < 0)
    {
      t = 0;
    }
    else if (t > 1)
    {
      t = 1;
    }

    return (b - a) * t * t + a;
  }

  public static float QuadOut(float a, float b, float t)
  {
    if (t < 0)
    {
      t = 0;
    }
    else if (t > 1)
    {
      t = 1;
    }

    return -(b - a) * t * (t - 2) + a;
  }

}
