using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mqtt;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Backdoor
{
    public class Backdoor : Singleton<Backdoor>
    {
        public event EventHandler<BackdoorEventArgs> BackdoorEvent;

        private bool initialized = false;
        private string mqttHost;
        private string baseTopic;
        private string clientId;

        public Boolean IsConnected { get; set; }


        public async Task Initialize(string mqttHost = "localhost", string baseTopic = "Backdoor", string clientId = "MobileApp")
        {
            this.mqttHost = mqttHost;
            this.baseTopic = baseTopic;
            this.clientId = clientId;

            if (initialized)
            {
                return;
            }

            var configuration = new MqttConfiguration();
            var client = await MqttClient.CreateAsync(this.mqttHost, configuration);
            var sessionState = await client.ConnectAsync(new MqttClientCredentials(clientId: this.clientId));
            
            await client.SubscribeAsync($"{this.baseTopic}/#", MqttQualityOfService.AtLeastOnce); // QoS0

            client.MessageStream.Subscribe(msg => HandleReceivedMessage(msg));
            initialized = true;
            this.IsConnected = client.IsConnected;
        }

        private void HandleReceivedMessage(MqttApplicationMessage message)
        {
            var topic = message.Topic;
            var payloadAsString = Encoding.UTF8.GetString(message.Payload);
            BackdoorEvent?.Invoke(this, new BackdoorEventArgs(topic, payloadAsString));
        }
    }

    public class BackdoorEventArgs : EventArgs
    {
        public string Topic { get; private set; }

        public string Subtopic { get; private set; }

        public string Payload { get; private set; }

        public BackdoorEventArgs(string topic, string payload)
        {
            Topic = topic;
            var subTopics = topic.Split('/');
            var subTopicsWithoutBaseTopic = string.Join("/", subTopics.Skip(1));
            Subtopic = subTopicsWithoutBaseTopic;
            Payload = payload;
        }
    }

    public abstract class Singleton<T>
        where T : Singleton<T>, new()
    {
        private static T _instance = new T();
        public static T Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
