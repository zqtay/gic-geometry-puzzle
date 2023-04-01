namespace Geometry {

  /// <summary>
  /// Custom exception for Geometry
  /// </summary>
  public class GeometryException : System.Exception {
    private GeometryExceptionType reason;
    public GeometryException(GeometryExceptionType reason) {
      this.reason = reason;
    }
    public static void throwIt(GeometryExceptionType reason) {
      throw new GeometryException(reason);
    }
    public GeometryExceptionType getReason() {
      return this.reason;
    }
  }

  public enum GeometryExceptionType : ushort {
    POINT_INVALID = 1,
    LINE_SAME_ENDPOINTS,
    LINE_NO_ENDPOINTS,
    LINE_PARALLEL,
    LINE_INVALID,
    INTERSECT_INVALID,
    OPERATION_INVALID
  }
}