﻿using System;


namespace Xyperico.Base.Exceptions
{
  public class VersionConflictException : Exception
  {
    public string Id { get; private set; }

    public Type EntityType { get; private set; }

    public long Version { get; private set; }


    public VersionConflictException(string id, Type entityType, int version)
      : this(id, entityType, (long)version)
    {
    }


    public VersionConflictException(string id, Type entityType, long version)
      : base(string.Format("Could not update ID {0}, version {1}, of type {2} since a newer version already exists.",
                           id, version, entityType))
    {
      Id = id;
      EntityType = entityType;
      Version = version;
    }
  }
}
