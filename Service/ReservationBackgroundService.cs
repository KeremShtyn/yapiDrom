using System.Text.Json;
using Entity.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;


namespace Service
{
    public class ReservationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private IMqttClient? _mqttClient;

        public ReservationBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await SetupMqttClientAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

                using var scope = _serviceProvider.CreateScope();
                var reservationService = scope.ServiceProvider.GetRequiredService<IReservationService>();

                // Fetch and end expired reservations
                var expiredReservations = await reservationService.EndExpiredReservationsAsync();

                // Publish MQTT messages for ended reservations
                foreach (var reservation in expiredReservations)
                {
                    await PublishReservationEndMessageAsync(reservation);
                }                   
                
            }
        }

        private async Task SetupMqttClientAsync()
        {
            var options = new MqttClientOptionsBuilder()
                .WithClientId("ReservationService")
                .WithTcpServer("broker.hivemq.com", 1883) // Public broker example
                .WithCleanSession()
                .Build();

            var mqttFactory = new MqttClientFactory();
            _mqttClient = mqttFactory.CreateMqttClient();

            _mqttClient.ConnectedAsync += async e =>
            {
                Console.WriteLine("Connected to MQTT broker.");
                await Task.CompletedTask;
            };

            _mqttClient.DisconnectedAsync += async e =>
            {
                Console.WriteLine($"Disconnected from MQTT broker: {e.Exception?.Message}");
                await Task.Delay(TimeSpan.FromSeconds(5)); // Retry delay
                await _mqttClient.ConnectAsync(options); // Reconnect
            };

            await _mqttClient.ConnectAsync(options);
        }

        public async Task PublishReservationStartMessageAsync(Reservation reservation)
        {
            if (_mqttClient is null || !_mqttClient.IsConnected)
            {
                Console.WriteLine("MQTT client is not connected. Cannot publish message.");
                return;
            }

            var payload = JsonSerializer.Serialize(new
            {
                ReservationId = reservation.Id,
                UserId = reservation.UserId,
                ScooterId = reservation.ScooterId,
                StartTime = reservation.StartTime
            });

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("ReservationStart")
                .WithPayload(payload)
                .WithQualityOfServiceLevel(0x00)
                .Build();

            await _mqttClient.PublishAsync(message);
            Console.WriteLine($"Published MQTT ReservationStart message for ReservationId: {reservation.Id}");
        }

        private async Task PublishReservationEndMessageAsync(Reservation reservation)
        {
            if (_mqttClient is null || !_mqttClient.IsConnected)
            {
                Console.WriteLine("MQTT client is not connected. Cannot publish message.");
                return;
            }

            var payload = JsonSerializer.Serialize(new
            {
                ReservationId = reservation.Id,
                UserId = reservation.UserId,
                ScooterId = reservation.ScooterId,
                EndedBy = "auto",
                EndTime = reservation.EndTime
            });

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("ReservationEnd")
                .WithPayload(payload)
                .WithQualityOfServiceLevel(0x00)
                .Build();

            await _mqttClient.PublishAsync(message);
            Console.WriteLine($"Published MQTT message for ReservationId: {reservation.Id}");
        }


    }
}
