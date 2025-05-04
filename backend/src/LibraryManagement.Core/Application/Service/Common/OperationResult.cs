public class OperationResult
{
    public bool Success { get; private set; }
    public string? Message { get; private set; }
    public object? Data { get; private set; }

    private OperationResult(bool success, object? data = null, string? message = null)
    {
        Success = success;
        Data = data;
        Message = message;
    }

    // No-data success
    public static OperationResult Ok(string? message = null) =>
        new OperationResult(true, null, message);

    // With-data success
    public static OperationResult Ok(object data, string? message = null) =>
        new OperationResult(true, data, message);

    // Failure (no data ever)
    public static OperationResult Fail(string message) => new OperationResult(false, null, message);
}
