using NHibernate;
using Xyperico.Base.NHibernate;


namespace Xyperico.Base.IntegrationTests
{
  public class TestHelper : Xyperico.Base.Tests.TestHelper
  {
    protected NHibernateRepositoryBase NHibernateBase = new NHibernateRepositoryBase();


    #region Transaction handling

    protected enum TransactionScopeType { Fixture, Test, Nothing };

    protected enum TransactionResultType { Rollback, Commit };

    protected bool EnableTransaction = true;

    protected TransactionScopeType TransactionScope = TransactionScopeType.Fixture;

    protected TransactionResultType TransactionResult = TransactionResultType.Rollback;

    protected ITransaction Transaction;


    protected override void TestFixtureSetUp()
    {
      if (TransactionScope == TransactionScopeType.Fixture)
        TransactionSetup();
      base.TestFixtureSetUp();
    }

    
    protected override void SetUp()
    {
      if (TransactionScope == TransactionScopeType.Test)
        TransactionSetup();
      base.SetUp();
    }

    
    protected virtual void TransactionSetup()
    {
      FlushData();
      if (EnableTransaction)
        Transaction = Session.BeginTransaction();
    }


    protected override void TearDown()
    {
      base.TearDown();
      if (TransactionScope == TransactionScopeType.Test)
        TransactionTearDown();
    }


    protected override void TestFixtureTearDown()
    {
      base.TestFixtureTearDown();
      if (TransactionScope == TransactionScopeType.Fixture)
        TransactionTearDown();
    }


    protected virtual void TransactionTearDown()
    {
      // A flush brings forth OR-mapper inconcistencies and problems in the tests 
      // and ensures we can see everything in an SQL-profiler if required.
      FlushData();

      // Check for null since something might easily have gone wrong
      if (Transaction != null)
        if (TransactionResult == TransactionResultType.Rollback)
          Transaction.Rollback();
        else
          Transaction.Commit();

      // Important that we clear the session after each test, since we roll back whatever changes was made in it.
      Session.Clear();
    }

    #endregion


    public virtual void FlushData()
    {
      Session.Flush();
    }


    public virtual void FlushAndClearSession()
    {
      Session.Flush();
      Session.Clear();
    }


    protected virtual ISession Session
    {
      get
      {
        return NHibernateBase.Session;
      }
    }
  }
}
