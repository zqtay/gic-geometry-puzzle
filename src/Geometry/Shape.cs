namespace Geometry {

  /// <summary>
  /// Class for a polygon shape.
  /// </summary>
  public class Shape {
    /// <summary>
    /// Minimum number of vertices for generating random shape.
    /// </summary>
    public const int VERTICES_MIN = 3;
    /// <summary>
    /// Maximum number of vertices for generating random shape.
    /// </summary>
    public const int RANDOM_VERTICES_MAX = 8;
    /// <summary>
    /// Canvas size boundaries for generating random shape, minimum x.
    /// </summary>
    public const int RANDOM_CANVAS_SIZE_MIN_X = -100;
    /// <summary>
    /// Canvas size boundaries for generating random shape, maximum x.
    /// </summary>
    public const int RANDOM_CANVAS_SIZE_MAX_X = 100;
    /// <summary>
    /// Canvas size boundaries for generating random shape, minimum y.
    /// </summary>
    public const int RANDOM_CANVAS_SIZE_MIN_Y = -100;
    /// <summary>
    /// Canvas size boundaries for generating random shape, maximum y.
    /// </summary>
    public const int RANDOM_CANVAS_SIZE_MAX_Y = 100;

    /// <summary>
    /// List of vertices on the shape.
    /// </summary>
    private List<Point> vertices;
    /// <summary>
    /// List of sides on the shape
    /// </summary>
    private List<Line> sides;
    /// <summary>
    /// Flag to indicate if shape is finalized
    /// </summary>
    private bool zFinal;

    /// <summary>
    /// Constructor for creating an empty shape instance.
    /// </summary>
    public Shape() {
      vertices = new List<Point> { };
      sides = new List<Line> { };
      zFinal = false;
    }

    /// <summary>
    /// Generate a shape with random vertices.
    /// </summary>
    /// <returns>A randomly generated shape.</returns>
    public static Shape genRandom(int maxVertices=RANDOM_VERTICES_MAX) {
      // Random number generator
      Random rnd = new Random();
      // Number of vertices
      int nVertices = rnd.Next(VERTICES_MIN, maxVertices + 1);
      // x, y coordinates for new random point
      // i current loop index
      int x, y, i = 0;
      // New point
      Point p;
      // New shape
      Shape s = new Shape();
      // Current error count
      int errCount = 0;
      // Max number of error count allowed before resetting the shape
      int maxErrCount = 10;
      // Add vertices one by one
      while (i <= nVertices) {
        if (errCount >= maxErrCount) {
          // Reset shape
          s.init();
          i = 0;
          errCount = 0;
        }
        if (i < nVertices) {
          // Generate random point
          x = rnd.Next(RANDOM_CANVAS_SIZE_MIN_X, RANDOM_CANVAS_SIZE_MAX_X + 1);
          y = rnd.Next(RANDOM_CANVAS_SIZE_MIN_Y, RANDOM_CANVAS_SIZE_MAX_Y + 1);
          p = new Point(x, y);
          try {
            s.addVertex(p);
            i++;
          }
          catch (GeometryException e) {
            // Invalid point to be added, try another random point
            errCount++;
            continue;
          }
        }
        else if (i == nVertices) {
          // Finalize shape here
          try {
            s.finalize();
            // Shape is finalized
            break;
          }
          catch (GeometryException e) {
            // Invalid point to be added, revert last added point
            errCount++;
            s.vertices.RemoveAt(s.vertices.Count - 1);
            s.sides.RemoveAt(s.sides.Count - 1);
            i--;
          }
        }
      }
      return s;
    }

    /// <summary>
    /// Get the vertices of the shape
    /// </summary>
    /// <returns>List of points</returns>
    public List<Point> getVertices() {
      return this.vertices;
    }

    /// <summary>
    /// Get the sides of the shape
    /// </summary>
    /// <returns>List of lines</returns>
    public List<Line> getSides() {
      return this.sides;
    }

    /// <summary>
    /// Initialize the shape. <br />
    /// This clears the shape vertices and sides.
    /// </summary>
    public void init() {
      vertices = new List<Point> { };
      sides = new List<Line> { };
    }

    /// <summary>
    /// Check if the given point is a vertex on the shape.
    /// </summary>
    /// <param name="p">Point to check.</param>
    /// <returns>true if point is a vertex.</returns>
    public bool vertexExists(Point p) {
      foreach (Point vertex in vertices) {
        if (p.isEqual(vertex)) return true;
      }
      return false;
    }

    /// <summary>
    /// Check if the given line is a valid side to be added to the shape.
    /// </summary>
    /// <param name="line">Line to be added.</param>
    /// <param name="zFinal">true if line is the final side for the shape.</param>
    /// <returns>true if line is valid.</returns>
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

    /// <summary>
    /// Add a point as vertex to the shape. <br />
    /// This checks if the given point is valid to be added. <br />
    /// If not valid, GeometryException will be thrown.
    /// </summary>
    /// <param name="p">Point to be added</param>
    public void addVertex(Point p) {
      // Check whether same vertex already exists
      if (this.vertexExists(p)) {
        throw new GeometryException(GeometryExceptionType.POINT_INVALID);
      }
      if (vertices.Count >= 1) {
        // Error flag
        // Line = last vertex to p
        Line newLine = new Line(vertices[vertices.Count - 1], p);
        // Check new side is valid
        if (sides.Count >= 1) {
          if (!this.validateSide(newLine)) {
            throw new GeometryException(GeometryExceptionType.POINT_INVALID);
          }
        }
        // New side
        sides.Add(newLine);
      }
      // New point
      vertices.Add(p);
    }

    /// <summary>
    /// Check is current shape valid to be finalized. <br />
    /// This checks if the added vertices are enough to form a shape (minimum 3 vertices). <br />
    /// This also checks the final vertex to the first vertex is a valid side. <br />
    /// </summary>
    /// <returns>true if shape is valid and ready to be finalized.</returns>
    public bool isShapeValidFinal() {
      if (vertices.Count < VERTICES_MIN) return false;
      Point p = vertices[vertices.Count - 1];
      Line newLine = new Line(vertices[vertices.Count - 1], vertices[0]);
      if (this.validateSide(newLine, true)) return true;
      else return false;
    }

    /// <summary>
    /// Finalize a shape. <br />
    /// This checks if the added vertices are enough to form a shape (minimum 3 vertices). <br />
    /// This also checks the final vertex to the first vertex is a valid side. <br />
    /// If shape is not able to be finalized, GeometryException will be thrown.
    /// </summary>
    public void finalize() {
      if (vertices.Count < VERTICES_MIN) {
        throw new GeometryException(GeometryExceptionType.SHAPE_INCOMPLETE);
      }
      Point p = vertices[vertices.Count - 1];
      Line newLine = new Line(vertices[vertices.Count - 1], vertices[0]);
      // Validate new side
      if (this.validateSide(newLine, true)) {
        // Add final side
        sides.Add(new Line(vertices[vertices.Count - 1], vertices[0]));
        // Set final flag to true
        this.zFinal = true;
      }
      else {
        throw new GeometryException(GeometryExceptionType.POINT_INVALID);
      }
    }

    /// <summary>
    /// Check if the given point is inside the shape. <br />
    /// It is determined by extending a line from the point to the right
    /// and check how many sides of the shape are being crossed.<br />
    /// If the number of crossings is odd, the point is inside the shape.
    /// If even, the point is outside. <br />
    /// If point lies on the one of the shape vertices or sides, it counts as inside the shape.
    /// </summary>
    /// <param name="p">Point to check.</param>
    /// <returns></returns>
    public bool isPointInside(Point p) {
      // Shape is not finalized
      if (zFinal != true) throw new GeometryException(GeometryExceptionType.SHAPE_NOT_FINALIZED);

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