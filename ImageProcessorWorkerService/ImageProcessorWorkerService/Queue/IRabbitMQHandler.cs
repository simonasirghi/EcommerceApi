namespace ImageProcessorWorkerService.Queue
{
    public interface IRabbitMQHandler
    {
        void Publish(string message);
        string Receive();
    }
}
