using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Reflection;
using Xunit;

namespace Sgw.KebabCaseRouteTokens.Tests
{
    public class TestClass
    {
        public void GetUserData()
        {

        }

        public void Get()
        {

        }
    }

    public class ActionModelConventionTests
    {
        private ActionModel actionModel;
        private MethodInfo methodInfo;

        public ActionModelConventionTests()
        {
            methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.GetUserData));
            actionModel = new ActionModel(methodInfo, new object[] { })
            {
                ActionName = methodInfo.Name
            };
        }

        [Fact]
        public void Should_Kebaberize_Full_Method_Name_When_No_Prefixes_Are_Supplied()
        {
            var actionModelConvention = new KebabCaseRouteTokenReplacementActionModelConvention();

            actionModelConvention.Apply(actionModel);

            actionModel.ActionName.Should().Be("get-user-data");
        }

        [Fact]
        public void Should_Kebaberize_Without_Prefix_When_Supplied_As_Params()
        {
            var actionModelConvention = new KebabCaseRouteTokenReplacementActionModelConvention("Get");

            actionModelConvention.Apply(actionModel);

            actionModel.ActionName.Should().Be("user-data");
        }

        [Fact]
        public void Should_Kebaberize_Without_Prefix_When_Supplied_As_Enumerable()
        {
            var actionModelConvention = new KebabCaseRouteTokenReplacementActionModelConvention(new string[] { "Get" });

            actionModelConvention.Apply(actionModel);

            actionModel.ActionName.Should().Be("user-data");
        }

        [Fact]
        public void Should_Skip_Kebaberize_When_ActionName_Differs_From_ActionMethodName()
        {
            string originalActionName = $"Test{methodInfo.Name}";

            actionModel.ActionName = originalActionName;

            var actionModelConvention = new KebabCaseRouteTokenReplacementActionModelConvention("Get");

            actionModelConvention.Apply(actionModel);

            actionModel.ActionName.Should().Be(originalActionName);
        }

        [Fact]
        public void Should_Change_ActionModel_ActionName_To_Empty_String_When_Name_Matches_Supplied_Prefix()
        {
            methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.Get));
            actionModel = new ActionModel(methodInfo, new object[] { })
            {
                ActionName = methodInfo.Name
            };

            var actionModelConvention = new KebabCaseRouteTokenReplacementActionModelConvention(new string[] { "GET" });

            actionModelConvention.Apply(actionModel);

            actionModel.ActionName.Should().Be("");
        }
    }
}
