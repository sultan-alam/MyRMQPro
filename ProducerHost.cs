using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

/*	
================================================================
Title:		ProducerHost
Author:		Sultan     
Purpose:	Host Service to run the producer in background
Creation:	02-Mar-2024
================================================================
Modification History    
Author		Date		Description of change    

================================================================    
Missing:    

================================================================    
*/

namespace MyRMQPro
{
    public class ProducerHost : IHostedService
    {
        private IRabbitMQProducer _rabbitMQProducer;

        public ProducerHost(IConfiguration config, IRabbitMQProducer producer)
        {
            _rabbitMQProducer = producer;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            for (int i = 1; i <= 15; i++)
            {
                Deposit deposit = new Deposit(i, i*1000,DateTime.Now);
                _rabbitMQProducer.SendMessage(deposit);            
                Thread.Sleep(1000);
            }
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }        
    }
}
