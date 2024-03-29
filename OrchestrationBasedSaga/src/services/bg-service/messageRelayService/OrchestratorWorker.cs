﻿using core_application.Interfaces;
using core_domain.Entitites;
using Dapper;
using MassTransit;
using System.Text.Json;

namespace messageRelayService
{
    public class OrchestratorWorker : BackgroundService
    {
        private readonly ILogger<OrchestratorWorker> _logger;
        private readonly IBus _bus;
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public OrchestratorWorker(ILogger<OrchestratorWorker> logger, IBus bus, IEnumerable<IDbConnectionFactory> dbConnectionFactories)
        {
            _logger = logger;
            _bus = bus;
            _dbConnectionFactory = dbConnectionFactories.Single(x => x.Context == "orchestrator");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation("Orchestrator Message Relay Service is running at: {time}", DateTime.Now);

                using (var connection = this._dbConnectionFactory.GetOpenConnection())
                {
                    string sql = $@"     SELECT
                                          ""Id"",
                                          ""Type"",
                                          ""Message"",
                                          ""CreatedOn""
                                     FROM public.""OutboxMessages"" 
                                ";

                    var messages = await connection.QueryAsync<OutboxMessage>(sql);

                    foreach (var relatedOutBoxMessage in messages)
                    {
                        try
                        {
                            var message = JsonSerializer.Deserialize(relatedOutBoxMessage.Message, Type.GetType(relatedOutBoxMessage.Type));

                            await this._bus.Publish(message);

                            await connection.ExecuteAsync(@"DELETE FROM public.""OutboxMessages"" WHERE ""Id""=@Id", new { Id = relatedOutBoxMessage.Id });
                        }
                        catch (Exception ex)
                        {
                            this._logger.LogError(ex, $"{relatedOutBoxMessage.Id} idli mesaj event bus gönderiminde hata ile karşılaşıldı");
                        }
                    }
                }
            }
        }
    }
}
