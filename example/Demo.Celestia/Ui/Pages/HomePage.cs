﻿using System;
using Unicorn.Taf.Core.Utility.Synchronization;
using Unicorn.UI.Core.Controls.Interfaces;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.Driver;

namespace Demo.Celestia.Ui.Pages
{
    public class HomePage : BasePage, ILoadable
    {
        public HomePage() : base(string.Empty, "Celestia: Home")
        {
        }

        [Find(Using.WebCss, "h1 > a")]
        public WebControl HomeLink { get; set; }

        [ByClass("fa-map")]
        public WebControl VirtualTextures { get; set; }

        [ByClass("fa-headphones")]
        public WebControl AudioPlaying { get; set; }

        [ByClass("fa-bullseye")]
        public WebControl Trajectories { get; set; }

        public bool WaitForLoading(TimeSpan timeout)
        {
            const string overlayZindexAfterAnimation = "-1";
            const string jsGetOverlayZindex =
                "return window.getComputedStyle(document.querySelector('.landing'), '::after').getPropertyValue('z-index')";
            
            // During fade out animation nothing is interactable, need to wait for interaction is available.
            new DefaultWait(timeout)
                .Until(() => (string)WebDriver.Instance.ExecuteJS(jsGetOverlayZindex) == overlayZindexAfterAnimation);

            return true;
        }
    }
}
