namespace Geometry {

  /// <summary>
  /// Class for 2D coordinate (x, y)
  /// </summary>
  public class Point {
    /// <summary>
    /// x coordinate of the point
    /// </summary>
    public readonly double x;
    /// <summary>
    /// y coordinate of the point
    /// </summary>
    public readonly double y;

    /// <summary>
    /// Constructor with given coordinates
    /// </summary>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    public Point(double x, double y) {
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
      return (this.x == p.x && this.y == p.y);
    }
  }
}