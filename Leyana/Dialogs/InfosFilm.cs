using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AllocineAPI;
using AllocineAPI.Entyties;

namespace Leyana.Dialogs
{
    public class InfosFilm
    {
        public async System.Threading.Tasks.Task<List<string>> FilmsALAffiche()
        {
            List<String> listeFilms = new List<string>();
            try
            {
                AllocineOnDisplay OnDisplay = new AllocineOnDisplay();
                List<Movie> films = new List<Movie>();

                await OnDisplay.LoadWebsite();
                OnDisplay.LoadMovies();
                films = OnDisplay.Movies;

                for (int i = 0; i < films.Count && i < 5; i++) 
                {
                    listeFilms.Add(films[i].Title);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur: " + ex.Message);
            }

            return listeFilms;
        }
    }
}