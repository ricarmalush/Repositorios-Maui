using Microsoft.IdentityModel.Tokens;

namespace Transference.Resources.Bases.Response
{
    public class BaseResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public int TotalRecords { get; set; }
        public string? Message { get; set; }
        public IEnumerable<ValidationFailure>? Errors { get; set; }
    }

    public class OvertimeDetails
    {
        public string Property1 { get; set; }
        public int Property2 { get; set; }
    }


}
