﻿using System.Text;
using FakeItEasy;
using NUnit.Framework;
using Obvs.Configuration;
using Obvs.Serialization.Xml;
using Obvs.Serialization.Xml.Configuration;
using Obvs.Types;

namespace Obvs.Serialization.Tests
{
    
    public class TestXmlSerialization
    {
        [Test]
        public void ShouldSerializeToXml()
        {
            IMessageSerializer serializer = new XmlMessageSerializer();

            var message = new TestMessageProto { Id = 123, Name = "SomeName" };
            var serialize = XmlSerializerDefaults.Encoding.GetString(serializer.Serialize(message));

            Assert.NotNull(serialize);
            StringAssert.Contains(message.Id.ToString(), serialize);
            StringAssert.Contains(message.Name, serialize);
        }

        [Test]
        public void ShouldDeserializeFromXml()
        {
            IMessageSerializer serializer = new XmlMessageSerializer();
            IMessageDeserializer<TestMessageProto> deserializer = new XmlMessageDeserializer<TestMessageProto>();

            var message = new TestMessageProto {Id = 123, Name = "SomeName"};
            var serialize = serializer.Serialize(message);
            var deserialize = deserializer.Deserialize(serialize);

            Assert.AreEqual(message, deserialize);
        }
        
        [Test]
        public void ShouldDeserializeFromXmlAscii()
        {
            IMessageSerializer serializer = new XmlMessageSerializer();
            IMessageDeserializer<TestMessageProto> deserializer = new XmlMessageDeserializer<TestMessageProto>();

            var message = new TestMessageProto {Id = 123, Name = "SomeName"};
            var serialize = serializer.Serialize(message);
            var ascii = Encoding.Convert(XmlSerializerDefaults.Encoding, Encoding.ASCII, serialize);
            var deserialize = deserializer.Deserialize(ascii);

            Assert.AreEqual(message, deserialize);
        }


        [Test]
        public void ShouldPassInCorrectFluentConfig()
        {
            var fakeConfigurator = A.Fake<ICanSpecifyEndpointSerializers<IMessage, ICommand, IEvent, IRequest, IResponse>>();
            fakeConfigurator.SerializedAsXml();
            
            A.CallTo(() => fakeConfigurator.SerializedWith(
                A<IMessageSerializer>.That.IsInstanceOf(typeof (XmlMessageSerializer)),
                A<IMessageDeserializerFactory>.That.IsInstanceOf(typeof (XmlMessageDeserializerFactory))))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}