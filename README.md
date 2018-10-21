# Twitter Election Integrity Research

This is a growing suite of tools to interact with the [Election Integrity data](https://about.twitter.com/en_us/values/elections-integrity.html#data) that Twitter released.

## Quickstart

1. Get an Elasticsearch instance running somewhere
2. Install .NET Core
3. In the `ElectionInterferenceIndexer` folder:
    * Run `dotnet run --ElasticsearchUrl http://some-elasticsearch-url:1234 --Type Users https://storage.googleapis.com/twitter-election-integrity/hashed/ira/ira_users_csv_hashed.zip` to index the IRA accounts that Twitter identified
    * Run `dotnet run --ElasticsearchUrl http://some-elasticsearch-url:1234 --Type Tweets https://storage.googleapis.com/twitter-election-integrity/hashed/ira/ira_tweets_csv_hashed.zip` to index the tweets from the IRA accounts that Twitter identified

## Building

TBD

## Code of Conduct

We are committed to fostering an open and welcoming environment. Please read our [code of conduct](CODE_OF_CONDUCT.md) before participating in or contributing to this project.

## Contributing

We welcome contributions and collaboration on this project. Please read our [contributor's guide](CONTRIBUTING.md) to understand how best to work with us.

## License and Authors

[![Daniel James logo](https://secure.gravatar.com/avatar/eaeac922b9f3cc9fd18cb9629b9e79f6.png?size=16) Daniel James](https://github.com/thzinc)

[![license](https://img.shields.io/github/license/thzinc/twitter-election-integrity.svg)](https://github.com/thzinc/twitter-election-integrity/blob/master/LICENSE)
[![GitHub contributors](https://img.shields.io/github/contributors/thzinc/twitter-election-integrity.svg)](https://github.com/thzinc/twitter-election-integrity/graphs/contributors)

This software is made available by Daniel James under the MIT license.