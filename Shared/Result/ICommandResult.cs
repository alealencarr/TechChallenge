using System.Text.Json.Serialization;

namespace Shared.Result
{
    public interface ICommandResult
    {
        List<string> Messages { get; set; }

        [JsonIgnore]
        bool Succeeded { get; set; }

        [JsonIgnore]
        bool Conflict { get; set; }
    }

    public interface ICommandResult<T> : ICommandResult
    {
        T Data { get; set; }
    }
}
