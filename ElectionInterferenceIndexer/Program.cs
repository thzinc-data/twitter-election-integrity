using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.Extensions.Configuration;
using Nest;

namespace ElectionInterferenceIndexer
{
    class Program
    {
        public class Settings
        {
            public Uri ElasticsearchUrl { get; set; }
            public FileType Type { get; set; }
            public string Source { get; set; }
        }
        public enum FileType
        {
            Unknown = 0,
            Users,
            Tweets,
        }
        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddCommandLine(args);

            var configuration = builder.Build();
            var settings = new Settings();
            configuration.Bind(settings);
            var elasticClient = GetElasticClient(settings.ElasticsearchUrl);

            Console.WriteLine($"Processing {settings.Source} as {settings.Type}");
            Stream stream;
            if (Uri.TryCreate(settings.Source, UriKind.Absolute, out var sourceUri))
            {
                if (sourceUri.Scheme == "file")
                {
                    stream = GetFileStream(sourceUri.AbsolutePath);
                }
                else if (sourceUri.Scheme.StartsWith("http"))
                {
                    stream = new PartialHTTPStream(sourceUri.ToString(), 1024 * 1024);
                }
                else
                {
                    throw new NotImplementedException($"Unknown scheme {sourceUri.Scheme} in URI {sourceUri}");
                }
            }
            else
            {
                stream = GetFileStream(settings.Source);
            }

            using (stream)
            {
                var header = new byte[4];
                stream.Read(header, 0, 4);
                stream.Seek(0, SeekOrigin.Begin);
                if (header.SequenceEqual(new byte[] { 0x50, 0x4B, 0x03, 0x04 }))
                {
                    using (var archive = new ZipArchive(stream, ZipArchiveMode.Read, true))
                    {
                        foreach (var entry in archive.Entries)
                        {
                            await HandleStream(elasticClient, settings.Type, entry.FullName, entry.Open());
                        }
                    }
                }
                else
                {
                    await HandleStream(elasticClient, settings.Type, settings.Source, stream);
                }
            }
        }

        private static async Task HandleStream(ElasticClient elasticClient, FileType fileType, string path, Stream stream)
        {
            Console.WriteLine($"Processing {path} as {fileType}");
            switch (fileType)
            {
                case FileType.Users:
                    {
                        await IndexManyAsync<User>(elasticClient, stream);
                        break;
                    }
                case FileType.Tweets:
                    {
                        await IndexManyAsync<Tweet>(elasticClient, stream);
                        break;
                    }
                default:
                    throw new NotImplementedException("Unknown file type");
            }
        }

        private static async Task IndexManyAsync<T>(ElasticClient elasticClient, Stream stream)
            where T : class, ITimeSeriesDocument<string>
        {
            var indexes = new HashSet<string>();
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader))
            {
                while (csv.Read())
                {
                    var document = csv.GetRecord<T>();
                    var indexName = $"{typeof(T).Name.ToLower()}-{document.Timestamp:yyyy-MM}";
                    if (!indexes.Contains(indexName))
                    {
                        var createIndexResult = await elasticClient.CreateIndexAsync(indexName, ci => ci
                            .Mappings(md => md
                                .Map<T>(m => m.AutoMap())));
                        if (createIndexResult.TryGetServerErrorReason(out var reason))
                        {
                            Console.Error.WriteLine($"Error: {reason}");
                        }
                        else if (createIndexResult.IsValid)
                        {
                            indexes.Add(indexName);
                        }
                    }

                    await elasticClient.IndexAsync(document, id => id.Index(indexName));
                }
            }
        }

        private static Stream GetFileStream(string path)
        {
            Console.WriteLine($"Using local {path}");
            if (!File.Exists(path)) throw new FileNotFoundException("The Source file could not be loaded", path);
            return File.OpenRead(path);
        }

        private static ElasticClient GetElasticClient(Uri elasticSearchUrl)
        {
            var config = new ConnectionSettings(elasticSearchUrl);
            var elasticClient = new ElasticClient(config);
            if (!elasticClient.CatHealth().IsValid) throw new InvalidOperationException($"Elasticsearch is not healthy! {elasticSearchUrl}");
            return elasticClient;
        }
    }
}
