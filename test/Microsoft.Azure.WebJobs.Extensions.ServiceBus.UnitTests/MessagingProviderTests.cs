﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Options;
using Xunit;

namespace Microsoft.Azure.WebJobs.ServiceBus.UnitTests
{
    public class MessagingProviderTests
    {
        [Fact]
        public void CreateMessageReceiver_ReturnsExpectedReceiver()
        {
            string defaultConnection = "Endpoint=sb://default.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=abc123=";
            var config = new ServiceBusOptions
            {
                ConnectionString = defaultConnection
            };
            var provider = new MessagingProvider(new OptionsWrapper<ServiceBusOptions>(config));
            var receiver = provider.CreateMessageReceiver("entityPath");
            Assert.Equal("entityPath", receiver.Path);

            var receiver2 = provider.CreateMessageReceiver("entityPath");
            Assert.Same(receiver, receiver2);

            config.PrefetchCount = 100;
            receiver = provider.CreateMessageReceiver("entityPath1");
            Assert.Equal(100, receiver.PrefetchCount);
        }

        [Fact]
        public void CreateMessageReceiverWithMSI_ReturnsExpectedReceiver()
        {
            
            var config = new ServiceBusOptions
            {
                UseManagedServiceIdentity = true,
                Endpoint = "sb://default.servicebus.windows.net/"
            };
            var provider = new MessagingProvider(new OptionsWrapper<ServiceBusOptions>(config));
            var receiver = provider.CreateMessageReceiver("entityPath");
            Assert.Equal("entityPath", receiver.Path);

            var receiver2 = provider.CreateMessageReceiver("entityPath");
            Assert.Same(receiver, receiver2);

            config.PrefetchCount = 100;
            receiver = provider.CreateMessageReceiver("entityPath1");
            Assert.Equal(100, receiver.PrefetchCount);
        }

        [Fact]
        public void CreateMessageSender_ReturnsExpectedSender()
        {
            string defaultConnection = "Endpoint=sb://default.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=abc123=";
            var config = new ServiceBusOptions
            {
                ConnectionString = defaultConnection
            };
            var provider = new MessagingProvider(new OptionsWrapper<ServiceBusOptions>(config));
            var sender = provider.CreateMessageSender("entityPath");
            Assert.Equal("entityPath", sender.Path);

            var sender2 = provider.CreateMessageSender("entityPath");
            Assert.Same(sender, sender2);
        }

        [Fact]
        public void CreateMessageSenderWithMSI_ReturnsExpectedSender()
        {
            var config = new ServiceBusOptions
            {
                UseManagedServiceIdentity = true,
                Endpoint = "sb://default.servicebus.windows.net/"
            };
            var provider = new MessagingProvider(new OptionsWrapper<ServiceBusOptions>(config));
            var sender = provider.CreateMessageSender("entityPath");
            Assert.Equal("entityPath", sender.Path);

            var sender2 = provider.CreateMessageSender("entityPath");
            Assert.Same(sender, sender2);
        }
    }
}
