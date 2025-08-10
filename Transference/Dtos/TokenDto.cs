using Newtonsoft.Json;

namespace Transference.Dtos
{
    public class TokenDto
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("data")]
        public string Token { get; set; }

        [JsonProperty("totalRecords")]
        public int? TotalRecords { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("errors")]
        public string Errors { get; set; }
    }
}
