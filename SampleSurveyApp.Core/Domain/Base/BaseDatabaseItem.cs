using System;
using SampleSurveyApp.Core.Services;
using SQLite;

namespace SampleSurveyApp.Core.Domain.Base
{
    public abstract class BaseDatabaseItem : IDatabaseItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}

