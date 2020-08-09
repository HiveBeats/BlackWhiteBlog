using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using BlackWhiteBlog.Controllers;

namespace BlackWhiteBlog.Tests.WeatherForecasts
{
    public class WeatherForecastTests
    {
        [Fact]
        public void Controller_Return_Valid_Json()
        {
            var controller = new WeatherForecastController(null);

            var items = controller.Get().ToList();
            var firstItem = items.FirstOrDefault();

            Assert.True(firstItem is WeatherForecast);
        }

        [Fact]
        public void Controller_Return_5_Elements()
        {
            var controller = new WeatherForecastController(null);

            var items = controller.Get().ToList();

            Assert.Equal(5, items.Count);
        }
    }
}