namespace ExtensionMethods {
  public static class DecimalExtensions {
    public const decimal TOLERANCE = 1.0e-26M;
    public static bool AlmostEqual(this decimal a, decimal b) {
      return Math.Abs(a - b) <= TOLERANCE;
    }
  }
}