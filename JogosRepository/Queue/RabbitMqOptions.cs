using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Jogos.Service.Infrastructure.Queue
{
    public static class RabbitMqOptions
    {
        public static string Host { get; set; }
        public static string Username { get; set; }
        public static string Password { get; set; }

        public static void Load(IConfiguration configuration)
        {
            Host = configuration["RabbitMq:Host"];
            Username = configuration["RabbitMq:UserName"];
            Password = configuration["RabbitMq:Password"];
        }
    }
}
