using System.Collections.Generic;


namespace Xyperico.Agres.Configuration
{
  public class BaseConfiguration : AbstractConfiguration
  {
    public static LoggingConfiguration Configure()
    {
      return new LoggingConfiguration();
    }


    public BaseConfiguration(AbstractConfiguration cfg)
      : base(cfg)
    {
    }
  }
}
