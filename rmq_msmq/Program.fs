module Program

open System.Messaging
open RabbitMQ.Client

open MSMQ.Util
open RabbitMQ.Helper
open RabbitMQ.Bridge

Bridge.msmq2rmq ("guest", "guest", "/", "localhost", 
            AmqpTcpEndpoint.UseDefaultPort, 
            Protocols.SafeLookup("AMQP_0_9")) 
         ("", "ms-test", null)
         @".\private$\MyTestingQueues"