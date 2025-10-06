namespace Jogos.Service.Application.Dtos
{

    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class Root
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        public int Took { get; set; }
        public bool TimedOut { get; set; }
        public Shards Shards { get; set; }
        public Hits Hits { get; set; }
        public Aggregations Aggregations { get; set; }
    }

    public class Shards
    {
        public int Total { get; set; }
        public int Successful { get; set; }
        public int Skipped { get; set; }
        public int Failed { get; set; }
    }

    public class Total
    {
        public int Value { get; set; }
        public string Relation { get; set; }
    }

    public class Aggregations
    {
        [JsonProperty("jogos_mais_presentes")]
        public JogosMaisPresentes JogosMaisPresentes { get; set; }
    }

    public class JogosMaisPresentes
    {
        [JsonProperty("doc_count_error_upper_bound")]
        public int DocCountErrorUpperBound { get; set; }
        [JsonProperty("sum_other_doc_count")]
        public int SumOtherDocCount { get; set; }
        public List<Bucket> Buckets { get; set; }
    }

    public class Bucket
    {
        public int Key { get; set; }
        public int DocCount { get; set; }
        [JsonProperty("nome_do_jogo")]
        public NomeDoJogo NomeDoJogo { get; set; }
    }

    public class NomeDoJogo
    {
        public Hits Hits { get; set; }
    }

    public class Hits
    {
        public Total Total { get; set; }
        public object MaxScore { get; set; }
        public List<HitItem> HitsList { get; set; }
    }

    public class HitItem
    {
        public string Index { get; set; }
        public string Id { get; set; }
        public double Score { get; set; }
        public Source Source { get; set; }
    }

    public class Source
    {
        public string Nome { get; set; }
    }

}
