using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

/*	
================================================================
Title:		RabbitMQProducer
Author:		Sultan     
Purpose:	RabbitMQ Producer/ Pulisher
Creation:	02-Mar-2024
================================================================
Modification History    
Author		Date		Description of change    
Sultan      11-Mar-24   Ctor receiving the configuration
================================================================    
Missing:    

================================================================    
*/

namespace MyRMQPro
{
    internal class RabbitMQProducer : IDisposable, IRabbitMQProducer
    {
        private string HostName = string.Empty;
        private int Port = 0;
        private string UserName = string.Empty;
        private string Password = string.Empty;
        private string Exchange = string.Empty;
        private string Type = string.Empty;
        private string Queue = string.Empty;

        private IConnection _connection;
        private IModel _channel;
        private bool disposedValue;

        /// <summary>
        /// Ctor receives the Configuration
        /// </summary>
        /// <param name="config"></param>        
        public RabbitMQProducer(IConfiguration config)
        {            
            HostName = config["RMQ:HostName"];
            Port = config.GetValue<int>("RMQ:Port");
            UserName = config["RMQ:UserName"];
            Password = config["RMQ:Password"];
            Exchange = config["RMQ:Exchange"];
            Type = config["RMQ:Type"];
            Queue = config["RMQ:Queue"];
        }
        /// <summary>
        /// Sending message to the Queue (FIFO)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>       
        public void SendMessage<T>(T message)
        {
            Console.WriteLine("==============================================================================================");
            //Rabbit MQ Server
            /*var factory = new ConnectionFactory
            { 
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            }*/
            var factory = new ConnectionFactory
            {
                HostName = HostName,
                Port = Port,
                UserName = UserName,
                Password = Password
            };
            //Create the RabbitMQ connection
            _connection = factory.CreateConnection();
            //Creating channel with session and model            
            _channel = _connection.CreateModel();
            //Declaring Exchange
            _channel.ExchangeDeclare(Exchange, Type, durable: true, autoDelete: false);
            //Declaring the queue
            _channel.QueueDeclare(Queue, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(Queue, Exchange, string.Empty);
            //Serialize the message
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            //put the data on to the product queue
            _channel.BasicPublish(exchange: Exchange, routingKey: String.Empty, body: body);
            Console.WriteLine($"Message published: {json}");            
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _channel.Close();
                    _connection.Close();
                }                
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
