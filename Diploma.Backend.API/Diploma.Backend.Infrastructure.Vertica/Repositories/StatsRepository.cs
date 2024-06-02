using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Repositories.Stats;
using Diploma.Backend.Infrastructure.Stats.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertica.Data.VerticaClient;

namespace Diploma.Backend.Infrastructure.Stats.Repositories
{
    public class StatsRepository : IStatsRepository
    {
        private readonly string _connectionString;

        public StatsRepository(IConfiguration configuration)
        {
            _connectionString = BuildConnectionString(configuration);
        }

        public async Task<StatsForQuestion> GetStatsForQuestion(int questionId)
        {
            using (var connection = new VerticaConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = CreateCommand(connection, QueryProvider.GetStatsForQuestionQuery, questionId))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new StatsForQuestion
                        {
                            QuestionId = reader.GetInt32(reader.GetOrdinal("question_id")),
                            Views = reader.GetInt32(reader.GetOrdinal("views")),
                            Answered = reader.GetInt32(reader.GetOrdinal("answered"))
                        };
                    }
                }
            }

            return null;
        }

        public async Task<List<StatsForOption>> GetStatsForOption(int questionId)
        {
            var statsForOptions = new List<StatsForOption>();

            using (var connection = new VerticaConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = CreateCommand(connection, QueryProvider.GetStatsForOptionQuery, questionId))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        statsForOptions.Add(new StatsForOption
                        {
                            QuestionId = reader.GetInt32(reader.GetOrdinal("question_id")),
                            OptionId = reader.GetInt32(reader.GetOrdinal("option_id")),
                            Answered = reader.GetInt32(reader.GetOrdinal("answered"))
                        });
                    }
                }
            }

            return statsForOptions;
        }

        public async Task<List<StatsForGender>> GetStatsForGender(int questionId)
        {
            var statsForGenders = new List<StatsForGender>();

            using (var connection = new VerticaConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = CreateCommand(connection, QueryProvider.GetStatsForGenderQuery, questionId))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        statsForGenders.Add(new StatsForGender
                        {
                            QuestionId = reader.GetInt32(reader.GetOrdinal("question_id")),
                            OptionId = reader.GetInt32(reader.GetOrdinal("option_id")),
                            Gender = reader.GetString(reader.GetOrdinal("gender")),
                            Answered = reader.GetInt32(reader.GetOrdinal("answered"))
                        });
                    }
                }
            }

            return statsForGenders;
        }

        public async Task<List<StatsForGeo>> GetStatsForGeo(int questionId)
        {
            var statsForGeos = new List<StatsForGeo>();

            using (var connection = new VerticaConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = CreateCommand(connection, QueryProvider.GetStatsForGeoQuery, questionId))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        statsForGeos.Add(new StatsForGeo
                        {
                            QuestionId = reader.GetInt32(reader.GetOrdinal("question_id")),
                            OptionId = reader.GetInt32(reader.GetOrdinal("option_id")),
                            Geo = reader.GetString(reader.GetOrdinal("geo")),
                            Answered = reader.GetInt32(reader.GetOrdinal("answered"))
                        });
                    }
                }
            }

            return statsForGeos;
        }

        public async Task<List<StatsForLang>> GetStatsForLang(int questionId)
        {
            var statsForLangs = new List<StatsForLang>();

            using (var connection = new VerticaConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = CreateCommand(connection, QueryProvider.GetStatsForLangQuery, questionId))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        statsForLangs.Add(new StatsForLang
                        {
                            QuestionId = reader.GetInt32(reader.GetOrdinal("question_id")),
                            OptionId = reader.GetInt32(reader.GetOrdinal("option_id")),
                            Lang = reader.GetString(reader.GetOrdinal("lang")),
                            Answered = reader.GetInt32(reader.GetOrdinal("answered"))
                        });
                    }
                }
            }

            return statsForLangs;
        }

        private VerticaCommand CreateCommand(VerticaConnection connection, string query, int questionId)
        {
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new VerticaParameter("@QuestionId", questionId));
            return command;
        }

        private string BuildConnectionString(IConfiguration configuration)
        {
            var verticaSettings = GetVerticaSettings(configuration);
            var host = verticaSettings["Host"];
            var port = int.Parse(verticaSettings["Port"]);
            var database = verticaSettings["Database"];
            var user = verticaSettings["User"];
            var password = verticaSettings["Password"];

            var builder = new VerticaConnectionStringBuilder
            {
                Host = host,
                Port = port,
                Database = database,
                User = user,
                Password = password
            };

            return builder.ToString();
        }

        private IConfigurationSection GetVerticaSettings(IConfiguration configuration)
        {
            return configuration.GetSection("VerticaSettings");
        }
    }
}
