﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Filmes.Core.Interfaces;
using Filmes.Core.ResponseModels;
using Filmes.Core.Services;
using Filmes.Infraestrutura.InfraestruturaServices;

namespace Filmes.Testes
{
    [TestClass]
    public class FilmesLancamentosTestesDeIntegracao
    {
        private static IMovieUpComing _movie;

        public FilmesLancamentosTestesDeIntegracao()
        {
            var apiSettings = new ApiSettingsService();
            var movieConsumer = new MovieApiConsumerServices(apiSettings);
            _movie = new MovieUpComingServices(apiSettings, movieConsumer, new MovieGenreServices(apiSettings, movieConsumer));
        }

        [TestMethod]
        public void Quando_Requisitar_API_Sem_Parametros_Retorna_Lista_Filmes_Lancamentos()
        {
            MovieUpComingDTO upComingMovies = _movie.GetMoviesUpComing("pt-BR", 1, "");
            Assert.IsTrue(upComingMovies.Results.Any());
        }

        [TestMethod]
        public void Quando_Requisitar_API_Sem_Parametros_Retorna_Lista_Filmes_Lancamentos_Com_Descricao_Dos_Generos()
        {
            MovieUpComingDTO upComingMovies = _movie.GetMoviesUpComing("pt-BR", 1, "");
            Assert.IsTrue(upComingMovies.Results.Any(a=>a.Genres.Any()));
        }

        [TestMethod]
        public void Quando_Requisitar_API_Com_Parametro_Page_Maior_Que_Quinhentos_Retorna_Results_Vazio()
        {
            MovieUpComingDTO upComingMovies = _movie.GetMoviesUpComing("pt-BR", 501, "");
            Assert.IsTrue(!upComingMovies.Results.Any());
        }

        [TestMethod]
        public void Quando_Requisitar_API_Com_Parametro_Page_Maior_Que_Quinhentos_Retorna_Lista_Com_Erros()
        {
            MovieUpComingDTO upComingMovies = _movie.GetMoviesUpComing("pt-BR", 501, "");
            Assert.IsTrue(upComingMovies.Errors.Errors.Any());
        }
    }
}