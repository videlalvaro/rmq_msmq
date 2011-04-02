module RabbitMQ.Helper

open RabbitMQ.Client

/// gets a ConnectionFactory
let factory (user, pass, vhost, hostname, port, protocol) : ConnectionFactory = 
    let f = new ConnectionFactory()
    f.UserName    <- user
    f.Password    <- pass
    f.VirtualHost <- vhost
    f.Protocol    <- protocol
    f.HostName    <- hostname
    f.Port        <- port
    f

///Publish MSMQ messages to exchange using rKey
type Publisher(chann:IModel, exchange:string, rKey:string, basicProperties:IBasicProperties) =
    let mutable disposed = false
    
    let cleanup() =
        if not disposed then
            disposed <- true;
            chann.Dispose();

    interface System.IDisposable with
        member x.Dispose() = cleanup()

    member this.CloseAll() = cleanup()
     
    member this.PublishMsg (body:string) =
        let content = System.Text.Encoding.UTF8.GetBytes(body)
        chann.BasicPublish(exchange, rKey, basicProperties, content)