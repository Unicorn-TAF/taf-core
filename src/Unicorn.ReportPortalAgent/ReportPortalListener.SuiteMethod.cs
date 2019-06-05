﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using Unicorn.Taf.Core.Testing;

namespace Unicorn.ReportPortalAgent
{
    public partial class ReportPortalListener
    {
        private readonly Dictionary<SuiteMethodType, TestItemType> itemTypes =
            new Dictionary<SuiteMethodType, TestItemType>
        {
            { SuiteMethodType.BeforeSuite, TestItemType.BeforeClass },
            { SuiteMethodType.BeforeTest, TestItemType.BeforeMethod },
            { SuiteMethodType.AfterTest, TestItemType.AfterMethod },
            { SuiteMethodType.AfterSuite, TestItemType.AfterClass },
            { SuiteMethodType.Test, TestItemType.Step },
        };

        internal void StartSuiteMethod(SuiteMethod suiteMethod)
        {
            try
            {
                var id = suiteMethod.Outcome.Id;
                var parentId = suiteMethod.Outcome.ParentId;
                var name = suiteMethod.Outcome.Title;

                this.currentTest = suiteMethod;

                var startTestRequest = new StartTestItemRequest
                {
                    StartTime = DateTime.UtcNow,
                    Name = name,
                    Type = itemTypes[suiteMethod.MethodType]
                };

                startTestRequest.Tags = new List<string>();
                startTestRequest.Tags.Add(suiteMethod.Outcome.Author);
                startTestRequest.Tags.Add(Environment.MachineName);

                var testVal = this.suitesFlow[parentId].StartNewTestNode(startTestRequest);
                this.testFlowIds[id] = testVal;
            }
            catch (Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }

        internal void FinishSuiteMethod(SuiteMethod suiteMethod)
        {
            try
            {
                var id = suiteMethod.Outcome.Id;
                var result = suiteMethod.Outcome.Result;

                this.currentTest = null;

                if (!this.testFlowIds.ContainsKey(id))
                {
                    return;
                }

                var updateTestRequest = new UpdateTestItemRequest();

                // adding categories to test
                updateTestRequest.Tags = new List<string>();

                if (suiteMethod.MethodType.Equals(SuiteMethodType.Test))
                {
                    (suiteMethod as Test).Categories.ForEach(c => updateTestRequest.Tags.Add(c));
                }

                // adding description to test
                updateTestRequest.Description = suiteMethod.Outcome.Title;

                this.testFlowIds[id].Update(updateTestRequest);

                // adding failure items
                if (suiteMethod.Outcome.Result == Taf.Core.Testing.Status.Failed)
                {
                    var text = suiteMethod.Outcome.Exception.Message + Environment.NewLine + suiteMethod.Outcome.Exception.StackTrace;

                    if (!string.IsNullOrEmpty(suiteMethod.Outcome.Screenshot))
                    {
                        byte[] screenshotBytes = File.ReadAllBytes(suiteMethod.Outcome.Screenshot);
                        AddAttachment(id, LogLevel.Error, text, "Fail screenshot", "image/png", screenshotBytes);
                    }
                    else
                    {
                        AddLog(id, LogLevel.Error, text);
                    }

                    if (!string.IsNullOrEmpty(suiteMethod.Outcome.Output))
                    {
                        byte[] outputBytes = Encoding.ASCII.GetBytes(suiteMethod.Outcome.Output);
                        AddAttachment(id, LogLevel.Error, string.Empty, "Execution log", "text/plain", outputBytes);
                    }
                }

                var finishTestRequest = new FinishTestItemRequest
                {
                    EndTime = DateTime.UtcNow,
                    Status = statusMap[result]
                };

                // adding issue to finish test if failed test has a defect
                if (suiteMethod.Outcome.Result == Taf.Core.Testing.Status.Failed && suiteMethod.Outcome.Defect != null)
                {
                    finishTestRequest.Issue = new Issue
                    {
                        Type = suiteMethod.Outcome.Defect.DefectType,
                        Comment = suiteMethod.Outcome.Defect.Comment
                    };
                }

                // finishing test
                this.testFlowIds[id].Finish(finishTestRequest);
            }
            catch (Exception exception)
            {
                Console.WriteLine("ReportPortal exception was thrown." + Environment.NewLine + exception);
            }
        }
    }
}