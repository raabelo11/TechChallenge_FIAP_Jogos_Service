namespace Jogos.Service.Application.Dtos
{
    public class JogosResponse
    {
        public bool Ok { get; set; }
        public Object? data { get; set; }
        public string[]? Errors { get; set; }

    }
}
