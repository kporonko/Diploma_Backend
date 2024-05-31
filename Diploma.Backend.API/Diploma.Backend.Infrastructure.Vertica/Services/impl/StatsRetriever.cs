using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Helpers;
using Diploma.Backend.Application.Services;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Models;
using Diploma.Backend.Infrastructure.Stats.Helpers;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vertica.Data.VerticaClient;

namespace Diploma.Backend.Infrastructure.Stats.Services.impl
{
    public class StatsRetriever : IStatsRetriever
    {
        private readonly string _connectionString;

        public StatsRetriever(IConfiguration configuration)
        {
            _connectionString = BuildConnectionString(configuration);
        }

        public async Task<BaseResponse<StatsResponse>> GetStats(int questionId)
        {
            var response = new StatsResponse();

            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                response.StatsForQuestion = await GetStatsForQuestion(connection, questionId);
                response.StatsForOption = await GetStatsForOption(connection, questionId);
                response.StatsForGender = await GetStatsForGender(connection, questionId);
                response.StatsForGeo = await GetStatsForGeo(connection, questionId);
                response.StatsForLang = await GetStatsForLang(connection, questionId);
            }

            return BaseResponseGenerator.GenerateValidBaseResponse(response);
        }

        private VerticaConnection CreateConnection()
        {
            return new VerticaConnection(_connectionString);
        }

        private async Task<StatsForQuestion> GetStatsForQuestion(VerticaConnection connection, int questionId)
        {
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

            return null;
        }

        private async Task<StatsForOption> GetStatsForOption(VerticaConnection connection, int questionId)
        {
            using (var command = CreateCommand(connection, QueryProvider.GetStatsForOptionQuery, questionId))
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new StatsForOption
                    {
                        QuestionId = reader.GetInt32(reader.GetOrdinal("question_id")),
                        OptionId = reader.GetInt32(reader.GetOrdinal("option_id")),
                        Answered = reader.GetInt32(reader.GetOrdinal("answered"))
                    };
                }
            }

            return null;
        }

        private async Task<StatsForGender> GetStatsForGender(VerticaConnection connection, int questionId)
        {
            using (var command = CreateCommand(connection, QueryProvider.GetStatsForGenderQuery, questionId))
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new StatsForGender
                    {
                        QuestionId = reader.GetInt32(reader.GetOrdinal("question_id")),
                        OptionId = reader.GetInt32(reader.GetOrdinal("option_id")),
                        Gender = reader.GetString(reader.GetOrdinal("gender")),
                        Answered = reader.GetInt32(reader.GetOrdinal("answered"))
                    };
                }
            }

            return null;
        }

        private async Task<StatsForGeo> GetStatsForGeo(VerticaConnection connection, int questionId)
        {
            using (var command = CreateCommand(connection, QueryProvider.GetStatsForGeoQuery, questionId))
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new StatsForGeo
                    {
                        QuestionId = reader.GetInt32(reader.GetOrdinal("question_id")),
                        OptionId = reader.GetInt32(reader.GetOrdinal("option_id")),
                        Geo = reader.GetString(reader.GetOrdinal("geo")),
                        Answered = reader.GetInt32(reader.GetOrdinal("answered"))
                    };
                }
            }

            return null;
        }

        private async Task<StatsForLang> GetStatsForLang(VerticaConnection connection, int questionId)
        {
            using (var command = CreateCommand(connection, QueryProvider.GetStatsForLangQuery, questionId))
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new StatsForLang
                    {
                        QuestionId = reader.GetInt32(reader.GetOrdinal("question_id")),
                        OptionId = reader.GetInt32(reader.GetOrdinal("option_id")),
                        Lang = reader.GetString(reader.GetOrdinal("lang")),
                        Answered = reader.GetInt32(reader.GetOrdinal("answered"))
                    };
                }
            }

            return null;
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