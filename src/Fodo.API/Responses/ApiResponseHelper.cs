namespace Fodo.API.Responses
{
    public static class ApiResponseHelper
    {
        public static ApiResponse<T> Success<T>(T body)
            => new ApiResponse<T>(200, body);

        public static ApiResponse<T> Fail<T>(int code, T body)
            => new ApiResponse<T>(code, body);
    }
}
