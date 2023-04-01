using Geometry;

namespace UnitTests;

public class PointTests {
  Point p1;
  Point p2;
  double x;
  double y;

  [SetUp]
  public void Setup() {
  }

  [Test]
  public void TestConstructor_NRM1() {
    x = 10;
    y = 20;
    p1 = new Point(x, y);
    Assert.AreEqual(x, p1.x);
    Assert.AreEqual(y, p1.y);
  }

  [Test]
  public void TestConstructor_NRM2() {
    x = -10;
    y = -20;
    p1 = new Point(x, y);
    Assert.AreEqual(x, p1.x);
    Assert.AreEqual(y, p1.y);
  }

  [Test]
  public void TestConstructor_NRM3() {
    x = 3.142;
    y = 2.718;
    p1 = new Point(x, y);
    Assert.AreEqual(x, p1.x);
    Assert.AreEqual(y, p1.y);
  }

  [Test]
  public void TestConstructor_NRM4() {
    x = Int32.Parse("123");
    y = Int32.Parse("-456");
    p1 = new Point(x, y);
    Assert.AreEqual(x, p1.x);
    Assert.AreEqual(y, p1.y);
  }

  [Test]
  public void TestConstructor_NRM5() {
    x = y = -3.142;
    Object obj = new Point(x, y);
    p1 = (Point)obj;
    Assert.AreEqual(x, p1.x);
    Assert.AreEqual(y, p1.y);
  }

  [Test]
  public void TestIsEqual_NRM1() {
    p1 = new Point(1, 2);
    p2 = new Point(1, 2);
    Assert.True(p1.isEqual(p2));
    Assert.True(p2.isEqual(p1));
  }

  [Test]
  public void TestIsEqual_NRM2() {
    p1 = new Point(-20, -50);
    p2 = new Point(-20, -50);
    Assert.True(p1.isEqual(p2));
    Assert.True(p2.isEqual(p1));
  }

  [Test]
  public void TestIsEqual_NRM3() {
    p1 = new Point(1.23, -4.56);
    p2 = new Point(1.23, -4.56);
    Assert.True(p1.isEqual(p2));
    Assert.True(p2.isEqual(p1));
  }

  [Test]
  public void TestIsEqual_NRM4() {
    p1 = new Point(1.23, -4.56);
    p2 = new Point(-1.23, -4.56);
    Assert.False(p1.isEqual(p2));
    Assert.False(p2.isEqual(p1));
  }

  [Test]
  public void TestIsEqual_ERR1() {
    p1 = new Point(0, 0);
    p2 = null;
    try {
      Assert.False(p1.isEqual(p2));
    }
    catch (Exception e) {
      if (e is NullReferenceException) {
        Assert.Pass();
      }
    }
    Assert.Fail();
  }

  [Test]
  public void TestToString_NRM1() {
    x = 0;
    y = 0;
    p1 = new Point(x, y);
    Assert.AreEqual( $"({x},{y})", p1.toString());
  }

  [Test]
  public void TestToString_NRM2() {
    x = -30;
    y = -40;
    p1 = new Point(x, y);
    Assert.AreEqual( $"({x},{y})", p1.toString());
  }

  [Test]
  public void TestToString_NRM3() {
    x = -3.142;
    y = 2.718;
    p1 = new Point(x, y);
    Assert.AreEqual($"({x},{y})", p1.toString());
  }
}