using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Reflection;
using Xunit;

namespace Sgw.KebabCaseRouteTokens.Tests
{
    public class TestStandardController { }
    public class TestNonStandardCont { }


    public class ControllerModelConventionTests
    {
        private ControllerModel controllerModel;
        private TypeInfo typeInfo;

        public ControllerModelConventionTests()
        {
            typeInfo = typeof(TestStandardController).GetTypeInfo();
            controllerModel = new ControllerModel(typeInfo, new object[] { })
            {
                ControllerName = "TestStandard"
            };
        }

        [Fact]
        public void Should_Kebaberize_ControllerName()
        {
            var controllerModelConvention = new KebabCaseRouteTokenReplacementControllerModelConvention();

            controllerModelConvention.Apply(controllerModel);

            controllerModel.ControllerName.Should().Be("test-standard");
        }

        [Fact]
        public void Should_Skip_Kebaberizing_When_TypeName_Does_Not_End_With_Controller()
        {
            typeInfo = typeof(TestNonStandardCont).GetTypeInfo();
            controllerModel = new ControllerModel(typeInfo, new object[] { })
            {
                ControllerName = "TestNonStandardCont"
            };

            var controllerModelConvention = new KebabCaseRouteTokenReplacementControllerModelConvention();

            controllerModelConvention.Apply(controllerModel);

            controllerModel.ControllerName.Should().Be("TestNonStandardCont");
        }
    }
}
