using System;
using CuttingEdge.Conditions;
using log4net;
using Xyperico.Base;


namespace Xyperico.Agres.Configuration
{
  public static class ObjectContainerConfigurationExtensions
  {
    private static readonly ILog Logger = LogManager.GetLogger(typeof(ObjectContainerConfigurationExtensions));

    private const string ObjectContainer_SettingsKey = "BaseConfiguration_ObjectContainer";


    /// <summary>
    /// Configure which object container to use for dependency injection.
    /// </summary>
    /// <param name="cfg"></param>
    /// <param name="container"></param>
    /// <returns></returns>
    public static BaseConfiguration ObjectContainer(this ObjectContainerConfiguration cfg, IObjectContainer container)
    {
      Condition.Requires(cfg, "cfg").IsNotNull();
      Condition.Requires(container, "container").IsNotNull();

      IObjectContainer c;
      if (cfg.TryGet<IObjectContainer>(ObjectContainer_SettingsKey, out c))
        throw new InvalidOperationException(string.Format("Cannot set object container twice. Existing container is {0} - now got {1}.", c, container));

      cfg.Set(ObjectContainer_SettingsKey, container);
      Logger.DebugFormat("Using object container {0}", container);

      return new BaseConfiguration(cfg);
    }


    public static IObjectContainer GetObjectContainer(AbstractConfiguration cfg)
    {
      IObjectContainer container = cfg.Get<IObjectContainer>(ObjectContainer_SettingsKey);
      if (container == null)
        throw new InvalidOperationException(string.Format("No object container has been configured for dependency injection."));
      return container;
    }
  }
}
