using System;


namespace Xyperico.Agres.EventStore
{
  public class VersionConflictException : Exception
  {
    public VersionConflictException(long expectedVersion, long actualVersion, string key)
      : base(string.Format("Version mismatch for '{0}'. Expected {1} but got {2}.", key, expectedVersion, actualVersion))
    {
    }

    public VersionConflictException(long expectedVersion, string key)
      : base(string.Format("Version mismatch for '{0}'. Could not insert {1}.", key, expectedVersion))
    {
    }
  }
}
