namespace GestorPresupuestosAPI.Features.Utility
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public List<string>? Errors { get; set; }

        public ApiResponse(bool success, string message, object data = null, List<string> errors = null)
        {
            Success = success;
            Message = message;
            Data = data;
            Errors = errors ?? new List<string>();
        }

        public static ApiResponse Ok(string message, object data = null)
        {
            return new ApiResponse(true, message, data);
        }

        public static ApiResponse BadRequest(string message, List<string> errors = null)
        {
            return new ApiResponse(false, message, null, errors);
        }

        public static ApiResponse NotFound(string message)
        {
            return new ApiResponse(false, message);
        }

        public static ApiResponse Unauthorized(string message)
        {
            return new ApiResponse(false, message);
        }
    }
}
