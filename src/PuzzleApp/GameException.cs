namespace PuzzleApp {

  /// <summary>
  /// Custom exception for Geometry
  /// </summary>
  public class GameException : System.Exception {
    private GameExceptionType reason;
    public GameException(GameExceptionType reason) {
      this.reason = reason;
    }
    public static void throwIt(GameExceptionType reason) {
      throw new GameException(reason);
    }
    public GameExceptionType getReason() {
      return this.reason;
    }
  }

  public enum GameExceptionType : ushort {
    MODE_INVALID,
    STATE_INVALID,
    INPUT_INVALID,
    OPERATION_INVALID
  }
}