using Geometry;

namespace UnitTests;

public class LineTests {
  Point? p1;
  Point? p2;
  Line? l1;
  Line? l2;

  [SetUp]
  public void Setup() {
  }

  [TearDown]
  public void TearDown() {
    p1 = null;
    p2 = null;
    l1 = null;
    l2 = null;
  }

  [Test]
  public void TestConstructor_NRM1() {
    // Line equation: y = 0, 0 <= x <= 1;
    p1 = new Point(0, 0);
    p2 = new Point(1, 0);
    l1 = new Line(p1, p2);
    Assert.True(l1.p1.isEqual(p1));
    Assert.True(l1.p2.isEqual(p2));
    Assert.That(l1.a, Is.EqualTo(0));
    Assert.That(l1.b, Is.EqualTo(1));
    Assert.That(l1.c, Is.EqualTo(0));
  }

  [Test]
  public void TestConstructor_NRM2() {
    // Line equation: x - y + 1 = 0, -1 <= x <= 0;
    p1 = new Point(-3.2M, 4.95M);
    p2 = new Point(7, -6.53M);
    l1 = new Line(p1, p2);
    Assert.True(l1.p1.isEqual(p1));
    Assert.True(l1.p2.isEqual(p2));
    Assert.That(l1.a, Is.EqualTo(11.48M));
    Assert.That(l1.b, Is.EqualTo(10.2M));
    Assert.That(l1.c, Is.EqualTo(-13.754M));
  }

  [Test]
  public void TestConstructor_NRM3() {
    // Line equation: 16x - 88 = 0 (x - 5.5 = 0), -10 <= y <= 6;
    p1 = new Point(5.5M, -10);
    p2 = new Point(5.5M, 6);
    l1 = new Line(p1, p2);
    Assert.True(l1.p1.isEqual(p1));
    Assert.True(l1.p2.isEqual(p2));
    Assert.That(l1.a, Is.EqualTo(16));
    Assert.That(l1.b, Is.EqualTo(0));
    Assert.That(l1.c, Is.EqualTo(-88));
  }

  [Test]
  public void TestConstructor_NRM4() {
    // Line equation: x + 2y + 3 = 0
    l1 = new Line(1, 2, 3);
    Assert.True(l1.p1 == null);
    Assert.True(l1.p2 == null);
    Assert.That(l1.a, Is.EqualTo(1));
    Assert.That(l1.b, Is.EqualTo(2));
    Assert.That(l1.c, Is.EqualTo(3));
  }

  [Test]
  public void TestConstructor_NRM5() {
    // Line equation: -2.5x - 7.43y + 9.1 = 0
    l1 = new Line(-2.5M, -7.43M, 9.1M);
    Assert.True(l1.p1 == null);
    Assert.True(l1.p2 == null);
    Assert.That(l1.a, Is.EqualTo(-2.5M));
    Assert.That(l1.b, Is.EqualTo(-7.43M));
    Assert.That(l1.c, Is.EqualTo(9.1M));
  }

  [Test]
  public void TestConstructor_NRM6() {
    p1 = new Point(1, 2);
    p2 = new Point(3, 4);
    l1 = new Line(p1, p2);
    p1 = new Point(5, 6);
    p2 = new Point(7, 8);
    l2 = new Line(p1, p2);
    Assert.True(l1.p1.isEqual(new Point(1, 2)));
    Assert.True(l1.p2.isEqual(new Point(3, 4)));
    Assert.True(l2.p1.isEqual(new Point(5, 6)));
    Assert.True(l2.p2.isEqual(new Point(7, 8)));
  }

  [Test]
  public void TestConstructor_ERR1() {
    p1 = new Point(1, 2);
    p2 = new Point(1, 2);
    try {
      l1 = new Line(p1, p2);
    }
    catch (GeometryException e) {
      if (e.getReason() == GeometryExceptionType.LINE_SAME_ENDPOINTS) {
        Assert.Pass();
      }
    }
    Assert.Fail();
  }

  [Test]
  public void TestConstructor_ERR2() {
    try {
      l1 = new Line(0, 0, 7);
    }
    catch (GeometryException e) {
      if (e.getReason() == GeometryExceptionType.LINE_INVALID) {
        Assert.Pass();
      }
    }
    Assert.Fail();
  }

  [Test]
  public void TestHasEndPoints_NRM1() {
    p1 = new Point(10, -10);
    p2 = new Point(-5, 5);
    l1 = new Line(p1, p2);
    Assert.True(l1.hasEndPoints());
  }

  [Test]
  public void TestHasEndPoints_NRM2() {
    l1 = new Line(-2.5M, -7.43M, 9.1M);
    Assert.False(l1.hasEndPoints());
  }

  [Test]
  public void TestIsEqual_NRM1() {
    p1 = new Point(1.23M, -4.56M);
    p2 = new Point(7.89M, -0.12M);
    l1 = new Line(p1, p2);
    p1 = new Point(7.89M, -0.12M);
    p2 = new Point(1.23M, -4.56M);
    l2 = new Line(p1, p2);
    Assert.True(l1.isEqual(l2));
    Assert.True(l2.isEqual(l1));
  }

  [Test]
  public void TestIsEqual_NRM2() {
    p1 = new Point(4, 5);
    p2 = new Point(9, 11);
    l1 = new Line(p1, p2);
    p1 = new Point(4, 5);
    p2 = new Point(6, 7);
    l2 = new Line(p1, p2);
    Assert.False(l1.isEqual(l2));
    Assert.False(l2.isEqual(l1));
  }

  [Test]
  public void TestIsEqual_NRM3() {
    l1 = new Line(1.23M, -4.56M, 7.89M);
    l2 = new Line(-2.706M, 10.032M, -17.358M);
    Assert.True(l1.isEqual(l2));
    Assert.True(l2.isEqual(l1));
  }

  [Test]
  public void TestIsEqual_NRM4() {
    l1 = new Line(1.23M, -4.56M, 7.89M);
    l2 = new Line(1.23M, -4.56M, 7.90M);
    Assert.False(l1.isEqual(l2));
    Assert.False(l2.isEqual(l1));
  }

  [Test]
  public void TestIsEqual_NRM5() {
    l1 = new Line(0, -4.56M, 7.89M);
    l2 = new Line(0, -4.56M, 7.89M);
    Assert.True(l1.isEqual(l2));
    Assert.True(l2.isEqual(l1));
  }

  [Test]
  public void TestIsEqual_NRM6() {
    l1 = new Line(0, -4.56M, 7.89M);
    l2 = new Line(1, -4.56M, 7.89M);
    Assert.False(l1.isEqual(l2));
    Assert.False(l2.isEqual(l1));
  }

  [Test]
  public void TestIsParallel_NRM1() {
    p1 = new Point(3, 10);
    p2 = new Point(5, 5);
    l1 = new Line(p1, p2);
    p1 = new Point(-7, -6.5M);
    p2 = new Point(-9, -1.5M);
    l2 = new Line(p1, p2);
    Assert.True(l1.isParallel(l2));
    Assert.True(l2.isParallel(l1));
  }

  [Test]
  public void TestIsParallel_NRM2() {
    p1 = new Point(1, 2);
    p2 = new Point(3, 4);
    l1 = new Line(p1, p2);
    p1 = new Point(1, 2);
    p2 = new Point(3.1M, 4);
    l2 = new Line(p1, p2);
    Assert.False(l1.isParallel(l2));
    Assert.False(l2.isParallel(l1));
  }

  [Test]
  public void TestIsParallel_NRM3() {
    l1 = new Line(1.1M, -2.2M, 3.3M);
    l2 = new Line(-6.7M, 13.4M, 4.4M);
    Assert.True(l1.isParallel(l2));
    Assert.True(l2.isParallel(l1));
  }

  [Test]
  public void TestIsParallel_NRM4() {
    l1 = new Line(1, 2, 3);
    l2 = new Line(1, 2.1M, 3);
    Assert.False(l1.isParallel(l2));
    Assert.False(l2.isParallel(l1));
  }

  [Test]
  public void TestIsColinear_NRM1() {
    // 8x - 3y - 37 = 0
    p1 = new Point(4, -5);
    p2 = new Point(5, 3);
    l1 = new Line(p1, p2);
    p1 = new Point(0, -37);
    p2 = new Point(4.625M, 0);
    l2 = new Line(p1, p2);
    Assert.True(l1.isColinear(l2));
    Assert.True(l2.isColinear(l1));
  }

  [Test]
  public void TestIsColinear_NRM2() {
    // 8x - 3y - 37 = 0
    p1 = new Point(0, 0);
    p2 = new Point(5, 5);
    l1 = new Line(p1, p2);
    p1 = new Point(6, 6);
    p2 = new Point(8, 8);
    l2 = new Line(p1, p2);
    Assert.True(l1.isColinear(l2));
    Assert.True(l2.isColinear(l1));
  }

  [Test]
  public void TestIsColinear_NRM3() {
    // 8x - 3y - 37 = 0
    p1 = new Point(0, 0);
    p2 = new Point(5, 5);
    l1 = new Line(p1, p2);
    p1 = new Point(6, 6);
    p2 = new Point(8.01M, 8);
    l2 = new Line(p1, p2);
    Assert.False(l1.isColinear(l2));
    Assert.False(l2.isColinear(l1));
  }

  [Test]
  public void TestIsColinear_NRM4() {
    l1 = new Line(5, -24, 1);
    l2 = new Line(-4, 19.2M, -0.8M);
    Assert.True(l1.isColinear(l2));
    Assert.True(l2.isColinear(l1));
  }

  [Test]
  public void TestIsColinear_NRM5() {
    l1 = new Line(1.1M, -2.2M, 3.3M);
    l2 = new Line(-6.7M, 13.4M, 4.4M);
    Assert.False(l1.isColinear(l2));
    Assert.False(l2.isColinear(l1));
  }

  [Test]
  public void TestIsPointConnected_NRM1() {
    p1 = new Point(4.5M, -2.6M);
    p2 = new Point(-6.1M, 5);
    l1 = new Line(p1, p2);
    p1 = new Point(-6.1M, 5);
    p2 = new Point(3, 2);
    l2 = new Line(p1, p2);
    Assert.True(l1.isPointConnected(l2));
    Assert.True(l2.isPointConnected(l1));
  }

  [Test]
  public void TestIsPointConnected_NRM2() {
    p1 = new Point(1.34M, 2.4M);
    p2 = new Point(-6.1M, 5);
    l1 = new Line(p1, p2);
    p1 = new Point(1.34M, 2.4M);
    p2 = new Point(4.5M, -2.6M);
    l2 = new Line(p1, p2);
    Assert.True(l1.isPointConnected(l2));
    Assert.True(l2.isPointConnected(l1));
  }

  [Test]
  public void TestIsPointConnected_NRM3() {
    p1 = new Point(10.2M, 25M);
    p2 = new Point(5.1M, -6.1M);
    l1 = new Line(p1, p2);
    p1 = new Point(9.3M, 8.9M);
    p2 = new Point(10.2M, 25M);
    l2 = new Line(p1, p2);
    Assert.True(l1.isPointConnected(l2));
    Assert.True(l2.isPointConnected(l1));
  }


  [Test]
  public void TestIsPointConnected_NRM4() {
    p1 = new Point(4.5M, -2.6M);
    p2 = new Point(-6.1M, 5);
    l1 = new Line(p1, p2);
    p1 = new Point(-6.1M, 5);
    p2 = new Point(4.5M, -2.6M);
    l2 = new Line(p1, p2);
    Assert.True(l1.isPointConnected(l2));
    Assert.True(l2.isPointConnected(l1));
  }

  [Test]
  public void TestIsPointConnected_NRM5() {
    p1 = new Point(1, 2);
    p2 = new Point(3, 4);
    l1 = new Line(p1, p2);
    p1 = new Point(1, 3);
    p2 = new Point(2, 4);
    l2 = new Line(p1, p2);
    Assert.False(l1.isPointConnected(l2));
    Assert.False(l2.isPointConnected(l1));
  }

  [Test]
  public void TestIsPointConnected_NRM6() {
    p1 = new Point(0, 0);
    p2 = new Point(3, 3);
    l1 = new Line(p1, p2);
    p1 = new Point(3.1M, 3.1M);
    p2 = new Point(4, 4);
    l2 = new Line(p1, p2);
    Assert.False(l1.isPointConnected(l2));
    Assert.False(l2.isPointConnected(l1));
  }

  [Test]
  public void TestIsPointConnected_ERR1() {
    p1 = new Point(0, 0);
    p2 = new Point(3, 3);
    l1 = new Line(p1, p2);
    l2 = new Line(-4, 19.2M, -0.8M);
    int iPassed = 0;
    try {
      l1.isPointConnected(l2);
    }
    catch (GeometryException e) {
      if (e.getReason() == GeometryExceptionType.LINE_NO_ENDPOINTS) {
        iPassed++;
      }
    }
    try {
      l2.isPointConnected(l1);
    }
    catch (GeometryException e) {
      if (e.getReason() == GeometryExceptionType.LINE_NO_ENDPOINTS) {
        iPassed++;
      }
    }
    if (iPassed == 2) {
      Assert.Pass();
    }
    Assert.Fail();
  }

  [Test]
  public void TestIsPointConnected_ERR2() {
    l1 = new Line(1, 2, 3);
    l2 = new Line(4, 5, 6);
    int iPassed = 0;
    try {
      l1.isPointConnected(l2);
    }
    catch (GeometryException e) {
      if (e.getReason() == GeometryExceptionType.LINE_NO_ENDPOINTS) {
        iPassed++;
      }
    }
    try {
      l2.isPointConnected(l1);
    }
    catch (GeometryException e) {
      if (e.getReason() == GeometryExceptionType.LINE_NO_ENDPOINTS) {
        iPassed++;
      }
    }
    if (iPassed == 2) {
      Assert.Pass();
    }
    Assert.Fail();
  }

  [Test]
  public void TestGetSlope_NRM1() {
    p1 = new Point(1.23M, -4.56M);
    p2 = new Point(7.89M, -0.12M);
    l1 = new Line(p1, p2);
    decimal slope = (-0.12M + 4.56M) / (7.89M - 1.23M);
    Assert.That(l1.getSlope(), Is.EqualTo(slope));
  }

  [Test]
  public void TestGetSlope_NRM2() {
    l1 = new Line(1.23M, -4.56M, 7.89M);
    decimal slope = 1.23M / 4.56M;
    Assert.That(l1.getSlope(), Is.EqualTo(slope));
  }

  [Test]
  public void TestCheckPoint_NRM1() {
    l1 = new Line(1.23M, -4.56M, 7.89M);
    // Line formula return y with given x
    var y = (decimal x) => (decimal)(((1.23M * x) + 7.89M) / 4.56M);
    for (decimal x = -100; x <= 100; x += 0.1M) {
      p1 = new Point(x, y(x));
      Assert.True(l1.checkPoint(p1));
    }
  }

  [Test]
  public void TestCheckPoint_NRM2() {
    p1 = new Point(1.23M, -4.56M);
    p2 = new Point(7.89M, -0.12M);
    l1 = new Line(p1, p2);
    var y = (decimal x) => (decimal)(((4.44M * x) - 35.8308M) / 6.66M);
    bool result;
    for (decimal x = -100; x <= 100; x += 0.1M) {
      p1 = new Point(x, y(x));
      result = l1.checkPoint(p1);
      if (x >= 1.23M && x <= 7.89M) {
        Assert.True(result);
      }
      else {
        Assert.False(result);
      }
    }
  }

  [Test]
  public void TestGetIntersectPoint_NRM1() {
    p1 = new Point(0, 0);
    p2 = new Point(1, 1);
    l1 = new Line(p1, p2);
    p1 = new Point(1, 0);
    p2 = new Point(0, 1);
    l2 = new Line(p1, p2);

    p1 = new Point(0.5M, 0.5M);
    p2 = l1.getIntersectPoint(l2);
    Assert.True(p1.isEqual(p2));
    p2 = l2.getIntersectPoint(l1);
    Assert.True(p1.isEqual(p2));
  }

  [Test]
  public void TestGetIntersectPoint_NRM2() {
    p1 = new Point(1.23M, -4.56M);
    p2 = new Point(7.89M, -0.12M);
    l1 = new Line(p1, p2);
    p1 = new Point(-3.45M, -6.78M);
    p2 = new Point(-9.01M, -2.34M);
    l2 = new Line(p1, p2);

    p1 = new Point(-2.8357610474631751227495908347M, -7.2705073649754500818330605565M);
    p2 = l1.getIntersectPoint(l2);
    Assert.True(p1.isEqual(p2));
    p2 = l2.getIntersectPoint(l1);
    Assert.True(p1.isEqual(p2));
  }

  [Test]
  public void TestGetIntersectPoint_NRM3() {
    l1 = new Line(1.23M, -4.56M, 7.89M);
    l2 = new Line(-0.12M, 3.45M, 6.78M);

    p1 = new Point(-15.728512296079863647431214999M, -2.5122960798636474312149987826M);
    p2 = l1.getIntersectPoint(l2);
    Assert.True(p1.isEqual(p2));
    p2 = l2.getIntersectPoint(l1);
    Assert.True(p1.isEqual(p2));
  }

  [Test]
  public void TestGetIntersectType_NRM1() {
    // Not intersecting within line boundaries
    p1 = new Point(0, 0);
    p2 = new Point(1, 1);
    l1 = new Line(p1, p2);
    p1 = new Point(1, 2);
    p2 = new Point(2, 2);
    l2 = new Line(p1, p2);
    Assert.That(l1.getIntersectType(l2), Is.EqualTo(IntersectType.NONE));
    Assert.That(l2.getIntersectType(l1), Is.EqualTo(IntersectType.NONE));
  }

  [Test]
  public void TestGetIntersectType_NRM2() {
    // Parallel lines
    p1 = new Point(0, 0);
    p2 = new Point(1, 1);
    l1 = new Line(p1, p2);
    p1 = new Point(0, 1);
    p2 = new Point(1, 2);
    l2 = new Line(p1, p2);
    Assert.That(l1.getIntersectType(l2), Is.EqualTo(IntersectType.NONE));
    Assert.That(l2.getIntersectType(l1), Is.EqualTo(IntersectType.NONE));
  }

  [Test]
  public void TestGetIntersectType_NRM3() {
    // Point connected
    p1 = new Point(0, 0);
    p2 = new Point(1, 1);
    l1 = new Line(p1, p2);
    p1 = new Point(2, 0);
    p2 = new Point(1, 1);
    l2 = new Line(p1, p2);
    Assert.That(l1.getIntersectType(l2), Is.EqualTo(IntersectType.POINT_TO_POINT));
    Assert.That(l2.getIntersectType(l1), Is.EqualTo(IntersectType.POINT_TO_POINT));
  }

  [Test]
  public void TestGetIntersectType_NRM4() {
    // Point connected, colinear
    p1 = new Point(0, 0);
    p2 = new Point(1, 1);
    l1 = new Line(p1, p2);
    p1 = new Point(1, 1);
    p2 = new Point(2, 2);
    l2 = new Line(p1, p2);
    Assert.That(l1.getIntersectType(l2), Is.EqualTo(IntersectType.POINT_TO_POINT));
    Assert.That(l2.getIntersectType(l1), Is.EqualTo(IntersectType.POINT_TO_POINT));
  }

  [Test]
  public void TestGetIntersectType_NRM5() {
    // One endpoint touch other line
    p1 = new Point(0, 0);
    p2 = new Point(1, 1);
    l1 = new Line(p1, p2);
    p1 = new Point(0, 1);
    p2 = new Point(2, 1);
    l2 = new Line(p1, p2);
    Assert.That(l1.getIntersectType(l2), Is.EqualTo(IntersectType.POINT_TO_LINE));
    Assert.That(l2.getIntersectType(l1), Is.EqualTo(IntersectType.LINE_TO_POINT));
  }

  [Test]
  public void TestGetIntersectType_NRM6() {
    // One endpoint touch other line
    p1 = new Point(6, 3);
    p2 = new Point(2, 7);
    l1 = new Line(p1, p2);
    p1 = new Point(-5, 4);
    p2 = new Point(3, 6);
    l2 = new Line(p1, p2);
    Assert.That(l1.getIntersectType(l2), Is.EqualTo(IntersectType.LINE_TO_POINT));
    Assert.That(l2.getIntersectType(l1), Is.EqualTo(IntersectType.POINT_TO_LINE));
  }

  [Test]
  public void TestGetIntersectType_NRM7() {
    // Lines cross
    p1 = new Point(0, 0);
    p2 = new Point(1, 1);
    l1 = new Line(p1, p2);
    p1 = new Point(1, 0);
    p2 = new Point(0, 1);
    l2 = new Line(p1, p2);
    Assert.That(l1.getIntersectType(l2), Is.EqualTo(IntersectType.LINE_TO_LINE));
    Assert.That(l2.getIntersectType(l1), Is.EqualTo(IntersectType.LINE_TO_LINE));
  }

  [Test]
  public void TestGetIntersectType_NRM8() {
    // Lines cross
    p1 = new Point(3, -1);
    p2 = new Point(2, 7);
    l1 = new Line(p1, p2);
    p1 = new Point(-5, 4);
    p2 = new Point(3, 6);
    l2 = new Line(p1, p2);
    Assert.That(l1.getIntersectType(l2), Is.EqualTo(IntersectType.LINE_TO_LINE));
    Assert.That(l2.getIntersectType(l1), Is.EqualTo(IntersectType.LINE_TO_LINE));
  }

  [Test]
  public void TestGetIntersectType_NRM9() {
    // Lines cross
    p1 = new Point(0, 0);
    p2 = new Point(4, 4);
    l1 = new Line(p1, p2);
    p1 = new Point(5, 5);
    p2 = new Point(3, 3);
    l2 = new Line(p1, p2);
    Assert.That(l1.getIntersectType(l2), Is.EqualTo(IntersectType.OVERLAP));
    Assert.That(l2.getIntersectType(l1), Is.EqualTo(IntersectType.OVERLAP));
  }

  [Test]
  public void TestGetIntersectType_NRM10() {
    // Lines cross
    p1 = new Point(0, 5);
    p2 = new Point(5, 0);
    l1 = new Line(p1, p2);
    p1 = new Point(5, 0);
    p2 = new Point(3, 2);
    l2 = new Line(p1, p2);
    Assert.That(l1.getIntersectType(l2), Is.EqualTo(IntersectType.OVERLAP));
    Assert.That(l2.getIntersectType(l1), Is.EqualTo(IntersectType.OVERLAP));
  }

  [Test]
  public void TestGetIntersectType_NRM11() {
    // Lines cross
    p1 = new Point(-2, 5);
    p2 = new Point(-6, 3);
    l1 = new Line(p1, p2);
    p1 = new Point(-6, 3);
    p2 = new Point(-2, 5);
    l2 = new Line(p1, p2);
    Assert.That(l1.getIntersectType(l2), Is.EqualTo(IntersectType.EQUIVALENT));
    Assert.That(l2.getIntersectType(l1), Is.EqualTo(IntersectType.EQUIVALENT));
  }
}