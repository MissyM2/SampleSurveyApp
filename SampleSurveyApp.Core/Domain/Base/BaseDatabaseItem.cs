using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SampleSurveyApp.Core.Services;
using SQLite;

namespace SampleSurveyApp.Core.Domain.Base
{
    public abstract class BaseDatabaseItem : ObservableObject, IDatabaseItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}

