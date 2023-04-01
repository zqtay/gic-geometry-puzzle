using ExtensionMethods;

namespace Geometry {

  /// <summary>
  /// Class for 2D coordinate (x, y)
  /// </summary>
  public class Point {
    /// <summary>
    /// x coordinate of the point
    /// </summary>
    public readonly decimal x;
    /// <summary>
    /// y coordinate of the point
    /// </summary>
    public readonly decimal y;

    /// <summary>
    /// Constructor with given coordinates
    /// </summary>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    public Point(decimal x, decimal y) {
      this.x = x;
      this.y = y;
    }

    /// <summary>
    /// Convert coordinate to string
    /// </summary>
    /// <returns>String "(x,y)"</returns>
    public String toString() {
      return $"({this.x},{this.y})";
    }

    /// <summary>
    /// Check if this point is equal to the given point
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public bool isEqual(Point p) {
      return (this.x.AlmostEqual(p.x) && this.y.AlmostEqual(p.y));
    }
  }
}