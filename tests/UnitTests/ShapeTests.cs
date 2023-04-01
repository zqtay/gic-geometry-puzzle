using Geometry;

namespace UnitTests;

public class ShapeTests {
  Shape s1;
  Point p1;

  [SetUp]
  public void Setup() {
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
    List<Point> points = createListOfPoints(coords);
    foreach (Point p in points) {
      s.addPoint(p);
    }
    s.finalize();
    return s;
  }

  [Test]
  public void TestConstructor_NRM1() {
    s1 = new Shape();
    Assert.AreEqual(typeof(Shape), s1.GetType());
  }

  [Test]
  public void TestAddPointFinalize_NRM1() {
    s1 = new Shape();
    List<Point> points = createListOfPoints((0, 0), (1, 1), (1, 0));
    foreach (Point p in points) {
      s1.addPoint(p);
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
    List<Point> points = createListOfPoints(
      (1, 1), (5, 1), (5, 5)
    );
    foreach (Point p in points) {
      s1.addPoint(p);
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
    List<Point> points = createListOfPoints(
      (0, 1), (0, 2), (0, 3), (1, 2), (2, 3), (3, 3),
      (3, 1), (2, 1), (2, -1), (1, 0), (0, -1), (-1, 0)
    );
    foreach (Point p in points) {
      s1.addPoint(p);
    }
    s1.finalize();
    Assert.Pass();
  }

  [Test]
  public void TestAddPointFinalize_ERR1() {
    s1 = new Shape();
    p1 = null;
    List<Point> points = createListOfPoints(
      (1, 1), (5, 1), (5, 1)
    );
    try {
      foreach (Point p in points) {
        p1 = p;
        s1.addPoint(p);
      }
      Assert.Fail();
    }
    catch (GeometryException e) {
      Assert.AreEqual(points[points.Count - 1], p1);
      Assert.AreEqual(GeometryExceptionType.POINT_INVALID, e.getReason());
      return;
    }
  }

  [Test]
  public void TestAddPointFinalize_ERR2() {
    s1 = new Shape();
    p1 = null;
    List<Point> points = createListOfPoints(
      (1, 1), (5, 1), (5, 5), (4, 0)
    );
    try {
      foreach (Point p in points) {
        p1 = p;
        s1.addPoint(p);
      }
      Assert.Fail();
    }
    catch (GeometryException e) {
      Assert.AreEqual(points[points.Count - 1], p1);
      Assert.AreEqual(GeometryExceptionType.POINT_INVALID, e.getReason());
      return;
    }
    Assert.Fail();
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
}