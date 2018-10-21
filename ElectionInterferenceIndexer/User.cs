using System;

namespace ElectionInterferenceIndexer
{
    public class User : ITimeSeriesDocument<string>
    {
        public string Id => userid;
        public DateTimeOffset Timestamp => account_creation_date;
        public string userid { get; set; }
        public string user_display_name { get; set; }
        public string user_screen_name { get; set; }
        public string user_reported_location { get; set; }
        public string user_profile_description { get; set; }
        public string user_profile_url { get; set; }
        public ulong follower_count { get; set; }
        public ulong following_count { get; set; }
        public DateTimeOffset account_creation_date { get; set; }
        public string account_language { get; set; }
    }
}
