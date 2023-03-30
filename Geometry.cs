namespace Geometry {
  /// <summary>
  /// Class for 2D coordinate (x, y)
  /// </summary>
  public class Point {
    public readonly double x;
    public readonly double y;
    public Point(double x, double y) {
      this.x = x;
      this.y = y;
    }
  }

  /// <summary>
  /// Class for linear operation
  /// </summary>
  public class Line {
    /// <summary>
    /// Two points make a line
    /// </summary>
    public readonly Point p1, p2;
    // ax + by + c = 0
    /// <summary>
    /// Coefficients for general linear equation ax + by + c = 0
    /// </summary>
    public readonly double a, b, c;

    /// <summary>
    /// Constructor with two points
    /// </summary>
    /// <param name="p1">Point 1</param>
    /// <param name="p2">Point 2</param>
    public Line(Point p1, Point p2) {
      this.p1 = p1;
      this.p2 = p2;
      this.a = this.p1.y - this.p2.y;
      this.b = this.p2.x - this.p1.x;
      this.c = this.p1.x * this.p2.y - this.p2.x * this.p1.y;
      if (this.a < 0) {
        // Force x coefficient to positive
        this.a *= -1;
        this.b *= -1;
        this.c *= -1;
      }
    }

    /// <summary>
    /// Constructor with coeffficients of linear equation: ax + by + c = 0
    /// </summary>
    /// <param name="a">Coeffeicent for x</param>
    /// <param name="b">Coeffeicent for y</param>
    /// <param name="c">Constant</param>
    public Line(double a, double b, double c) {
      this.a = a;
      this.b = b;
      this.c = c;
      if (this.a < 0) {
        // Force x coefficient to positive
        this.a *= -1;
        this.b *= -1;
        this.c *= -1;
      }
      this.p1 = null;
      this.p2 = null;
    }

    /// <summary>
    /// Check if two lines are parallel
    /// </summary>
    /// <param name="l1">Line 1</param>
    /// <param name="l2">Line 2</param>
    /// <returns>true if both lines are parallel</returns>
    public static bool areParallel(Line l1, Line l2) {
      return ((l1.a * l2.b) - (l2.a * l1.b) == 0);
    }

    /// <summary>
    /// Check if two lines are co-linear (exist on the same line)
    /// </summary>
    /// <param name="l1">Line 1</param>
    /// <param name="l2">Line 2</param>
    /// <returns>true if both lines are colinear</returns>
    public static bool areColinear(Line l1, Line l2) {
      double ac = (l1.a * l2.c) - (l2.a * l1.c);
      double bc = (l1.b * l2.c) - (l2.b * l1.c);
      return (areParallel(l1, l2) && ac == 0 && bc == 0);
    }

    /// <summary>
    /// Get the intersecting point of two lines
    /// Both lines must not be parallel
    /// </summary>
    /// <param name="l1">Line 1</param>
    /// <param name="l2">Line 2</param>
    /// <returns>Point of intersection</returns>
    public static Point getIntersect(Line l1, Line l2) {
      double ab = (l1.a * l2.b) - (l2.a * l1.b);
      double bc = (l1.b * l2.c) - (l2.b * l1.c);
      double ca = (l1.c * l2.a) - (l2.c * l1.a);
      return new Point(bc / ab, ca / ab);
    }

    /// <summary>
    /// Check if two lines are intersecting
    /// </summary>
    /// <param name="l1">Line 1</param>
    /// <param name="l2">Line 2</param>
    /// <returns>true if both lines intersect</returns>
    public static bool areIntersect(Line l1, Line l2) {
      bool zParallel = areParallel(l1, l2);
      bool zColinear = areColinear(l1, l2);
      if (l1.p1 == null && l1.p2 == null && l2.p1 == null && l2.p2 == null) {
        // No point limits
        // Lines intersect if not parallel or is colinear
        return !zParallel || zColinear;
      } else {
        // Both line parallel and not colinear
        if (zParallel && !zColinear) return false;
        // Both line colinear, check range
        if (zColinear) {
          // Check any endpoints are within the other
          if (l1.p1 != null && l1.p2 != null) {
            return l2.checkPoint(l1.p1) || l2.checkPoint(l1.p2);
          } else if (l2.p1 != null && l2.p2 != null) {
            return l1.checkPoint(l2.p1) || l1.checkPoint(l2.p2);
          } else {
            // This case should be extrapolate case
            throw new GeometryException("Invalid line");
          }
        }
        // Not parallel
        if (!zParallel) {
          Point pInt = getIntersect(l1, l2);
          // Check the extrapolated intersect point is on both lines
          // If it is, the actual line segments are intersecting
          return l1.checkPoint(pInt) && l2.checkPoint(pInt);
        }
        // Invalid case, all cases should be handled
        throw new GeometryException("Invalid line");
      }
    }

    /// <summary>
    /// Get the slope of the line
    /// </summary>
    /// <returns></returns>
    public double getSlope() {
      return -(this.a / this.b);
    }

    /// <summary>
    /// Check whether a point is on the line
    /// </summary>
    /// <param name="p">Point to be checked</param>
    /// <returns>true if point is on the line and within range</returns>
    public bool checkPoint(Point p) {
      // Check if the point is on the extrapolated line
      bool result = (this.a * p.x + this.b * p.y + this.c == 0);
      // Check if the point is within range (if present)
      if (result && this.p1 != null && this.p2 != null) {
        var minX = this.p1.x < this.p2.x ? this.p1.x : this.p2.x;
        var maxX = this.p1.x > this.p2.x ? this.p1.x : this.p2.x;
        var minY = this.p1.y < this.p2.y ? this.p1.y : this.p2.y;
        var maxY = this.p1.y > this.p2.y ? this.p1.y : this.p2.y;
        result = (p.x >= minX && p.x <= maxX) && (p.y >= minY && p.y <= maxY);
      }
      return result;
    }
  }

  /// <summary>
  /// Custom exception for Geometry
  /// </summary>
  public class GeometryException : System.Exception {
    public GeometryException() { }
    public GeometryException(string message) : base(message) { }
  }
}