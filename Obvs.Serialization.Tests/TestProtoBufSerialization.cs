﻿using FakeItEasy;
using NUnit.Framework;
using Obvs.Configuration;
using Obvs.Serialization.ProtoBuf;
using Obvs.Serialization.ProtoBuf.Configuration;
using Obvs.Types;

namespace Obvs.Serialization.Tests
{
    
    public class TestProtoBufSerialization
    {
        [Test]
        public void ShouldSerializeToProtoBuf()
        {
            IMessageSerializer serializer = new ProtoBufMessageSerializer();

            var message = new TestMessageProto { Id = 123, Name = "SomeName" };
            var serialize = serializer.Serialize(message);

            Assert.NotNull(serialize);
            Assert.AreEqual(serialize.Length, 25);
        }

        [Test]
        public void ShouldDeserializeFromProtoBuf()
        {
            IMessageSerializer serializer = new ProtoBufMessageSerializer();
            IMessageDeserializer<TestMessageProto> deserializer = new ProtoBufMessageDeserializer<TestMessageProto>();

            var message = new TestMessageProto { Id = 123, Name = "SomeName" };
            var serialize = serializer.Serialize(message);
            var deserialize = deserializer.Deserialize(serialize);

            Assert.AreEqual(message, deserialize);
        }

        [Test]
        public void ShouldPassInCorrectFluentConfig()
        {
            var fakeConfigurator = A.Fake<ICanSpecifyEndpointSerializers<IMessage, ICommand, IEvent, IRequest, IResponse>>();
            fakeConfigurator.SerializedAsProtoBuf();
            
            A.CallTo(() => fakeConfigurator.SerializedWith(
                A<IMessageSerializer>.That.IsInstanceOf(typeof (ProtoBufMessageSerializer)),
                A<IMessageDeserializerFactory>.That.IsInstanceOf(typeof (ProtoBufMessageDeserializerFactory))))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
