using NUnit.Framework;
using Xyperico.Base;
using Xyperico.Agres.Serialization;


namespace Xyperico.Agres.Tests
{
  [SetUpFixture]
  public class SetupFixture
  {
    public static void Setup(IObjectContainer container)
    {
      log4net.Config.XmlConfigurator.Configure();
      AbstractSerializer.RegisterKnownType(typeof(MySerializationMessage));
      AbstractSerializer.RegisterKnownType(typeof(MySerializationId));
    }


    [SetUp]
    public void TestSetup()
    {
      Xyperico.Base.Testing.TestHelper.ClearObjectContainer();
      Setup(Xyperico.Base.Testing.TestHelper.ObjectContainer);
    }
  }
}
