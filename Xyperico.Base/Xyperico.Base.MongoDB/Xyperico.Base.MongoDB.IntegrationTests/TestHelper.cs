namespace Xyperico.Base.MongoDB.IntegrationTests
{
  public class TestHelper : Xyperico.Base.Tests.TestHelper
  {
    protected MongoDBRepositoryBase MongoDBBase = new MongoDBRepositoryBase();

    protected void FlushAndClearSession()
    {
    }
  }
}
