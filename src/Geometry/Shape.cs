namespace Geometry {

  public class Shape {
    public readonly List<Point> vertices;
    public readonly List<Line> sides;

    public Shape() {
      vertices = new List<Point> { };
      sides = new List<Line> { };
    }

    public bool validateSides(Line line) {
      for (int i = 0; i < sides.Count; i++) {
        // Only point to point is allowed
        if (line.getIntersectType(sides[i]) != IntersectType.POINT_TO_POINT) return false;
      }
      return true;
    }

    public void addPoint(Point p) {
      if (vertices.Count >= 1) {
        // Error flag
        bool zError = false;
        // Line = last vertex to p
        Line newLine = new Line(vertices[vertices.Count - 1], p);

        // Check new side is valid
        if (sides.Count >= 1) {
          if (!this.validateSides(newLine)) zError = true;
        }

        // If p is possible to form a shape (p is the third vertex)
        // Also check the line from p to first vertex is valid
        if (sides.Count >= 2) {
          Line newLine2 = new Line(vertices[vertices.Count - 1], vertices[0]);
          if (!this.validateSides(newLine)) zError = true;
        }

        // Check error
        if (zError) {
          throw new GeometryException(GeometryExceptionType.POINT_INVALID);
        }
        else {
          // New side
          sides.Add(newLine);
        }
      }
      // New point
      vertices.Add(p);
    }

    public void finalize() {
      Point p = vertices[vertices.Count - 1];
      Line newLine = new Line(vertices[vertices.Count - 1], vertices[0]);
      if (this.validateSides(newLine)) {
        sides.Add(newLine);
      }
      else {
        throw new GeometryException(GeometryExceptionType.POINT_INVALID);
      }
    }

    public bool checkPointWithin(Point p) {
      // Check p = any of the vertices
      // Check p on any of the sides
      // Check Line(p, new Point(maxX, p.y)) intersect with how many sides
      return false;
    }
  }
}