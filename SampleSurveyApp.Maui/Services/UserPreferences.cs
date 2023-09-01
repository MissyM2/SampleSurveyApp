﻿using System;
using SampleSurveyApp.Core.Services;

namespace SampleSurveyApp.Maui.Services
{
    /// <summary>
    /// The wrapper class around the Xamarin.Essentials.Preferences class.
    /// So that unit testing is possible.
    /// </summary>
    public class UserPreferences : IUserPreferences
    {
        public bool ContainsKey(string key)
        {
            return Preferences.ContainsKey(key);
        }

        public bool Get(string key, bool defaultValue)
        {
            return Preferences.Get(key, defaultValue);
        }

        public string Get(string key, string defaultValue)
        {
            return Preferences.Get(key, defaultValue);
        }

        public void Set(string key, bool value)
        {
            Preferences.Set(key, value);
        }

        public void Set(string key, string value)
        {
            Preferences.Set(key, value);
        }
    }
}
