# rmq_msmq #

This project implements a bridge between Microsoft MQ and RabbitMQ.

The bridge is implemented using *FSharp* and the [RabbitMQ Client for .Net](http://www.rabbitmq.com/dotnet.html). It has been tested with version 2.4.0

## Implementation ##

If you have a MSMQ public queue called `@".\private$\MyTestingQueues"` then you can subscribe to it, receive messages from it and then send them to RabbitMQ to a queue that you decide.

## Usage ##

In your project you will have to add a reference to the System.Messaging dll and to the RabbitMQ client dll. Then you can have a program like this:

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

					
Here we pass three arguments to Bridge.msmq2rmq:

- A tuple with (user, password, vhost, host, port, protocol) to use while connecting to RabbitMQ.
- A tuple with (exchange, routing_key, basicProperties) used while publishing the message.
- The name of the MSMQ Queue.

In this case we are sending the messages to the *unnamed exchange* using the *queue name* as the *routing key*.

If a message with body "quit" is sent to the MSMQ queue, then the Receiver will quit. The messages are expected to be formatted using `BinaryMessageFormatter`.

## Disclaimer ##

Consider this project as alpha. Just an excercise to learn Fsharp and MSMQ. You could use this as a reference for your implementations or simply contribute to this project to make it more robust.

## TODO ##

- Allow importing messages from RabbitMQ to MSMQ

# LICENSE #

See LICENSE