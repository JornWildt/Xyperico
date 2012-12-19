using System.Collections.Generic;


namespace Xyperico.Base.Validation
{
  public class Validator
  {
    private List<ValidationError> ErrorList = new List<ValidationError>();


    public void Clear()
    {
      ErrorList.Clear();
    }


    public void AddError(string propertyName, string message)
    {
      ErrorList.Add(new ValidationError(propertyName, message));
    }


    public bool IsValid
    {
      get { return ErrorList.Count == 0; }
    }


    public IEnumerable<ValidationError> Errors
    {
      get { return ErrorList; }
    }


    #region Helpers

    public PropertyRequirement Require(string propertyName, object value)
    {
      return new PropertyRequirement(propertyName, value, this);
    }

    #endregion
  }
}
