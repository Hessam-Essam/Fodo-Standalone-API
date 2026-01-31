namespace Fodo.API.Responses
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public T Body { get; set; }

        public ApiResponse(int code, T body)
        {
            Code = code;
            Body = body;
        }
    }
}
