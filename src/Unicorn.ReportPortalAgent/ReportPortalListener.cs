﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using ReportPortal.Client;
using ReportPortal.Shared;
using ReportPortal.Shared.Reporter;
using Unicorn.ReportPortalAgent.Configuration;
using Unicorn.Taf.Core.Testing;

namespace Unicorn.ReportPortalAgent
{
    public partial class ReportPortalListener
    {
        private static Dictionary<Status, ReportPortal.Client.Models.Status> statusMap = new Dictionary<Status, ReportPortal.Client.Models.Status>();

        private Dictionary<Guid, ITestReporter> suitesFlow = new Dictionary<Guid, ITestReporter>();
        private Dictionary<Guid, ITestReporter> testFlowIds = new Dictionary<Guid, ITestReporter>();

        static ReportPortalListener()
        {
            var configPath = Path.Combine(
                Path.GetDirectoryName(new Uri(typeof(Config).Assembly.CodeBase).LocalPath),
                "ReportPortal.conf");
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configPath));

            Service reportPortalService;
            if (Config.Server.Proxy != null)
            {
                reportPortalService = new Service(Config.Server.Url, Config.Server.Project, Config.Server.Authentication.Uuid, new WebProxy(Config.Server.Proxy));
            }
            else
            {
                reportPortalService = new Service(Config.Server.Url, Config.Server.Project, Config.Server.Authentication.Uuid);
            }

            Bridge.Service = reportPortalService;

            statusMap[Status.Passed] = ReportPortal.Client.Models.Status.Passed;
            statusMap[Status.Failed] = ReportPortal.Client.Models.Status.Failed;
            statusMap[Status.Skipped] = ReportPortal.Client.Models.Status.Skipped;
        }

        public static Config Config
        {
            get;

            private set;
        }

        public string ExistingLaunchId
        {
            get;

            set;
        }

        public void ReportAddAttachment(Test test, string text, string attachmentName, string mime, byte[] content)
        {
            if (Config.IsEnabled && this.testFlowIds.ContainsKey(test.Outcome.Id))
            {
                AddAttachment(test.Outcome.Id, ReportPortal.Client.Models.LogLevel.Info, text, attachmentName, mime, content);
            }
        }
    }
}
