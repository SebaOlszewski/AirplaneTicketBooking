using Data.DataContext;
using Data.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Presentation.Models.ViewModels;
using Presentation.Models.ViewModels.Tickets;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace Presentation.Controllers
{
    public class TicketsController : Controller
    {
        private ITickets _ticketRepository;
        private IFlights _flightsRepository;
        public TicketsController(ITickets ticketRepository, IFlights flightsRepository)
        {
            _ticketRepository = ticketRepository;
            _flightsRepository = flightsRepository;
        }

        public IActionResult ListTickets()
        {
            IQueryable<Ticket> list = _ticketRepository.GetTickets().OrderBy(x => x.Id);
            var output = from p in list
                         select new ListTicketsViewModel()
                         {
                             Id = p.Id,
                             Row = p.Row,
                             Column = p.Column,
                             FlightFK = p.FlightFK,
                             Passport = p.PassportImage,
                             PricePaid = p.PricePaid,
                             Cancelled = p.Cancelled,
                         };
            return View(output);
        }

        [HttpGet]
        public IActionResult Book()
        {
            BookTicketViewModel myModel = new BookTicketViewModel(_flightsRepository);
            return View(myModel);
        }



        [HttpPost]
        public IActionResult Book(BookTicketViewModel myModel, BookTicketViewModel _flightsRepository, [FromServices] IWebHostEnvironment host)
        {
            string photoPath = "";



            try
            {
                //creating id name for a photo
                string photoName = Guid.NewGuid() + System.IO.Path.GetExtension(myModel.PassportImage.FileName);

                //creating an absolute path for not using database as the storage for the images but a folder on a server


                string absolutePassportPhotoPath = host.WebRootPath + @"\passports\" + photoName;

                photoPath = @"/passports/" + photoName;

                //saving the image in the folder
                using (FileStream fs = new FileStream(absolutePassportPhotoPath, FileMode.CreateNew))
                {
                    myModel.PassportImage.CopyTo(fs);
                    fs.Flush();
                    fs.Close();
                }

                _ticketRepository.Book(new Ticket()
                {
                    FlightFK = myModel.FlightFK,
                    Row = myModel.Row,
                    Column = myModel.Column,
                    PassportImage = photoPath,
                    PricePaid = myModel.PricePaid,
                    Owner = User.Identity.Name
                });
                return RedirectToAction("ListTickets", "Tickets");
            }
            catch (Exception ex)
            {
                myModel.Flights = _flightsRepository.Flights;
                //TempData["message"] = "Product saved successfully!";
                return RedirectToAction("ListTickets", "Tickets");
            }
        }

        public IActionResult Cancel(Guid Id)
        {
            try
            {
                _ticketRepository.Cancel(Id);

            }
            catch (Exception ex)
            {
                TempData["error"] = "Ticket was not canceled";
            }

            return RedirectToAction("ListTickets", "Tickets");
        }

        public IActionResult DeleteTicket(Guid Id, [FromServices] IWebHostEnvironment host)
        {
            try
            {
                string photoPath = "";
                var oldImagePath = _ticketRepository.GetTickets(Id).PassportImage;
                var absolutePassportPhotoPathToDelete = host.WebRootPath + @"\passports\" + System.IO.Path.GetFileName(oldImagePath);

                System.IO.File.Delete(absolutePassportPhotoPathToDelete);

                _ticketRepository.Cancel(Id);
                TempData["message"] = "Product deleted successfully";
                _ticketRepository.DeleteTicket(Id);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Product was not deleted";
            }
            return RedirectToAction("ListTickets", "Tickets");
        }

        //opens the page with the existing ticket data
        [HttpGet]
        public IActionResult Edit(Guid Id)
        {
            EditTicketViewModel myModel = new EditTicketViewModel(_flightsRepository);

            var originalTicket = _ticketRepository.GetTickets(Id);

            myModel.Id = Id;
            myModel.Row = originalTicket.Row;
            myModel.Column = originalTicket.Column;
            myModel.FlightFK = originalTicket.FlightFK;
            myModel.Image = originalTicket.PassportImage;
            myModel.PricePaid = originalTicket.PricePaid;
            myModel.Cancelled = originalTicket.Cancelled;

            return View(myModel);
        }


        [HttpPost]
        public IActionResult Edit(EditTicketViewModel myModel, [FromServices] IWebHostEnvironment host)
        {
            string photoPath = "";

            try
            {

                if (_ticketRepository.GetTickets(myModel.Id) == null)
                {
                    TempData["error"] = "Product does not exist!";
                    return RedirectToAction("ListTickets");
                }
                else
                {

                    if (myModel.PassportImage != null)
                    {
                        //creating id name for a photo
                        string photoName = Guid.NewGuid() + System.IO.Path.GetExtension(myModel.PassportImage.FileName);

                        //creating an absolute path for not using database as the storage for the images but a folder on a server


                        string absolutePassportPhotoPath = host.WebRootPath + @"\passports\" + photoName;

                        photoPath = @"/passports/" + photoName;

                        //saving the image in the folder
                        using (FileStream fs = new FileStream(absolutePassportPhotoPath, FileMode.CreateNew))
                        {
                            myModel.PassportImage.CopyTo(fs);
                            fs.Flush();
                            fs.Close();
                        }
                        var oldImagePath = _ticketRepository.GetTickets(myModel.Id).PassportImage;
                        var absolutePassportPhotoPathToDelete = host.WebRootPath + @"\passports\" + System.IO.Path.GetFileName(oldImagePath);

                        System.IO.File.Delete(absolutePassportPhotoPathToDelete);
                    }
                    else
                    {
                        photoPath = _ticketRepository.GetTickets(myModel.Id).PassportImage;
                    }

                    _ticketRepository.updateTicket(new Ticket()
                    {
                        Id = myModel.Id,
                        FlightFK = myModel.FlightFK,
                        Row = myModel.Row,
                        Column = myModel.Column,
                        PassportImage = photoPath,
                        PricePaid = myModel.PricePaid,


                    });


                    return RedirectToAction("ListTickets");
                }

            }
            catch (Exception ex)
            {
                myModel.Flights = _flightsRepository.GetFlights();
                //TempData["message"] = "Product saved successfully!";
                return RedirectToAction("ListTickets", "Tickets");
            }


        }

        public IActionResult TicketDetails(Guid Id)
        {
            var chosenTicket = _ticketRepository.GetTickets(Id);
            //var chosenFlight = _flightsRepository.GetFlights().Where(x => x.Id == (_ticketRepository.GetTickets().Where(x => x.FlightFK == Id)));
            if (chosenTicket == null)
            {
                //add information that flight was not found
                return RedirectToAction("ListTickets", "Tickets");
            }
            else
            {

                ListTicketsViewModel flightModel = new ListTicketsViewModel()
                {
                    Id = chosenTicket.Id,
                    Row = chosenTicket.Row,
                    Column = chosenTicket.Column,
                    FlightFK = chosenTicket.FlightFK,
                    CountryFrom = _flightsRepository.getCountryFrom(chosenTicket.FlightFK),
                    CountryTo = _flightsRepository.getCountryTo(chosenTicket.FlightFK),
                    Passport = chosenTicket.PassportImage,
                    PricePaid = chosenTicket.PricePaid,
                    Cancelled = chosenTicket.Cancelled
                };
                return View(flightModel);
            }
        }



    }
}