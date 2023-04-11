using System;
using System.Collections.Generic;
using Geometry;

namespace PuzzleApp {
  /// <summary>
  /// Class for running and managing game session
  /// </summary>
  public class Game {
    /// <summary>
    /// Maximum number of vertices allowed for randomly generated shape
    /// </summary>
    private const int RANDOM_SHAPE_VERTICES_MAX = 8;

    /// <summary>
    /// Game states
    /// </summary>
    private enum GameState : ushort {
      INTRO = 0, // Start of game, choose mode
      CUSTOM_SHAPE, // Step for creating a custom shape
      RANDOM_SHAPE, // Step for generating a random shape
      PUZZLE, // Puzzle playing step
      QUIT // Quit game
    };

    /// <summary>
    /// Instance of Shape to be interacted with during game session
    /// </summary>
    private Shape shape;
    /// <summary>
    /// Indicates current game state
    /// </summary>
    private GameState state = GameState.INTRO;

    /// <summary>
    /// Constructs a new game instance
    /// </summary>
    public Game() {
      this.state = GameState.INTRO;
      this.shape = null;
    }

    /// <summary>
    /// Helper function to print current shape vertices
    /// </summary>
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

    /// <summary>
    /// Helper function to print invalid input message
    /// </summary>
    /// <param name="input"></param>
    private void printInvalidInputMessage(String input) {
      Console.WriteLine($"Sorry, \"{input}\" is not a valid input");
    }

    /// <summary>
    /// Main method. This starts the game session in console. <br />
    /// Corresponding game internal method is called based on current game state. <br />
    /// This method exits if GameState.QUIT is reached.
    /// </summary>
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
            quit();
            return;
          default:
            // Should not reach here
            throw new GameException(GameExceptionType.STATE_INVALID);
        }
      }
    }

    /// <summary>
    /// Intro screen of the game. <br />
    /// Player chooses shape creation mode here. <br />
    /// </summary>
    /// <returns>
    /// <list>
    /// <item>GameState.CUSTOM_SHAPE if input == "1"</item>
    /// <item>GameState.RANDOM_SHAPE if input == "2"</item>
    /// </list>
    /// </returns>
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

    /// <summary>
    /// Step for creating a custom shape. <br />
    /// Player inputs coordinates to form a shape and each of them
    /// is validated and added to shape one by one. <br />
    /// Minimum 3 coordinates (vertices) must be added to the shape
    /// before a shape can be finalized. <br />
    /// </summary>
    /// <returns>GameState.PUZZLE if shape is finalized</returns>
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
        if (i <= 3 || !zValid) {
          Console.WriteLine($"Please enter coordinates {i} in x y format");
        }
        else {
          Console.WriteLine($"Please enter # to finalize your shape or enter coordinates {i} in x y format");
        }

        // Read user input
        input = Console.ReadLine().Split(" ");
        // Clear console after input
        Console.Clear();

        if (input[0] == "#" && zValid) {
          try {
            // Finalize shape
            this.shape.finalize();
          }
          catch (GeometryException e) {
            Console.WriteLine($"Shape not able to be finalized.\n");
            continue;
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
          if (input.Length == 2 && Decimal.TryParse(input[0], out x) && Decimal.TryParse(input[1], out y)) {
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
            // Blank line
            Console.WriteLine();
          }
        }
      }

      // Should not reach here
      throw new GameException(GameExceptionType.OPERATION_INVALID);
    }

    /// <summary>
    /// Step for creating a random shape. <br />
    /// Shape with number of vertices between 3 to 8 will be
    /// generated. <br />
    /// </summary>
    /// <returns>GameState.PUZZLE if shape is finalized</returns>
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

    /// <summary>
    /// Step for playing the puzzle. <br />
    /// Player inputs a test coordinates and the game checks if
    /// it is inside or outside the created shape. <br />
    /// The game will loop endlessly until player inputs # to
    /// quit the game. <br />
    /// </summary>
    /// <returns>GameState.QUIT when player inputs #</returns>
    private GameState playPuzzle() {
      decimal x, y;
      String[] input;
      Point p;
      bool result;

      while (true) {
        Console.WriteLine("Please key in test coordinates in x y format or enter # to quit the game");
        // Read user input
        input = Console.ReadLine().Split(" ");
        // Clear console after input
        Console.Clear();

        if (input[0] == "#") {
          return GameState.QUIT;
        }
        else {
          Console.WriteLine("Your finalized shape is");
          printShapeVertices();
          // Blank line
          Console.WriteLine();
          if (input.Length == 2 && Decimal.TryParse(input[0], out x) && Decimal.TryParse(input[1], out y)) {
            // Create new point
            p = new Point(x, y);
            // Check if the point is inside the shape
            result = this.shape.isPointInside(p);
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
      }

      // Should not reach here
      throw new GameException(GameExceptionType.OPERATION_INVALID);
    }

    /// <summary>
    /// Step for quitting the game. <br />
    /// Final messages are printed.
    /// </summary>
    private void quit() {
      Console.WriteLine("Thank you for playing the GIC geometry puzzle app");
      Console.WriteLine("Have a nice day!");
    }
  }
}