using Geometry;

namespace UnitTests;

public class ShapeTests {
  Shape? s1;
  Point? p1;
  List<Point>? lp;

  [SetUp]
  public void Setup() {
  }

  [TearDown]
  public void TearDown() {
    s1 = null;
    p1 = null;
    lp = null;
  }

  public List<Point> createListOfPoints(params (decimal, decimal)[] coords) {
    List<Point> result = new List<Point> { };
    foreach ((decimal, decimal) coord in coords) {
      result.Add(new Point(coord.Item1, coord.Item2));
    }
    return result;
  }

  public Shape createShapeFromPoints(params (decimal, decimal)[] coords) {
    Shape s = new Shape();
    lp = createListOfPoints(coords);
    foreach (Point p in lp) {
      s.addVertex(p);
    }
    s.finalize();
    return s;
  }

  public Point createPoint(decimal x, decimal y) {
    return new Point(x, y);
  }

  [Test]
  public void TestConstructor_NRM1() {
    s1 = new Shape();
    Assert.That(s1.GetType(), Is.EqualTo(typeof(Shape)));
  }

  [Test]
  public void TestAddPointFinalize_NRM1() {
    s1 = new Shape();
    lp = createListOfPoints((0, 0), (1, 1), (1, 0));
    foreach (Point p in lp) {
      s1.addVertex(p);
    }
    s1.finalize();
    Assert.Pass();
  }

  [Test]
  public void TestAddPointFinalize_NRM2() {
    //   * **
    //   **
    //   * **
    //  * *
    //   * *
    s1 = new Shape();
    lp = createListOfPoints(
      (1, 1), (5, 1), (5, 5)
    );
    foreach (Point p in lp) {
      s1.addVertex(p);
    }
    s1.finalize();
    Assert.Pass();
  }

  [Test]
  public void TestAddPointFinalize_NRM3() {
    //   * **
    //   **
    //   * **
    //  * *
    //   * *
    s1 = new Shape();
    lp = createListOfPoints(
      (0, 1), (0, 2), (0, 3), (1, 2), (2, 3), (3, 3),
      (3, 1), (2, 1), (2, -1), (1, 0), (0, -1), (-1, 0)
    );
    foreach (Point p in lp) {
      s1.addVertex(p);
    }
    s1.finalize();
    Assert.Pass();
  }

  [Test]
  public void TestAddPointFinalize_ERR1() {
    s1 = new Shape();
    p1 = null;
    lp = createListOfPoints(
      (1, 1), (5, 1), (5, 1)
    );
    try {
      foreach (Point p in lp) {
        p1 = p;
        s1.addVertex(p);
      }
      Assert.Fail();
    }
    catch (GeometryException e) {
      // Same point added
      // Current point to be added is (5, 1)
      Assert.That(p1, Is.EqualTo(lp[lp.Count - 1]));
      Assert.That(e.getReason(), Is.EqualTo(GeometryExceptionType.POINT_INVALID));
      return;
    }
  }

  [Test]
  public void TestAddPointFinalize_ERR2() {
    s1 = new Shape();
    p1 = null;
    lp = createListOfPoints(
      (1, 1), (5, 1), (5, 5), (4, 0)
    );
    try {
      foreach (Point p in lp) {
        p1 = p;
        s1.addVertex(p);
      }
      Assert.Fail();
    }
    catch (GeometryException e) {
      // Sides cross each other
      // Current point to be added is (4, 0)
      Assert.That(p1, Is.EqualTo(lp[lp.Count - 1]));
      Assert.That(e.getReason(), Is.EqualTo(GeometryExceptionType.POINT_INVALID));
      return;
    }
    Assert.Fail();
  }

  [Test]
  public void TestAddPointFinalize_ERR3() {
    s1 = new Shape();
    lp = createListOfPoints(
      (1, 1), (5, 1)
    );

    foreach (Point p in lp) {
      p1 = p;
      s1.addVertex(p);
    }

    try {
      // Not enough vertices to form a shape
      s1.finalize();
      Assert.Fail();
    }
    catch (GeometryException e) {
      Assert.That(e.getReason(), Is.EqualTo(GeometryExceptionType.SHAPE_INCOMPLETE));
      return;
    }
  }

  [Test]
  public void TestAddPointFinalize_ERR4() {
    s1 = new Shape();
    lp = createListOfPoints(
      (0, 0), (0, 1), (1, 0), (1, 1)
    );
    foreach (Point p in lp) {
      p1 = p;
      s1.addVertex(p);
    }
    try {
      s1.finalize();
    }
    catch (GeometryException e) {
      // Invalid side from (1,1) to (0,0)
      Assert.That(e.getReason(), Is.EqualTo(GeometryExceptionType.POINT_INVALID));
      return;
    }
  }

  [Test]
  public void TestAddPointFinalize_ERR5() {
    s1 = new Shape();
    lp = createListOfPoints(
      (-71, -51), (51, 69), (41, -71), (17, -32), (-75, -16)
    );
    try {
      foreach (Point p in lp) {
        p1 = p;
        s1.addVertex(p);
      }
      Assert.Fail();
    }
    catch (GeometryException e) {
      // (17, -32), (-75, -16) crosses (-71, -51), (51, 69)
      // Current point to be added is (-75, -16)
      Assert.That(p1, Is.EqualTo(lp[lp.Count - 1]));
      Assert.That(e.getReason(), Is.EqualTo(GeometryExceptionType.POINT_INVALID));
      return;
    }
  }

  [Test]
  public void TestAddPointFinalize_ERR6() {
    s1 = new Shape();
    lp = createListOfPoints(
      (62, 18), (-22, 86), (88, 0), (97, 30)
    );
    foreach (Point p in lp) {
      p1 = p;
      s1.addVertex(p);
    }
    try {
      s1.finalize();
      Assert.Fail();
    }
    catch (GeometryException e) {
      // (97, 30), (62, 18) crosses (-22, 86), (88, 0)
      Assert.That(e.getReason(), Is.EqualTo(GeometryExceptionType.POINT_INVALID));
      return;
    }
  }

  [Test]
  public void TestIsPointInside_NRM1() {
    s1 = createShapeFromPoints((1, 1), (5, 1), (5, 5));
    p1 = new Point(3, 2);
    Assert.True(s1.isPointInside(p1));
  }

  [Test]
  public void TestIsPointInside_NRM2() {
    s1 = createShapeFromPoints((1, 1), (5, 1), (5, 5));
    p1 = new Point(0, 1);
    Assert.False(s1.isPointInside(p1));
  }

  [Test]
  public void TestIsPointInside_NRM3() {
    // Create shape
    s1 = new Shape();
    lp = createListOfPoints(
      (0, 0), (-2, 0), (-2, -2), (0, -2), (2, 0), (5, -2),
      (5, 1), (6, 2), (5, 4), (4, 3), (3, 4), (2, 3)
    );
    foreach (Point p in lp) {
      s1.addVertex(p);
    }
    s1.finalize();

    // Test each vertex is inside the shape
    foreach (Point p in lp) {
      Assert.True(s1.isPointInside(p));
    }

    // Test point that lies on the sides is inside the shape
    Assert.True(s1.isPointInside(createPoint(-1, 0)));
    Assert.True(s1.isPointInside(createPoint(-2, -1)));
    Assert.True(s1.isPointInside(createPoint(-1, -2)));
    Assert.True(s1.isPointInside(createPoint(1, -1)));
    Assert.True(s1.isPointInside(createPoint(5, 0)));
    Assert.True(s1.isPointInside(createPoint(5.5M, 3)));
    Assert.True(s1.isPointInside(createPoint(1, 1.5M)));

    // Test point is inside the shape
    Assert.True(s1.isPointInside(createPoint(-1, -1)));
    Assert.True(s1.isPointInside(createPoint(1, 0)));
    Assert.True(s1.isPointInside(createPoint(2, 1)));
    Assert.True(s1.isPointInside(createPoint(2, 2)));
    Assert.True(s1.isPointInside(createPoint(3, 0)));
    Assert.True(s1.isPointInside(createPoint(3, 1)));
    Assert.True(s1.isPointInside(createPoint(3, 2)));
    Assert.True(s1.isPointInside(createPoint(3, 3)));
    Assert.True(s1.isPointInside(createPoint(4, -1)));

    // Test point is outside the shape
    Assert.False(s1.isPointInside(createPoint(-3, -3)));
    Assert.False(s1.isPointInside(createPoint(-3, -2)));
    Assert.False(s1.isPointInside(createPoint(-3, -1)));
    Assert.False(s1.isPointInside(createPoint(-3, -0)));
    Assert.False(s1.isPointInside(createPoint(0, 1)));
    Assert.False(s1.isPointInside(createPoint(0, 2)));
    Assert.False(s1.isPointInside(createPoint(0, 3)));
    Assert.False(s1.isPointInside(createPoint(0, 4)));
    Assert.False(s1.isPointInside(createPoint(0, 5)));
    Assert.False(s1.isPointInside(createPoint(0, 6)));
    Assert.False(s1.isPointInside(createPoint(2, -1)));
    Assert.False(s1.isPointInside(createPoint(2, -2)));
    Assert.False(s1.isPointInside(createPoint(4, 4)));
    Assert.False(s1.isPointInside(createPoint(6, -2)));
    Assert.False(s1.isPointInside(createPoint(6, 1)));
    Assert.False(s1.isPointInside(createPoint(7, 2)));
  }

  [Test]
  public void TestIsPointInside_ERR1() {
    s1 = new Shape();
    lp = createListOfPoints(
      (1, 1), (5, 1), (5, 5)
    );
    foreach (Point p in lp) {
      s1.addVertex(p);
    }
    p1 = createPoint(1, 1);
    try {
      s1.isPointInside(p1);
      Assert.Fail();
    }
    catch (GeometryException e) {
      // Shape is not finalized
      Assert.That(e.getReason(), Is.EqualTo(GeometryExceptionType.SHAPE_NOT_FINALIZED));
      return;
    }
  }

  [Test]
  public void TestGenRandom_NRM1() {
    for (int i = 0; i < 10000; i++) {
      s1 = Shape.genRandom();
      Assert.That(s1, Is.Not.EqualTo(null));
      Assert.That(s1.GetType(), Is.EqualTo(typeof(Shape)));
    }
    Assert.Pass();
  }
}