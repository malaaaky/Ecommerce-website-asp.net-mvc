using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMoviesServers _service;
        public MoviesController(IMoviesServers service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var allMovies = await _service.GetAllAsync(n => n.Cinema);
            return View(allMovies);
        }

        public async Task<IActionResult> Filter(string searchString)
        {
            var allMovies = await _service.GetAllAsync(n => n.Cinema);

            if(!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = allMovies.Where(n => n.Name.Contains(searchString) || n.Description.Contains(searchString)).ToList();
                return View("Index", filteredResult);
            }
            return View("Index", allMovies);
        }

        //Get: Movies Details

        public async Task<ActionResult> Details(int ID)
        {
            var movieDetails = await _service.GetMovieIdAsync(ID);
            return View(movieDetails);
        }

        //Get: Movie Create
        public async Task<IActionResult> Create()
        {
            var movieDropdownData = await _service.GetNewMovieDropdownsValues();

            ViewBag.Cinemas = new SelectList(movieDropdownData.Cinemas, "ID", "Name");
            ViewBag.Producers = new SelectList(movieDropdownData.Producers, "ID", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownData.Actors, "ID", "FullName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewMovieVM movie)
        {
            if(!ModelState.IsValid)
            {
                var movieDropdownData = await _service.GetNewMovieDropdownsValues();

                ViewBag.Cinemas = new SelectList(movieDropdownData.Cinemas, "ID", "Name");
                ViewBag.Producers = new SelectList(movieDropdownData.Producers, "ID", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownData.Actors, "ID", "FullName");
                return View(movie);
            }

            await _service.AddNewMovieAsync(movie);
            return RedirectToAction(nameof(Index));
        }

        //Get: Movie Edit
        public async Task<IActionResult> Edit(int ID)
        {
            var movieDetails = await _service.GetMovieIdAsync(ID);
            if (movieDetails == null) return View("NotFound");

            var response = new NewMovieVM()
            {
                ID = movieDetails.ID,
                Name = movieDetails.Name,
                Description = movieDetails.Description,
                Price = movieDetails.Price,
                StartDate = movieDetails.StartDate,
                EndDate = movieDetails.EndDate,
                ImageURL = movieDetails.ImageURL,
                MovieCategory = movieDetails.MovieCategory,
                CinemaId = movieDetails.CinemaId,
                ProducerId = movieDetails.ProducerId,
                ActorIds = movieDetails.Actors_Movies.Select(n => n.ActorId).ToList()
            };

            var movieDropdownData = await _service.GetNewMovieDropdownsValues();
            ViewBag.Cinemas = new SelectList(movieDropdownData.Cinemas, "ID", "Name");
            ViewBag.Producers = new SelectList(movieDropdownData.Producers, "ID", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownData.Actors, "ID", "FullName");

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int ID, NewMovieVM movie)
        {
            if (ID != movie.ID) return View("NotFound");

            if (!ModelState.IsValid)
            {
                var movieDropdownData = await _service.GetNewMovieDropdownsValues();

                ViewBag.Cinemas = new SelectList(movieDropdownData.Cinemas, "ID", "Name");
                ViewBag.Producers = new SelectList(movieDropdownData.Producers, "ID", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownData.Actors, "ID", "FullName");
                return View(movie);
            }

            await _service.UpdateMovieAsync(movie);
            return RedirectToAction(nameof(Index));
        }
    }
}
