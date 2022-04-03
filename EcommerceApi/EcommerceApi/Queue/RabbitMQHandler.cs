using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace EcommerceApi.Queue
{
    public class RabbitMQHandler:IRabbitMQHandler
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly string _queueName;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public RabbitMQHandler(string queueName)
        {
            _connectionFactory = new ConnectionFactory() { Uri = new Uri("amqp://myuser:mypassword@rabbitmq:5672/") };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = queueName;
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void Publish(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
        }

        public string Receive()
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            string message = null;

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                message = Encoding.UTF8.GetString(body);
            };
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return message;
        }
    }
}
