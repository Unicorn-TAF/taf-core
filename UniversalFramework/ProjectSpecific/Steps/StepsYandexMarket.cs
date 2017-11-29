﻿using ProjectSpecific.UI.Web;
using AspectInjector.Broker;
using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Steps.Attributes;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Web.Driver;
using Unicorn.UI.Web.Controls;

namespace ProjectSpecific.Steps
{
    [Aspect(typeof(TestStepsEvents))]
    public class StepsYandexMarket : TestSteps
    {
        IDriver driver;

        [TestStep("Open Portal '{0}'")]
        public void OpenPortal(string value)
        {
            driver = WebDriver.Instance;
            driver.Get(value);
        }

        [TestStep("Do Some Actions")]
        public void DoSomeActions()
        {
            YandexTopMenu menu = driver.Find<YandexTopMenu>(ByLocator.Css(".topmenu__list"));
            menu.Link.Click();
            WebControl checkbox = driver.Find<WebControl>(ByLocator.Xpath("//div[@class = 'catalog-menu__list']/a[. = 'Мобильные телефоны']"));
            checkbox.Click();
        }

        [TestStep("Close Browser")]
        public void CloseBrowser()
        {
            driver.Close();
        }
    }
}
