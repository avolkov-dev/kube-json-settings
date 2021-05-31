using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using StackExchange.Utils;
using Xunit;

namespace TestProject2
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var configuration = new ConfigurationBuilder()
                .WithPrefix(
                    "secrets",
                    c => c.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        {"secretValue1", "verySecret"},
                    })
                )
                .WithSubstitution(
                    c => c.AddInMemoryCollection(new Dictionary<string, string>()
                    {
                        {"Value1", "${secrets:secretValue1}"},
                    })
                )
                .Build();

            var cfg = new Config();
            configuration.Bind(cfg);
            
            Assert.Equal("verySecret", cfg.Value1);
            Assert.Null(cfg.Value2);
        }
        
        private class Config
        {
            public string Value1 { get; set; }
            public string Value2 { get; set; }
            
            public Nested Nested { get; set; }
        }
        
        private class Nested
        {
            public string A { get; set; }
        }
    }
}