using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using Castle.Windsor;
using NUnit.Framework;
using Xyperico.Base.DomainEvents;
using Xyperico.Base.ObjectContainers;


namespace Xyperico.Base.Testing
{
  public abstract class TestHelper
  {
    public static IObjectContainer ObjectContainer
    {
      get
      {
        return Xyperico.Base.ObjectContainer.Container;
      }
    }


    public static void ClearObjectContainer()
    {
      Xyperico.Base.ObjectContainers.Castle.Container = new WindsorContainer();
      Xyperico.Base.ObjectContainer.Container = new CastleObjectContainer(Xyperico.Base.ObjectContainers.Castle.Container);
    }


    /// <summary>
    /// Do not call. For use by NUnit only.
    /// </summary>
    [TestFixtureSetUp]
    public void MasterTestFixtureSetUp()
    {
      TestFixtureSetUp();
    }


    /// <summary>
    /// Executed only once before all tests. Override in subclasses to do subclass
    /// set up. Remember to call base.TestFixtureSetUp().
    /// NOTE: The [TestFixtureSetUp] attribute cannot be used in subclasses because it is already
    /// in use.
    /// </summary>
    protected virtual void TestFixtureSetUp()
    {
    }


    /// <summary>
    /// Do not call. For use by NUnit only.
    /// </summary>
    [SetUp]
    public void MasterSetUp()
    {
      SetUp();
    }


    /// <summary>
    /// Executed before each test method is run. Override in subclasses to do subclass
    /// set up. Remember to call base.SetUp().
    /// NOTE: The [SetUp] attribute cannot be used in subclasses because it is already
    /// in use.
    /// </summary>
    protected virtual void SetUp()
    {
    }


    /// <summary>
    /// Do not call. For use by NUnit only.
    /// </summary>
    [TearDown]
    public void MasterTearDown()
    {
      TearDown();
      DisposeBuiltInstances();
      DomainEventManager.ClearCallbacks();
    }

    /// <summary>
    /// Executed after each test method is run.  Override in subclasses to do subclass
    /// clean up. Remember to call base.TearDown().
    /// NOTE: [TearDown] attribute cannot be used in subclasses because it is
    /// already in use.
    /// </summary>
    protected virtual void TearDown()
    {
    }

    /// <summary>
    /// Do not call. For use by NUnit only.
    /// </summary>
    [TestFixtureTearDown]
    public void TestFixtureMasterTearDown()
    {
      TestFixtureTearDown();
    }

    /// <summary>
    /// Executed only once after all tests.  Override in subclasses to do subclass
    /// clean up. Remember to call base.TestFixtureTearDown().
    /// NOTE: [TestFixtureTearDown] attribute cannot be used in subclasses because it is
    /// already in use.
    /// </summary>
    protected virtual void TestFixtureTearDown()
    {
    }


    #region Builders

    private List<IDisposingBuilder> DisposingBuilders = new List<IDisposingBuilder>();

    
    protected void RegisterBuilder(IDisposingBuilder builder)
    {
      DisposingBuilders.Add(builder);
    }


    protected void DisposeBuiltInstances()
    {
      foreach (IDisposingBuilder builder in DisposingBuilders)
        builder.DisposeInstances();
    }

    #endregion


    #region Additional asserts

    public static void AssertStringContains(string needle, string haystack)
    {
      if (!haystack.Contains(needle))
        Assert.Fail("The string '{0}' is not contained in '{1}'.", needle, haystack);
    }


    public static MatchCollection AssertStringMatch(string needle, string haystack)
    {
      return AssertStringMatch(new Regex(needle, RegexOptions.Multiline), haystack);
    }


    public static MatchCollection AssertStringMatch(Regex needle, string haystack)
    {
      MatchCollection m = needle.Matches(haystack);
      if (m.Count == 0)
        Assert.Fail("The regex '{0}' does not match '{1}'.", needle, haystack);
      return m;
    }


    public static void AssertNotStringMatch(Regex needle, string haystack)
    {
      if (needle.Match(haystack).Success)
        Assert.Fail("The regex '{0}' matches unexpectedly '{1}'.", needle, haystack);
    }


    // source and inspiration: http://srtsolutions.com/blogs/chrismarinos/archive/2008/06/06/testing-for-exceptions-in-unit-test-frameworks.aspx
    public static void AssertThrows<Exception>(Action blockThatThrowsException) where Exception : System.Exception
    {
      try
      {
        blockThatThrowsException();
      }
      catch (System.Exception ex)
      {
        Console.WriteLine("Exeception text: " + ex.Message);

        if (ex.GetType() == typeof(Exception))
          return;

        Console.WriteLine(ex.StackTrace);
        Assert.Fail(String.Format("Expected {0}, got {1}", typeof(Exception), ex.GetType()));
      }

      Assert.Fail(String.Format("Expected {0}, but no exception was thrown", typeof(Exception)));
    }


    public static void AssertThrows<ExT>(Action blockThatThrowsException,
                                         Action<ExT> exceptionVerifier) where ExT : System.Exception
    {
      AssertThrows<ExT>(blockThatThrowsException, ex => { exceptionVerifier(ex); return true; });
    }


    public static void AssertThrows<ExT>(Action blockThatThrowsException,
                                         Func<ExT, bool> exceptionVerifier) where ExT : System.Exception
    {
      try
      {
        blockThatThrowsException();
      }
      catch (Exception ex)
      {
        Console.WriteLine("Exeception text: " + ex.Message);

        if (ex.GetType() != typeof(ExT))
        {
          Console.WriteLine(ex.StackTrace);
          Assert.Fail(String.Format("Expected {0}, got {1}", typeof(ExT), ex.GetType()));
        }

        if (!exceptionVerifier((ExT)ex))
          Assert.Fail(string.Format("Exception {0} failed verification. Got message: {1}", typeof(ExT), ex.Message));

        return;
      }

      Assert.Fail(String.Format("Expected {0}, but no exception was thrown", typeof(ExT)));
    }

    #endregion


    public static string GetMachineSpecificConfigurationSetting(string name)
    {
      string setting = ConfigurationManager.AppSettings[name + "_" + System.Environment.MachineName];
      if (setting == null)
        setting = ConfigurationManager.AppSettings[name];
      if (setting == null)
        throw new ArgumentException(string.Format("Could not locate any settings in config file that matched '{0}'.", name));
      return setting;
    }
  }
}
