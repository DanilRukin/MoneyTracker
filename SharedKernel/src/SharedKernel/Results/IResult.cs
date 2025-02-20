namespace SharedKernel.Results
{
    public interface IResult
    {
        bool IsSuccess { get; }
        ResultStatus ResultStatus { get; }
        Type ValueType { get; }
        object GetValue();
        IEnumerable<string> Errors { get; }
    }
}
