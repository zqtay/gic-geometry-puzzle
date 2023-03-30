using System;
using Geometry;

namespace GICGeometryPuzzle {

  public class Program {
    public static void Main() {
      var l1 = new Line(new Point(0, 0), new Point(1, 1));
      var l2 = new Line(new Point(1, 0), new Point(0, 1));
      //var l2 = new Line(0, 1, -0.5);
      Console.WriteLine($"{l1.a}x + {l1.b}y + {l1.c} = 0");
      Console.WriteLine($"{l2.a}x + {l2.b}y + {l2.c} = 0");
      var t = Line.areIntersect(l1, l2);
      Console.WriteLine($"isIntersect {t}");
    }
  }
}


