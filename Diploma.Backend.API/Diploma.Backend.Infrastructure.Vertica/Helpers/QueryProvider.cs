using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Infrastructure.Stats.Helpers
{

    public static class QueryProvider
    {
        public const string GetStatsForQuestionQuery = "SELECT question_id, SUM(views) AS views, SUM(answered) AS answered " +
                                                       "FROM pure_survey.completion_stats_overall_proj " +
                                                       "WHERE question_id = @QuestionId " +
                                                       "GROUP BY question_id;";

        public const string GetStatsForOptionQuery = "SELECT question_id, option_id, SUM(answered) AS answered " +
                                                     "FROM pure_survey.completion_stats_options_proj " +
                                                     "WHERE question_id = @QuestionId " +
                                                     "GROUP BY question_id, option_id;";

        public const string GetStatsForGenderQuery = "SELECT question_id, option_id, gender, SUM(answered) AS answered " +
                                                     "FROM pure_survey.completion_stats_gender_proj " +
                                                     "WHERE question_id = @QuestionId " +
                                                     "GROUP BY question_id, option_id, gender;";

        public const string GetStatsForGeoQuery = "SELECT question_id, option_id, geo, SUM(answered) AS answered " +
                                                  "FROM pure_survey.completion_stats_geo_proj " +
                                                  "WHERE question_id = @QuestionId " +
                                                  "GROUP BY question_id, option_id, geo;";

        public const string GetStatsForLangQuery = "SELECT question_id, option_id, lang, SUM(answered) AS answered " +
                                                   "FROM pure_survey.completion_stats_lang_proj " +
                                                   "WHERE question_id = @QuestionId " +
                                                   "GROUP BY question_id, option_id, lang;";
    }
}
