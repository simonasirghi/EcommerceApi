

using ImageProcessorWorkerService.Queue;
using ImageProcessorWorkerService.Storage;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ImageProcessorWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly string _queueName;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly CloudStorage _cloudStorage;

        public Worker(IOptions<AzureBlobStorageKeys> options)
        {
            _connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://myuser:mypassword@rabbitmq:5672/"),
                DispatchConsumersAsync = true
            };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = "image-processor";
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _cloudStorage = new CloudStorage(options);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                _channel.Dispose();
                _connection.Dispose();
                return Task.CompletedTask;
            }

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += Consumer_Received;
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            var path = Encoding.UTF8.GetString(@event.Body.Span);
            var fileName = path.Split('/').Last();
            using (FileStream fs = File.OpenRead(path))
            {
                await _cloudStorage.Upload(fs, fileName);
            }
            // Delete temporary local file

            File.Delete(path);


        }
    }
}