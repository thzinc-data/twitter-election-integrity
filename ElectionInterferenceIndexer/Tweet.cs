using System;
using System.Collections.Generic;

namespace ElectionInterferenceIndexer
{
    public class Tweet : ITimeSeriesDocument<string>
    {
        public string Id => tweetid;
        public DateTimeOffset Timestamp => tweet_time;
        public string tweetid { get; set; }
        public string userid { get; set; }
        public string user_display_name { get; set; }
        public string user_screen_name { get; set; }
        public string user_reported_location { get; set; }
        public string user_profile_description { get; set; }
        public string user_profile_url { get; set; }
        public ulong? follower_count { get; set; }
        public ulong? following_count { get; set; }
        public DateTimeOffset account_creation_date { get; set; }
        public string account_language { get; set; }
        public string tweet_language { get; set; }
        public string tweet_text { get; set; }
        public DateTimeOffset tweet_time { get; set; }
        public string tweet_client_name { get; set; }
        public string in_reply_to_tweetid { get; set; }
        public string in_reply_to_userid { get; set; }
        public string quoted_tweet_tweetid { get; set; }
        public bool is_retweet { get; set; }
        public string retweet_userid { get; set; }
        public string retweet_tweetid { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        public ulong? quote_count { get; set; }
        public ulong? reply_count { get; set; }
        public ulong? like_count { get; set; }
        public ulong? retweet_count { get; set; }
        public string hashtags { get; set; }
        public IEnumerable<string> hashtags_list => hashtags?
            .Trim(new[] { '[', ']' })
            .Split(", ");
        public string urls { get; set; }
        public IEnumerable<string> urls_list => urls?
            .Trim(new[] { '[', ']' })
            .Split(", ");
        public string user_mentions { get; set; }
        public IEnumerable<string> user_mentions_list => user_mentions?
            .Trim(new[] { '[', ']' })
            .Split(", ");
        public string poll_choices { get; set; }
        public IEnumerable<string> poll_choices_list => poll_choices?
            .Trim(new[] { '[', ']' })
            .Split(", ");
    }
}
