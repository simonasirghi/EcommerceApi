namespace EcommerceApi.Queue
{
    public interface IRabbitMQHandler
    {
        void Publish(string message);
        string Receive();
    }
}
