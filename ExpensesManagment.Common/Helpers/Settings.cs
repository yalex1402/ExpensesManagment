﻿using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpensesManagment.Common.Helpers
{
    public static class Settings
    {
        private const string _user = "user";
        private const string _token = "token";
        private const string _isLogin = "isLogin";
        private const string _trips = "trips";
        private const string _tripSelected = "tripSelected";
        private static readonly string _stringDefault = string.Empty;
        private static readonly bool _boolDefault = false;

        private static ISettings AppSettings => CrossSettings.Current;

        public static string Trips
        {
            get => AppSettings.GetValueOrDefault(_trips, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_trips, value);
        }

        public static string TripSelected
        {
            get => AppSettings.GetValueOrDefault(_tripSelected, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_tripSelected, value);
        }

        public static string User
        {
            get => AppSettings.GetValueOrDefault(_user, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_user, value);
        }

        public static string Token
        {
            get => AppSettings.GetValueOrDefault(_token, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_token, value);
        }

        public static bool IsLogin
        {
            get => AppSettings.GetValueOrDefault(_isLogin, _boolDefault);
            set => AppSettings.AddOrUpdateValue(_isLogin, value);
        }
    }
}
