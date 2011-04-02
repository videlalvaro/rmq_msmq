module RabbitMQ.Bridge.Bridge

open System.Messaging
open RabbitMQ.Client

open MSMQ.Util
open RabbitMQ.Helper

let onMessage (rmqPublisher:Publisher) (receiver:Receiver)(source:obj) (asyncResult:ReceiveCompletedEventArgs) = 
    try
        let queue = source :?> MessageQueue
        let msg  = queue.EndReceive(asyncResult.AsyncResult)
        let body = msg.Body.ToString()
        if body = "quit" then
            receiver.keepConsuming <- false
        else
            rmqPublisher.PublishMsg body
            queue.BeginReceive()
            |> ignore
    with
        | err -> printfn "Error: %s" err.Message

let msmq2rmq connOptions (exchange, rKey, basicProperties) msQueue = do
    let f = factory connOptions
    use conn = f.CreateConnection()
    use chann = conn.CreateModel()
    use publisher = new Publisher(chann, exchange, rKey, basicProperties)
    use q = MSMQ.Util.queue msQueue
    use receiver = new Receiver(q)
    receiver.startConsuming q (new BinaryMessageFormatter()) (onMessage publisher receiver)