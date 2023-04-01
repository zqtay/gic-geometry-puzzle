using ExtensionMethods;

namespace Geometry {

  /// <summary>
  /// Types of line intersection
  /// </summary>
  public enum IntersectType : ushort {
    NONE = 0, // Not intersecting
    POINT_TO_POINT = 1, // One endpoint is connected to the other line endpoint
    POINT_TO_LINE = 2, // One endpoint is connected to the other line segment
    LINE_TO_LINE = 3, // Lines cross each other
    OVERLAP = 4, // Line is colinear and overlapping
    EQUIVALENT = 5 // Both lines are equal
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
    public readonly decimal a, b, c;

    /// <summary>
    /// Constructor with two points
    /// </summary>
    /// <param name="p1">Point 1</param>
    /// <param name="p2">Point 2</param>
    public Line(Point p1, Point p2) {
      if (p1.isEqual(p2)) {
        throw new GeometryException(GeometryExceptionType.LINE_SAME_ENDPOINTS);
      }
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
    public Line(decimal a, decimal b, decimal c) {
      this.a = a;
      this.b = b;
      this.c = c;
      this.p1 = null;
      this.p2 = null;
    }

    /// <summary>
    /// Check if this line has both endpoints
    /// </summary>
    /// <returns>true if endpoints exist</returns>
    public bool hasEndPoints() {
      return (this.p1 != null && this.p2 != null);
    }

    /// <summary>
    /// Check if this line is parallel to the given line
    /// </summary>
    /// <param name="line">Line to check against</param>
    /// <returns>true if both lines are parallel</returns>
    public bool isParallel(Line line) {
      return ((this.a * line.b) - (line.a * this.b)).AlmostEqual(0);
    }

    /// <summary>
    /// Check if this line is co-linear with the given line (exist on the same line)
    /// </summary>
    /// <param name="line">Line to check against</param>
    /// <returns>true if both lines are colinear</returns>
    public bool isColinear(Line line) {
      decimal ac = (this.a * line.c) - (line.a * this.c);
      decimal bc = (this.b * line.c) - (line.b * this.c);
      return (this.isParallel(line) && ac.AlmostEqual(0) && bc.AlmostEqual(0));
    }

    /// <summary>
    /// Check if any of the endpoints of this line is connected to the other line's
    /// </summary>
    /// <param name="line">Line to check against</param>
    /// <returns>true if any endpoints are connected</returns>
    public bool isPointConnected(Line line) {
      if (!this.hasEndPoints() || !line.hasEndPoints()) {
        throw new GeometryException(GeometryExceptionType.LINE_NO_ENDPOINTS);
      }
      return (this.p1.isEqual(line.p1) || this.p1.isEqual(line.p2) || this.p2.isEqual(line.p2));
    }

    /// <summary>
    /// Check if this line is equivalent to the given line
    /// </summary>
    /// <param name="line">Line to check against</param>
    /// <returns>true if both lines are equivalent</returns>
    public bool isEqual(Line line) {
      if (this.hasEndPoints() && line.hasEndPoints()) {
        // Compare endpoints if exist
        return (this.p1.isEqual(line.p1) && this.p2.isEqual(line.p2)) ||
               (this.p1.isEqual(line.p2) && this.p2.isEqual(line.p1));
      }
      else if (!this.hasEndPoints() && !line.hasEndPoints()) {
        // Both line have no endpoints
        return (this.a == line.a && this.b == line.b && this.c == line.c);
      }
      // One line has endpoints but other dont
      return false;
    }

    /// <summary>
    /// Get the intersecting point of this line and given line
    /// Both lines must not be parallel
    /// </summary>
    /// <param name="line">Line to check against</param>
    /// <returns>Point of intersection</returns>
    public Point getIntersectPoint(Line line) {
      decimal ab = (this.a * line.b) - (line.a * this.b);
      decimal bc = (this.b * line.c) - (line.b * this.c);
      decimal ca = (this.c * line.a) - (line.c * this.a);
      if (ab.AlmostEqual(0)) {
        throw new GeometryException(GeometryExceptionType.LINE_PARALLEL);
      }
      return new Point(bc / ab, ca / ab);
    }

    /// <summary>
    /// Check if this line is intersecting with the given line
    /// </summary>
    /// <param name="line">Line to check against</param>
    /// <returns>Type of intersection IntersectType</returns>
    public IntersectType getIntersectType(Line line) {
      bool zParallel = this.isParallel(line);
      bool zColinear = this.isColinear(line);
      bool zThisHasEP = this.hasEndPoints();
      bool zLineHasEP = line.hasEndPoints();

      // Exact same lines
      if (this.isEqual(line)) return IntersectType.EQUIVALENT;
      // Both line parallel and not colinear
      if (zParallel && !zColinear) return IntersectType.NONE;

      if (!zThisHasEP && !zLineHasEP) {
        // No endpoints
        // Lines intersect if not parallel
        if (!zParallel) return IntersectType.LINE_TO_LINE;
        else return IntersectType.NONE;
      }
      else {
        // One or both lines has endpoints
        // Check if lines are point to point connected
        if (zThisHasEP && zLineHasEP && this.isPointConnected(line)) {
          if (zColinear) {
            // POINT_TO_POINT or OVERLAP
            // One line folds on another from the same endpoint
            if (this.checkPoint(line.p1) && this.checkPoint(line.p2)) return IntersectType.OVERLAP;
            else if (line.checkPoint(this.p1) && line.checkPoint(this.p2)) return IntersectType.OVERLAP;
            // Both line extends out from the same endpoint
            else return IntersectType.POINT_TO_POINT;
          }
          else {
            return IntersectType.POINT_TO_POINT;
          }
        }
        // Both line colinear, check boundaries
        if (zColinear) {
          // Check any endpoints are within the other
          if (zThisHasEP) {
            if (line.checkPoint(this.p1) || line.checkPoint(this.p2)) return IntersectType.OVERLAP;
            else return IntersectType.NONE;
          }
          else if (zLineHasEP) {
            if (this.checkPoint(line.p1) || this.checkPoint(line.p2)) return IntersectType.OVERLAP;
            else return IntersectType.NONE;
          }
        }
        // Not parallel
        if (!zParallel) {
          Point pInt = this.getIntersectPoint(line);
          // Check the extrapolated intersect point is on both lines
          // If it is, the actual line segments are intersecting
          if (this.checkPoint(pInt) && line.checkPoint(pInt)) {
            // POINT_TO_LINE or LINE_TO_LINE
            if ((zThisHasEP && (this.p1.isEqual(pInt) || this.p2.isEqual(pInt))) ||
                (zLineHasEP && (line.p1.isEqual(pInt) || line.p2.isEqual(pInt)))) {
              // Intersection point is one of the endpoints of one of the lines
              return IntersectType.POINT_TO_LINE;
            }
            else {
              // Both lines cross each other
              return IntersectType.LINE_TO_LINE;
            }
          }
          else {
            // Not intersecting
            return IntersectType.NONE;
          }
        }
        // Invalid case, all cases should be handled
        throw new GeometryException(GeometryExceptionType.LINE_INVALID);
      }
    }

    /// <summary>
    /// Check whether a point is on the line
    /// </summary>
    /// <param name="p">Point to be checked</param>
    /// <returns>true if point is on the line and within boundaries</returns>
    public bool checkPoint(Point p) {
      // Check if the point is on the extrapolated line
      bool result = (this.a * p.x + this.b * p.y + this.c).AlmostEqual(0);
      // Check if the point is within boundaries (if present)
      if (result && this.p1 != null && this.p2 != null) {
        result = ((p.x >= this.p1.x && p.x <= this.p2.x) || (p.x >= this.p2.x && p.x <= this.p1.x)) &&
                 ((p.y >= this.p1.y && p.y <= this.p2.y) || (p.y >= this.p2.y && p.y <= this.p1.y));
      }
      return result;
    }

    /// <summary>
    /// Get the slope of the line
    /// </summary>
    /// <returns></returns>
    public decimal getSlope() {
      return -(this.a / this.b);
    }
  }
}