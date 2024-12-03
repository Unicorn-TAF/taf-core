![Nuget](https://img.shields.io/nuget/v/Unicorn.Taf.Core?style=plastic) ![Nuget](https://img.shields.io/nuget/dt/Unicorn.Taf.Core?style=plastic)

# Unicorn.Taf.Core

Core of Unicorn test automation framework.

* Unit test framework implementation
* Base tests runners
* Logger abstractions
* Base utilities
* Asserts and base matchers

### Migration notes
[Migration notes to TAF v4](https://unicorn-taf.github.io/docs/migration/migration-to-taf-v4/)

## Test suite example

```csharp
// Test suite example. The class should be marked as [Suite].
// It's possible to specify any number of suite tags and metadata.
// Suite tags allow to use parameterized targeted runs: suites are selected based on specific tags presence.
[Suite("Hello World web app")]
[Tag(Platforms.Web), Tag(Apps.HelloWorld)]
[Metadata("Description", "Example of test suite with parameterized test.")]
[Metadata("Site link", "https://unicorn-taf.github.io/test-ui-apps.html")]
public class HelloWorldWebSuite : BaseWebSuite
{
    private HelloWorldPage HelloWorld => website.GetPage<HelloWorldPage>();

    // Data for parameterized test. The method should return a list of DataSet.
    // First parameter of DataSet is data set name and it is not a part of test data.
    // For test parameterization the method could be static or non-static.
    // For whole suite parameterization the method should be marked as [SuiteData] and be static.
    public List<DataSet> TestParameters() =>
        new List<DataSet>
        {
            new DataSet("Only title", 
                UsersFactory.GetUser(Users.NoGivenName), "Name is empty",
                UI.Control.HasAttributeContains("class", "error")),

            new DataSet("Title and name", 
                UsersFactory.GetUser(Users.JDoe), "Mr John said: 'Hello World!'", 
                Is.Not(UI.Control.HasAttributeContains("class", "error"))),
        };

    // Example of parameterized test. The method should be marked as [Test]
    // and have the same number of parameters as DataSets in test data (ignoring data set name).
    [Author(Authors.JDoe)]
    [Category(Categories.Smoke)]
    [Test("'Say' button functionality")]
    [TestData(nameof(TestParameters))]
    public void TestSaying(User user, string expectedText,
        TypeSafeMatcher<IControl> dialogMatcher) // It's possible to parameterize even matchers
    {
        if (!string.IsNullOrEmpty(user.Title))
        {
            Do.Website.HelloWorld.SelectTitle(user.Title);
        }

        Do.Website.HelloWorld.InputName(user.GivenName);
        Do.Website.HelloWorld.ClickSay();

        // There is a built-in assertion mechanism which works together with matchers 
        // (rich collection of self-describable checks)
        Assert.That(HelloWorld.Modal, dialogMatcher);
        Assert.That(HelloWorld.Modal.TextContent, UI.Control.HasText(expectedText));
    }

    // Example of simple test with specified category.
    // It's possible to specify tests execution order within a test suite using [Order].
    // Tests with higher order will be executed later.
    [Author(Authors.JDoe)]
    [Category(Categories.Smoke)]
    [Test("Hello World page default layout")]
    public void TestHelloWorldDefaultLayout() =>
        Do.Assertion.StartAssertionsChain()
            .VerifyThat(HelloWorld.MainTitle, UI.Control.HasText("\"Hello World\" app"))
            .VerifyThat(HelloWorld.TitleDropdown, UI.Dropdown.HasSelectedValue("Nothing selected"))
            .VerifyThat(HelloWorld.NameInput, UI.TextInput.HasValue(string.Empty))
            .AssertChain();

    // Actions executed after each test.
    // It's possible to specify:
    //  - whether it needs to be run in case of test fail or not
    //  - whether need to skip all next tests if AfterTest is failed or not
    [AfterTest]
    public void RefreshPage() =>
        Do.Website.RefreshPage();
}
```

## Test assembly setup and tear down

```csharp

// Actions performed before and/or after all tests execution.
[TestAssembly]
public class TestsAssembly
{
    // Actions before all tests execution.
    // The method should be static.
    [RunInitialize]
    public static void InitRun()
    {
        // Use of custom logger instead of default Console logger.
        ULog.SetLogger(new CustomLogger());

        // Set trace logging level.
        ULog.SetLevel(LogLevel.Trace);

        // It's possible to customize TAF configuration in assembly init. 
        // Current setting controls behavior of dependent tests in case 
        // when referenced test is failed (tests could be failed, skipped or not run)
        Unicorn.Taf.Core.Config.DependentTests = Unicorn.Taf.Core.TestsDependency.Skip;
    }

    // Actions after all tests execution.
    // The method should be static.
    [RunFinalize]
    public static void FinalizeRun()
    {
        // .. some cleanup actions
    }
}
```

## Unicorn config

### Config properties
Any of properties are optional, in case of property absence default value is used. Most of the properties could be overriden by code (for example in `[RunInitialize]`)

 - **testsDependency**: specifies how to deal with dependent tests in case when main test was failed. Available options
    - Skip
    - DoNotRun
    - Run (default)

 - **testsOrder**: specifies in which order to run tests within a suite. Available options
    - Alphabetical
    - Random

 - **parallel**: specifies how to parallel tests execution. Available options
    - Suite
    - None (default)

 - **threads**: specifies how many threads are used to run tests in parallel. Default: 1
 - **testTimeout**: specifies timeout for test and suite method execution in minutes. Default: 15
 - **suiteTimeout**: specifies timeout for test suite execution in minutes. Default: 40
 - **tags**: list of suites tags to be run. Default: empty (don't filter out suites by tags)
 - **categories**: list of test categories to be run. Default: empty (don't filter out tests by categories)
 - **tests**: list of tests masks to be run. Default: empty (run all)

 - **userDefined**: parent for user defined properties
any number of custom properties as key-value pair could be specified. In code the property could be retrieved by calling `Config.GetUserDefinedSetting("setting_name")`.

### Config file example
```json
{
    "testsDependency": "Skip",
    "testsOrder": "Random",
    "parallel": "Suite",
    "threads": 2,
    "testTimeout": 15,
    "suiteTimeout": 60,
    "tags": [ "Tag1", "Tag2" ],
    "categories": [ "Smoke" ],
    "tests": [ ],
    "userDefined": {
        "customProperty1": "value1",
        "customProperty2": "value2"
    }
}
```