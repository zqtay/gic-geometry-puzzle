using System;
using Geometry;

namespace GICGeometryPuzzle {

  public class Program {
    public static void Main() {
      Game game = new Game();
      game.start();
    }

    public static void test() {
      var l1 = new Line(new Point(0, 0), new Point(1, 1));
      var l2 = new Line(new Point(1, 0), new Point(0, 1));
      //var l2 = new Line(0, 1, -0.5);
      Console.WriteLine($"{l1.a}x + {l1.b}y + {l1.c} = 0");
      Console.WriteLine($"{l2.a}x + {l2.b}y + {l2.c} = 0");
      var t = Line.areIntersect(l1, l2);
      Console.WriteLine($"isIntersect {t}");
    }
  }

  public class Game {
    private Shape shape;

    public Game() {
      this.shape = null;
    }

    public void start() {
      this.shape = new Shape();
      String mode = intro();
      if (mode == "1") {
        useCustomShape();
      }
    }

    public String intro() {
      Console.WriteLine("Welcome to the GIC geometry puzzle app");
      Console.WriteLine("[1] Create a custom shape");
      Console.WriteLine("[2] Generate a random shape");
      return Console.ReadLine();
    }

    public void useCustomShape() {
      int i = 1;
      String[] input;
      Point p;
      int iVertex;
      // Maximum 8 points
      while (i <= 8) {
        // Minimum 3 points required
        if (i > 1) {
          if (i <= 3) {
            Console.WriteLine("Your current shape is incomplete");
          } else {
            Console.WriteLine("Your current shape is valid and complete");
          }
          // Print all current vertices
          for (iVertex = 0; iVertex < shape.vertices.Count; iVertex++) {
            Console.WriteLine($"{iVertex + 1}:{shape.vertices[iVertex].toString()}");
          }
        }
        if (i <= 3) {
          Console.WriteLine($"Please enter coordinates {i} in x y format");
        } else {
          Console.WriteLine($"Please enter # to finalize your shape or enter coordinates {i} in x y format");
        }
        // Read user input
        input = Console.ReadLine().Split(" ");
        if (input[0] == "#") {
          // Finalize shape
          Console.WriteLine("Your finalized shape is");
          for (iVertex = 0; iVertex < shape.vertices.Count; iVertex++) {
            Console.WriteLine($"{iVertex + 1}:{shape.vertices[iVertex].toString()}");
          }
          break;
        } else {
          // Process adding point
          p = new Point(Convert.ToDouble(input[0]), Convert.ToDouble(input[1]));
          try {
            shape.addPoint(p);
            i++;
          } catch (Exception e) {
            Console.WriteLine($"New coordinates{p.toString()} is invalid!!!");
            Console.WriteLine($"Not adding new coordinates to the current shape.");
          }
        }
      }
    }
  }
}


