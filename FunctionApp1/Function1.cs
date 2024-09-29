
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;
using Azure.Messaging.ServiceBus;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;



namespace MyFunctionApp;

    public class ValidateAndProcess
    {
        [Function("ValidateAndProcess")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
             RequestModel data;

            try
            {
                data = JsonConvert.DeserializeObject<RequestModel>(requestBody);
            }
            catch (JsonException)
            {
                return new BadRequestObjectResult("Invalid JSON format.");
            }

            // Validate
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(data, serviceProvider: null, items: null);
            bool isValid = Validator.TryValidateObject(data, validationContext, validationResults, true);

            if (!isValid)
            {
                var errors = validationResults.Select(vr => vr.ErrorMessage).ToArray();
                return new BadRequestObjectResult(new { Message = "Validation failed", Errors = errors });
            }

            // sending to service bus 
            await SendToServiceBus(data);

            return new OkObjectResult("Data is valid and has been sent to Service Bus.");
        }

/*==================================================== Service bus Sender ==========================================================================*/

        public static async Task SendToServiceBus(RequestModel model)
        {
            string connectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
            string topicName = Environment.GetEnvironmentVariable("TopicName");

            await using var client = new ServiceBusClient(connectionString);

            
            ServiceBusSender sender = client.CreateSender(topicName);
            string messageBody = JsonConvert.SerializeObject(model);
            ServiceBusMessage message = new ServiceBusMessage(messageBody);
            await sender.SendMessageAsync(message);

            Console.WriteLine($"Message sent to topic: {topicName}");
        }
    }

