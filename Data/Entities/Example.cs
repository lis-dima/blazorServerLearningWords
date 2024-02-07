﻿using lewBlazorServer.Data.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace lewBlazorServer.Data.Entities
{
    public class Example : IWordChildEntity
    {
        public int Id { get; set; }
        public Language Language { get; set; } = Language.En;

        public string Value { get; set; }
        public int WordId { get; set; }
        [JsonIgnore]
        public Word Word { get; set; }
        [NotMapped]
        public EntityType WordChildType { get; set; } = EntityType.Example;
    }
}
