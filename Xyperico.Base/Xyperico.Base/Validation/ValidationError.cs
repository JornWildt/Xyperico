namespace Xyperico.Base.Validation
{
  public class ValidationError
  {
    public string PropertyName { get; private set; }
    public string Message { get; private set; }

    public ValidationError(string propertyName, string message)
    {
      PropertyName = propertyName;
      Message = message;
    }
  }
}
