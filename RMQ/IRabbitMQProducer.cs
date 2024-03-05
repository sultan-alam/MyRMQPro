/*	
================================================================
Title:		RabbitMQProducer
Author:		Sultan     
Purpose:	RabbitMQ Producer Interface
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
    public interface IRabbitMQProducer
    {
        /// <summary>
        /// Sending message to the Queue (FIFO)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        public void SendMessage<T>(T message);        
    }
}
