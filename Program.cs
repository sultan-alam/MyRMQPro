using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

/*	
================================================================
Title:		Program
Author:		Sultan     
Purpose:	Main method, starting the Producer as Hosted Service
Creation:	02-Mar-2024
================================================================
Modification History    
Author		Date		Description of change    
Sultan      11-Mar-24   Sending configuration to RabbitMQ Producer
================================================================    
Missing:    

================================================================    
*/

namespace MyRMQPro
{
    public class Program
    {
        static void Main(string[] args)
        {           
            Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    var config = context.Configuration;
                    // Host
                    services.AddHostedService<ProducerHost>();                                        
                    // RabbitMQ
                    services.AddSingleton<IRabbitMQProducer,RabbitMQProducer>(i => new RabbitMQProducer(config));
                })
                .Build()
                .Run();
        }
    }
}
