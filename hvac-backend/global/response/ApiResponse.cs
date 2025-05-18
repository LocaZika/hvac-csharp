namespace hvac_backend.global.response;

public class ApiResponse {
  // Properties
  public short StatusCode { get; set; }
  public bool Ok => StatusCode >= 200 && StatusCode < 300;
  public List<string>? Messages { get; set; }
  public string? AccessToken { get; set; }
  public ApiResponse(short statusCode) {
    StatusCode = statusCode;
  }
  public ApiResponse(short statusCode, List<string> messages) {
    StatusCode = statusCode;
    Messages = messages;
  }
  public static ApiResponse Success(short statusCode, List<string> messages) => new(statusCode, messages);
  public static ApiResponse Removed() => new(200);
  public static ApiResponse Failed(short statusCode, List<string> messages) => new(statusCode, messages);
}

public class ApiResponse<T> : ApiResponse {
  // Properties
  public T? Data { get; set; }
  // Constructors
  public ApiResponse(short statusCode, T data) : base(statusCode) {
    StatusCode = statusCode;
    if (data != null) {
      Data = data;
    }
  }

  // Static Methods
  public static ApiResponse<T> Success(T data) => new(200, data);
  public static ApiResponse<T> Created(T data) => new(201, data);
  public static ApiResponse<T> Removed(T data) => new(200, data);
  public static ApiResponse<T> Failed(short statusCode, T data) => new(statusCode, data);
}
