namespace Xyperico.Agres.Configuration
{
  public static class Configure
  {
    /// <summary>
    /// Begin configuration section.
    /// </summary>
    /// <returns></returns>
    public static LoggingConfiguration With()
    {
      return new LoggingConfiguration();
    }
  }
}
