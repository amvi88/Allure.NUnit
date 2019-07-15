﻿using Allure.Commons;
using Allure.Commons.Model;
using TechTalk.SpecFlow;

namespace AllureSpecFlow
{
    [Binding]
    public class AllureBindings
    {
        static AllureLifecycle allure = AllureLifecycle.Instance;

        private FeatureContext featureContext;
        private ScenarioContext scenarioContext;

        public AllureBindings(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            this.featureContext = featureContext;
            this.scenarioContext = scenarioContext;
        }

        [BeforeFeature(Order = int.MinValue)]
        public static void FirstBeforeFeature()
        {
            // start feature container in BindingInvoker
        }

        [AfterFeature(Order = int.MaxValue)]
        public static void LastAfterFeature()
        {
            // write feature container in BindingInvoker
        }

        [BeforeScenario(Order = int.MinValue)]
        public void FirstBeforeScenario()
        {
            PluginHelper.StartTestContainer(featureContext, scenarioContext);
            //AllureHelper.StartTestCase(scenarioContainer.uuid, featureContext, scenarioContext);
        }

        [BeforeScenario(Order = int.MaxValue)]
        public void LastBeforeScenario()
        {
            // start scenario after last fixture and before the first step to have valid current step context in allure storage
            var scenarioContainer = PluginHelper.GetCurrentTestConainer(scenarioContext);
            PluginHelper.StartTestCase(scenarioContainer.uuid, featureContext, scenarioContext);
        }

        [AfterScenario(Order = int.MinValue)]
        public void FirstAfterScenario()
        {
            var scenarioId = PluginHelper.GetCurrentTestCase(scenarioContext).uuid;

            // update status to passed if there were no step of binding failures
            allure
                .UpdateTestCase(scenarioId,
                    x => x.status = (x.status != Status.none) ? x.status : Status.passed)
                .StopTestCase(scenarioId, true);
        }
    }
}