namespace Xyperico.Base.Validation
{
  public class PropertyRequirement
  {
    private string IsRequiredMessage = "{0} is required";
    private string HasMaxLengthMessage = "{0} is too long (max. {1} characters allowed)";

    public string PropertyName { get; private set; }

    public Validator Validator { get; private set; }

    public object Value { get; private set; }


    public PropertyRequirement(string propertyName, object value, Validator validator)
    {
      PropertyName = propertyName;
      Value = value;
      Validator = validator;
    }


    public PropertyRequirement IsNotNullOrEmpty()
    {
      if (string.IsNullOrEmpty((string)Value))
        Validator.AddError(PropertyName, string.Format(IsRequiredMessage, PropertyName));
      return this;
    }


    public PropertyRequirement HasMaxLength(int maxLength)
    {
      if (Value == null || ((string)Value).Length > maxLength)
        Validator.AddError(PropertyName, string.Format(HasMaxLengthMessage, PropertyName, maxLength));
      return this;
    }
  }
}
