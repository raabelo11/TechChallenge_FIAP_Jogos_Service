using System.Text;
using System.Text.Json;
using Jogos.Service.Application.Dtos;
using Newtonsoft.Json.Linq;


namespace Jogos.Service.Application.Utils
{
    public class ElasticClient
    {
        private readonly HttpClient httpClient;

        public ElasticClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task SalvarJogo(string nomeJogo, int idJogo)
        {
            Object objeto = new
            {
                Nome = nomeJogo,
                IdJogo = idJogo
            };
            var json = JsonSerializer.Serialize(objeto);

            var response = await httpClient.PostAsync("jogos/_doc/", new StringContent(json, Encoding.UTF8, "application/json"));

        }

        public async Task salvarPreferencia(int estudio, int idCliente)
        {
            Object objeto = new
            {
                nomeEstudio = estudio,
                idCliente = idCliente
            };
            var json = JsonSerializer.Serialize(objeto);
            var response = await httpClient.PostAsync("jogos-usuarios/_doc/", new StringContent(json, Encoding.UTF8, "application/json"));

        }
        public async Task<List<JogoDto>> ListarJogos()
        {
            var query = new JObject
            {
                ["size"] = 0,
                ["aggs"] = new JObject
                {
                    ["jogos_mais_presentes"] = new JObject
                    {
                        ["terms"] = new JObject
                        {
                            ["field"] = "IdJogo",
                            ["size"] = 10,
                            ["order"] = new JObject
                            {
                                ["_count"] = "desc"
                            }
                        },
                        ["aggs"] = new JObject
                        {
                            ["nome_do_jogo"] = new JObject
                            {
                                ["top_hits"] = new JObject
                                {
                                    ["size"] = 1,
                                    ["_source"] = new JObject
                                    {
                                        ["includes"] = new JArray("Nome")
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var json = query.ToString();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await this.httpClient.PostAsync("jogos/_search", content);
            response.EnsureSuccessStatusCode();
            var respostaTexto = await response.Content.ReadAsStringAsync();
            //var respostaJson = JObject.Parse(respostaTexto);
            JObject JObject = JObject.Parse(respostaTexto);
            var buckets =  JObject["aggregations"]["jogos_mais_presentes"]["buckets"];
            List<JogoDto> topGames = new List<JogoDto>();
            foreach (var buc in buckets)
            {
                JogoDto jogo = new JogoDto();
                jogo.Nome = (string)buc["nome_do_jogo"]["hits"]["hits"][0]["_source"]["Nome"];
                topGames.Add(jogo);
            }
            return topGames;
        }

        public async Task<int> EstudioPreferido(int idCliente)
        {
            var query = new
            {
                size = 0, // não precisamos dos documentos
                query = new
                {
                    term = new
                    {
                        idCliente = idCliente
                    }
                },
                aggs = new
                {
                    estudio_mais_frequente = new
                    {
                        terms = new
                        {
                            field = "nomeEstudio",
                            size = 10
                        }
                    }
                }
            };
            // Converte a query em string JSON
            string jsonQuery = Newtonsoft.Json.JsonConvert.SerializeObject(query);

            // Faz a requisição POST para o Elasticsearch
            var content = new StringContent(jsonQuery, Encoding.UTF8, "application/json");
            var response = await this.httpClient.PostAsync("jogos-usuarios/_search/", content);
            int estudioId = 0;

            string responseBody = await response.Content.ReadAsStringAsync();

            // Converte a resposta em JObject para acessar dinamicamente
            var jObject = JObject.Parse(responseBody);

            var buckets = jObject["aggregations"]?["estudio_mais_frequente"]?["buckets"];
            foreach (var buc in buckets)
            {
                estudioId = (int)buc["key"];
                return estudioId;
            }
            return estudioId;
        }

      
    }
}
