namespace Geometry {

  public class Shape {
    public readonly List<Point> vertices;
    public readonly List<Line> sides;

    public Shape() {
      vertices = new List<Point> { };
      sides = new List<Line> { };
    }

    public static Shape getRandomShape() {
      return null;
    }

    public bool vertexExists(Point p) {
      foreach (Point vertex in vertices) {
        if (p.isEqual(vertex)) return true;
      }
      return false;
    }

    public bool validateSide(Line line, bool zFinal = false) {
      IntersectType intType;
      for (int i = 0; i < sides.Count; i++) {
        intType = line.getIntersectType(sides[i]);
        if ((i == 0 && zFinal) || i == sides.Count - 1) {
          // 1. line is the final side to be added, it must connect to the first vertex
          // 2. line must connect to the last added vertex
          if (intType != IntersectType.POINT_TO_POINT) {
            // Should not reach, possibly sides list is not valid
            throw new GeometryException(GeometryExceptionType.LINE_INVALID);
          }
        }
        else {
          // All other sides should not intersect with line
          if (intType != IntersectType.NONE) return false;
        }
      }
      return true;
    }

    public void addPoint(Point p) {
      // Check whether same vertex already exists
      if (this.vertexExists(p)) {
        throw new GeometryException(GeometryExceptionType.POINT_INVALID);
      }
      if (vertices.Count >= 1) {
        // Error flag
        bool zError = false;
        // Line = last vertex to p
        Line newLine = new Line(vertices[vertices.Count - 1], p);
        // Check new side is valid
        if (sides.Count >= 1) {
          if (!this.validateSide(newLine)) zError = true;
        }
        // If p is possible to form a shape (p is the third vertex)
        // Also check the line from p to first vertex is valid
        if (sides.Count >= 2) {
          Line newLine2 = new Line(p, vertices[0]);
          if (!this.validateSide(newLine)) zError = true;
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
      // Check the line from final vertex to first vertex again
      Point p = vertices[vertices.Count - 1];
      Line newLine = new Line(vertices[vertices.Count - 1], vertices[0]);
      if (this.validateSide(newLine, true)) {
        sides.Add(newLine);
      }
      else {
        throw new GeometryException(GeometryExceptionType.POINT_INVALID);
      }
    }

    public bool isPointInside(Point p) {
      // Check p = any of the vertices
      foreach (Point vertex in vertices) {
        if (p.isEqual(vertex)) return true;
      }

      // Create a horizontal line to the furthest right vertex x
      decimal maxX = vertices[0].x;
      vertices.ForEach((Point p1) => maxX = (p1.x > maxX) ? p1.x : maxX);
      // Check maxX is larger than p.x
      // If not, make a valid line from p.x to p.x + 1
      maxX = (maxX > p.x) ? maxX : p.x + 1;
      Line testLine = new Line(p, new Point(maxX, p.y));

      // Number of crossings between testLine and sides
      // Odd means inside, even means outside
      int count = 0;

      // Position of previous other vertex (above or below testLine)
      int prevPos = 0;

      // Check testLine with each sides
      foreach (Line side in sides) {
        // First Check p on any of the sides
        if (side.checkPoint(p)) return true;
        // Check  intersect with how many sides
        IntersectType intType = testLine.getIntersectType(side);
        Point pOther;
        switch (intType) {
          case IntersectType.POINT_TO_POINT:
          // testLine p2 touches one of the vertices
          case IntersectType.LINE_TO_POINT:
            // testLine line segment touches one of the vertices

            // Determine whether testLine crosses the side
            // If consecutive sides' other vertices lie in opposite of testLine, count as a cross
            // If both vertices lie on same direction, does not count

            // Find the other vertex of the side is above or below testLine
            pOther = (side.p1.y == p.y) ? side.p2 : side.p1;
            if (pOther.y == p.y) {
              // Redundancy check if the other vertex also lies on the testLine
              // If it is, intersect should be OVERLAP
              // Should not reach here
              throw new GeometryException(GeometryExceptionType.OPERATION_INVALID);
            }

            switch (prevPos) {
              case 0:
                if (pOther.y > p.y) prevPos = 1;
                else if (pOther.y < p.y) prevPos = -1;
                break;
              case -1:
              // previous point is below testLine
              // check this point is above testLine
              case 1:
                // previous point is above testLine
                // check this point is below testLine
                if ((prevPos == 1 && pOther.y < p.y) ||
                    (prevPos == -1 && pOther.y > p.y)) {
                  // Opposite direction
                  count++;
                }
                // Reset position flag
                prevPos = 0;
                break;
              default:
                // Should not reach here
                throw new GeometryException(GeometryExceptionType.OPERATION_INVALID);
            }
            break;
          case IntersectType.POINT_TO_LINE:
            // Simple case, testLine p2 touches one of the side
          case IntersectType.LINE_TO_LINE:
            // Simple case, testLine and side cross each other
            count++;
            break;
          case IntersectType.OVERLAP:
          // Do nothing, check the position of the previous and next sides
          case IntersectType.NONE:
            // No intersection
            break;
          case IntersectType.EQUIVALENT:
          // Impossible to be equivalent as it means p is one of the vertices
          default:
            // Invalid intersect type
            throw new GeometryException(GeometryExceptionType.OPERATION_INVALID);
        }
      }

      // Odd = point is inside
      // Even = point is outside
      if (count % 2 == 1) {
        return true;
      }
      return false;
    }
  }
}