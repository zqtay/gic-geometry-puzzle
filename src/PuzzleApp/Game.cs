using System;
using System.Collections.Generic;
using Geometry;

namespace PuzzleApp {
  public class Game {
    private enum GameState : ushort {
      INTRO = 0,
      CUSTOM_SHAPE,
      RANDOM_SHAPE,
      PUZZLE,
      QUIT
    };

    private Shape shape;
    private GameState state = GameState.INTRO;

    public Game() {
      this.shape = null;
    }

    private void printShapeVertices() {
      if (this.shape == null) {
        // Shape not initialized
        throw new GameException(GameExceptionType.OPERATION_INVALID);
      }
      // index for looping through vertices
      int iVertex;
      // Reference for vertices of the shape
      List<Point> vertices = this.shape.getVertices();
      // Print each vertex
      for (iVertex = 0; iVertex < vertices.Count; iVertex++) {
        Console.WriteLine($"{iVertex + 1}:{vertices[iVertex].toString()}");
      }
    }

    private void printInvalidInputMessage(String input) {
      Console.WriteLine($"Sorry, \"{input}\" is not a valid input\n");
    }

    public void start() {
      state = GameState.INTRO;
      while (true) {
        switch (state) {
          case GameState.INTRO:
            state = intro();
            break;
          case GameState.CUSTOM_SHAPE:
            state = createCustomShape();
            break;
          case GameState.RANDOM_SHAPE:
            state = generateRandomShape();
            break;
          case GameState.PUZZLE:
            state = playPuzzle();
            break;
          case GameState.QUIT:
            state = quit();
            return;
          default:
            // Should not reach here
            throw new GameException(GameExceptionType.STATE_INVALID);
        }
      }
    }

    private GameState intro() {
      Console.Clear();
      while (true) {
        Console.WriteLine("Welcome to the GIC geometry puzzle app");
        Console.WriteLine("[1] Create a custom shape");
        Console.WriteLine("[2] Generate a random shape");
        String mode = Console.ReadLine();
        Console.Clear();
        if (mode == "1") return GameState.CUSTOM_SHAPE;
        else if (mode == "2") return GameState.RANDOM_SHAPE;
        else printInvalidInputMessage(mode);
      }

      // Should not reach here
      throw new GameException(GameExceptionType.OPERATION_INVALID);
    }

    private GameState createCustomShape() {
      // Loop index
      int i = 1;
      // Buffer for input string
      String[] input;
      // Point reference to be added to shape
      Point p;
      // Is current shape valid to be finalized
      bool zValid;

      // Create new shape
      this.shape = new Shape();

      while (true) {
        // Check validity at the top of the loop
        zValid = this.shape.isShapeValidFinal();
        // Minimum 3 points required
        if (i > 1) {
          Console.Write("Your current shape is ");
          // Print if shape is valid and complete
          if (i <= Shape.VERTICES_MIN) {
             Console.WriteLine("incomplete");
          }
          else if (zValid != true) {
            Console.WriteLine("invalid");
          }
          else {
            Console.WriteLine("valid and complete");
          }
          // Print all current vertices
          printShapeVertices();
        }

        // Print instructions
        if (i <= 3) Console.WriteLine($"Please enter coordinates {i} in x y format");
        else Console.WriteLine($"Please enter # to finalize your shape or enter coordinates {i} in x y format");

        // Read user input
        input = Console.ReadLine().Split(" ");
        // Clear console after input
        Console.Clear();

        if (input[0] == "#") {
          try {
            // Finalize shape
            this.shape.finalize();
          }
          catch (GeometryException e) {
            Console.WriteLine($"Shape not able to be finalized.\n");
          }
          // Finalize successful
          Console.WriteLine("Your finalized shape is");
          printShapeVertices();
          // Blank line
          Console.WriteLine();
          // Break loop and go to puzzle
          return GameState.PUZZLE;
        }
        else {
          // Process adding point
          decimal x, y;
          if (Decimal.TryParse(input[0], out x) && Decimal.TryParse(input[1], out y)) {
            p = new Point(x, y);
            try {
              shape.addVertex(p);
              i++;
            }
            catch (GeometryException e) {
              // Invalid point
              if (e.getReason() == GeometryExceptionType.POINT_INVALID) {
                Console.WriteLine($"New coordinates {p.toString()} is invalid!!!");
                Console.WriteLine($"Not adding new coordinates to the current shape.\n");
              }
              else {
                throw new GameException(GameExceptionType.OPERATION_INVALID);
              }
            }
          }
          else {
            printInvalidInputMessage(String.Join(" ", input));
          }
        }
      }

      // Should not reach here
      throw new GameException(GameExceptionType.OPERATION_INVALID);
    }

    private GameState generateRandomShape() {
      // Generate random shape shape
      this.shape = Shape.genRandom(RANDOM_SHAPE_VERTICES_MAX);

      // Display vertices
      Console.WriteLine("Your random shape is");
      printShapeVertices();
      // Blank line
      Console.WriteLine();

      // Go to puzzle
      return GameState.PUZZLE;
    }

    private GameState playPuzzle() {
      decimal x, y;
      String[] input;
      Point p;
      bool result;

      while (true) {
        Console.WriteLine("Please key in test coordinates in x y format or enter # to quit the game");
        input = Console.ReadLine().Split(" ");
        if (input[0] == "#") {
          return GameState.QUIT;
        }
        else if (Decimal.TryParse(input[0], out x) && Decimal.TryParse(input[1], out y)) {
          // Create new point
          p = new Point(x, y);
          // Check if the point is inside the shape
          result = this.shape.isPointInside(p);
          // Clear console
          Console.Clear();
          Console.WriteLine("Your finalized shape is");
          printShapeVertices();
          // Blank line
          Console.WriteLine();
          // Print result
          if (result) {
            Console.WriteLine($"Coordinates {p.toString()} is within your finalized shape");
          }
          else {
            Console.WriteLine($"Sorry, coordinates {p.toString()} is outside of your finalized shape");
          }
        }
        else {
          printInvalidInputMessage(String.Join(" ", input));
        }
      }

      // Should not reach here
      throw new GameException(GameExceptionType.OPERATION_INVALID);
    }

    private GameState quit() {
      Console.Clear();
      Console.WriteLine("Thank you for playing the GIC geometry puzzle app");
      Console.WriteLine("Have a nice day!");
      return GameState.INTRO;
    }
  }
}