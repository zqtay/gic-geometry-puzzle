using Geometry;

namespace UnitTests;

public class PointTests {
  Point? p1;
  Point? p2;
  decimal x;
  decimal y;

  [SetUp]
  public void Setup() {
  }

  [TearDown]
  public void TearDown() {
    p1 = null;
    p2 = null;
    x = 0;
    y = 0;
  }

  [Test]
  public void TestConstructor_NRM1() {
    x = 10;
    y = 20;
    p1 = new Point(x, y);
    Assert.That(p1.x, Is.EqualTo(x));
    Assert.That(p1.y, Is.EqualTo(y));
  }

  [Test]
  public void TestConstructor_NRM2() {
    x = -10;
    y = -20;
    p1 = new Point(x, y);
    Assert.That(p1.x, Is.EqualTo(x));
    Assert.That(p1.y, Is.EqualTo(y));
  }

  [Test]
  public void TestConstructor_NRM3() {
    x = 3.142M;
    y = 2.718M;
    p1 = new Point(x, y);
    Assert.That(p1.x, Is.EqualTo(x));
    Assert.That(p1.y, Is.EqualTo(y));
  }

  [Test]
  public void TestConstructor_NRM4() {
    x = Int32.Parse("123");
    y = Int32.Parse("-456");
    p1 = new Point(x, y);
    Assert.That(p1.x, Is.EqualTo(x));
    Assert.That(p1.y, Is.EqualTo(y));
  }

  [Test]
  public void TestConstructor_NRM5() {
    x = y = -3.142M;
    Object obj = new Point(x, y);
    p1 = (Point)obj;
    Assert.That(p1.x, Is.EqualTo(x));
    Assert.That(p1.y, Is.EqualTo(y));
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
    p1 = new Point(1.23M, -4.56M);
    p2 = new Point(1.23M, -4.56M);
    Assert.True(p1.isEqual(p2));
    Assert.True(p2.isEqual(p1));
  }

  [Test]
  public void TestIsEqual_NRM4() {
    p1 = new Point(1.23M, -4.56M);
    p2 = new Point(-1.23M, -4.56M);
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
    Assert.That(p1.toString(), Is.EqualTo($"({x},{y})"));
  }

  [Test]
  public void TestToString_NRM2() {
    x = -30;
    y = -40;
    p1 = new Point(x, y);
    Assert.That(p1.toString(), Is.EqualTo($"({x},{y})"));
  }

  [Test]
  public void TestToString_NRM3() {
    x = -3.142M;
    y = 2.718M;
    p1 = new Point(x, y);
    Assert.That(p1.toString(), Is.EqualTo($"({x},{y})"));
  }
}