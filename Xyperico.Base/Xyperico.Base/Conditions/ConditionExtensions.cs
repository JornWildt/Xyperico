using CuttingEdge.Conditions;
using Xyperico.Base.CommonDomainTypes;


namespace Xyperico.Base.Conditions
{
  public static class ConditionExtensions
  {
    public static ConditionValidator<string> IsValidEMail(this ConditionValidator<string> validator)
    {
      EMail.ValidateEMail(validator.Value, validator.ArgumentName);
      return validator;
    }
  }
}
