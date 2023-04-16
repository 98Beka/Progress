using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Serilog.Events;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug) // restricted... is Optional
    .CreateLogger();

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel()) {
    channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);
    var queueName = channel.QueueDeclare().QueueName;
    channel.QueueBind(queue: queueName,
    exchange: "direct_logs",
    routingKey: "logs");
    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (sender, e) => {
        var body = e.Body;
        var message = Encoding.UTF8.GetString(body.ToArray());
        Log.Warning(message);
    };
    channel.BasicConsume(queue: queueName,
        autoAck: true,
        consumer: consumer);
    Console.WriteLine("subscribed to the queue 'dev-queue'");
    Console.ReadLine();
}