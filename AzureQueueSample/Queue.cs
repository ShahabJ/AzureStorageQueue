using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzureQueueSample
{
    public class Queue
    {
        private CloudStorageAccount storageAccount;
        private string _queueName;

        public static CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }

            return storageAccount;
        }

        public Queue(string queueName)
        {
            storageAccount = GetStorageAccount();
            _queueName = queueName;

        }
        private CloudStorageAccount GetStorageAccount()
        {
            string storageConnectionString = AppSettings.LoadAppSettings().StorageConnectionString;
            return CreateStorageAccountFromConnectionString(storageConnectionString);
        }

        public async Task CreateQueue()
        {
            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a container.
            CloudQueue queue = queueClient.GetQueueReference(_queueName);

            // Create the queue if it doesn't already exist
            await queue.CreateIfNotExistsAsync();
        }


        public async Task InsertMessage(string messageValue)
        {
            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(_queueName);

            // Create the queue if it doesn't already exist.
            await queue.CreateIfNotExistsAsync();

            // Create a message and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage(messageValue);
            await queue.AddMessageAsync(message);
        }

        public async Task PeekNextMessage()
        {
            // Create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference(_queueName);

            // Peek at the next message
            var peekedMessage = await queue.PeekMessageAsync();

            // Display message.
            Console.WriteLine(peekedMessage.AsString);
        }

        public async Task ChangeContent()
        {
            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(_queueName);

            // Get the message from the queue and update the message contents.
            CloudQueueMessage message = await queue.GetMessageAsync();
            message.SetMessageContent("Updated contents.");
            await queue.UpdateMessageAsync(message,
                TimeSpan.FromSeconds(60.0),  // Make it invisible for another 60 seconds.
                MessageUpdateFields.Content | MessageUpdateFields.Visibility);
        }

        public async Task DequeueMessage()
        {
            // Create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference(_queueName);

            // Get the next message
            CloudQueueMessage retrievedMessage = await queue.GetMessageAsync();

            //Process the message in less than 30 seconds, and then delete the message
            await queue.DeleteMessageAsync(retrievedMessage);
        }
    }
}
