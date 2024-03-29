﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Movies;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(CertificationId), nameof(MovieId))]
    public class CertificationMovie
    {
        [JsonProperty("certification_id")] public int CertificationId { get; set; }
        public virtual Certification Certification { get; set; }

        [JsonProperty("movie_id")] public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        public CertificationMovie()
        {
        }

        public CertificationMovie(Certification? certification, MovieAppends? movie)
        {
            if (certification == null || movie == null) return;
            CertificationId = certification.Id;
            MovieId = movie.Id;
        }
    }
}