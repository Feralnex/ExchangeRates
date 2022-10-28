using DataAccess.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;

namespace DataAccess.Tests
{
    public class DeserializersTests
    {
        public string MidRate { get; private set; }
        public string TradeRate { get; private set; }

        [SetUp]
        public void Setup()
        {
            MidRate = "{'currency':'bat(Tajlandia)','code':'THB','mid':0.1280}";
            TradeRate = "{'currency':'dolar amerykañski','code':'USD','bid':4.8143,'ask':4.9115}";
        }

        [Test]
        public void DeserializeObject_ShouldReturnMidRate()
        {
            MidRate actual = JsonConvert.DeserializeObject<MidRate>(MidRate);

            Assert.AreEqual("bat(Tajlandia)", actual.Currency.Name);
            Assert.AreEqual("THB", actual.Currency.Code);
            Assert.AreEqual(0.1280M, actual.Mid);
        }

        [Test]
        public void DeserializeObject_ShouldReturnTradeRate()
        {
            TradeRate actual = JsonConvert.DeserializeObject<TradeRate>(TradeRate);

            Assert.AreEqual("dolar amerykañski", actual.Currency.Name);
            Assert.AreEqual("USD", actual.Currency.Code);
            Assert.AreEqual(4.8143M, actual.Bid);
            Assert.AreEqual(4.9115M, actual.Ask);
        }

        [Test]
        public void DeserializeObject_ShouldThrowNullReferenceExceptionWhenJsonDoesNotHaveRequiredParameters()
        {
            Assert.Throws<NullReferenceException>(() => JsonConvert.DeserializeObject<MidRate>(TradeRate));
            Assert.Throws<NullReferenceException>(() => JsonConvert.DeserializeObject<TradeRate>(MidRate));
        }

        [Test]
        public void DeserializeObject_ShouldThrowArgumentNullExceptionWhenJsonIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => JsonConvert.DeserializeObject<MidRate>(null));
            Assert.Throws<ArgumentNullException>(() => JsonConvert.DeserializeObject<TradeRate>(null));
        }

        [Test]
        public void DeserializeObject_ShouldThrowJsonReaderExceptionWhenJsonHasInvalidSyntax()
        {
            Assert.Throws<JsonReaderException>(() => JsonConvert.DeserializeObject<MidRate>("[}"));
            Assert.Throws<JsonReaderException>(() => JsonConvert.DeserializeObject<TradeRate>("[}"));
        }
    }
}