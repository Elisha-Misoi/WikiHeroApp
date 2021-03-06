﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WikiHero.Models
{
   public class Character
    {


        [JsonProperty("site_detail_url")]
        public string SiteDetailUrl { get; set; }

        [JsonProperty("character_enemies")]
        public IList<Character> CharacterEnemies { get; set; }

        [JsonProperty("creators")]
        public IList<Creator> Creators { get; set; }

        [JsonProperty("date_added")]
        public string DateAdded { get; set; }

        [JsonProperty("date_last_updated")]
        public string DateLastUpdated { get; set; }
        public DateTime LastUpdate { get=>DateTime.Parse(DateLastUpdated);}

        [JsonProperty("deck")]
        public string Deck { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("gender")]
        public int Gender { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("movies")]
        public IList<Movie> Movies { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("publisher")]
        public Publisher Publisher { get; set; }

        [JsonProperty("real_name")]
        public string RealName { get; set; }

        [JsonProperty("teams")]
        public IList<Team> Teams { get; set; }

        [JsonProperty("volume_credits")]
        public IList<Volume> VolumeCredits { get; set; }

        [JsonProperty("issue_credits")]
        public IList<Comic> Issues { get; set; }
    }
    public class Creator
    {

        [JsonProperty("name")]
        public string Name { get; set; }
    }
    public class ResultCharacter
    {
        [JsonProperty("results")]
        public Character Character { get; set; }
    }
    public class ResultCharacters
    {

        [JsonProperty("results")]
        public IList<Character> Characters { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }
}
