using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Azure.Messaging.ServiceBus;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.IO;

namespace MyFunctionApp
{
    public class ValidateAndProcess
    {
        [Function("ValidateAndProcess")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            string requestBody;
            RequestModel data;

            try
            {
                requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                data = JsonConvert.DeserializeObject<RequestModel>(requestBody);
            }
            catch (JsonException)
            {
                return new BadRequestObjectResult("Invalid JSON format.");
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
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

            try
            {
                // Sending to service bus
                await SendToServiceBus(data);
            }
            catch (ServiceBusException ex)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return new OkObjectResult("Data is valid and has been sent to Service Bus.");
        }

        public static async Task SendToServiceBus(RequestModel model)
        {
            string connectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
            string topicName = Environment.GetEnvironmentVariable("TopicName");

            try
            {
                await using var client = new ServiceBusClient(connectionString);
                ServiceBusSender sender = client.CreateSender(topicName);
                string messageBody = JsonConvert.SerializeObject(model);
                ServiceBusMessage message = new ServiceBusMessage(messageBody);
                await sender.SendMessageAsync(message);

                Console.WriteLine($"Message sent to topic: {topicName}");
            }
            catch (ServiceBusException ex)
            {
                Console.WriteLine($"ServiceBusException: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }
    }
}
