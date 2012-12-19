using System.Collections.Generic;


namespace Xyperico.Base.Validation
{
  public interface IValidate
  {
    bool IsValid { get; }
    IEnumerable<ValidationError> Errors { get; }
  }
}
