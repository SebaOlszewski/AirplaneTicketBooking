using Data.DataContext;
using Data.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Presentation.Models.ViewModels.Flight;
using Presentation.Models.ViewModels.Ticket;

namespace Presentation.Controllers
{
    public class TicketController : Controller
    {
        private ISeatInterface _seatRepository;
        private ITicketInterface _ticketRepository;
        private FlightDbRepository _flightRepository;
        public TicketController(ISeatInterface seatRepository, ITicketInterface ticketRepository, FlightDbRepository flightRepository)
        {
            _seatRepository = seatRepository;
            _ticketRepository = ticketRepository;
            _flightRepository = flightRepository;
        }

        public IActionResult ListTickets()
        {
            try
            {
                IQueryable<Ticket> list = _ticketRepository.getTickets().Where(x => x.Owner == User.Identity.Name);
                if(list.Count() == 0)
                {
                    TempData["error"] = "You have no tickets - it's time to go somewhere!";
                    return RedirectToAction("ListFLights", "Flight");
                }
                var output = from p in list
                             select new ListTicketViewModel()
                             {
                                 Id = p.Id,
                                 PassportImage = p.PassportImage,
                                 PricePaid = p.PricePaid,
                                 Cancelled = p.Cancelled,
                                 SeatFk = p.SeatFk,
                                 CountryFrom = _flightRepository.getCountryFrom(_seatRepository.GetSeat(p.SeatFk).FlightFk),
                                 CountryTo = _flightRepository.getCountryTo(_seatRepository.GetSeat(p.SeatFk).FlightFk),

                             };

                return View(output);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error while loading the flight data!";
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        public IActionResult BookTicket(Guid chosenFlightId)
        {
            var chosenFlight = _flightRepository.GetFlight(chosenFlightId);
            int maxRowLength = _seatRepository.getMaxRowsFromAFlight(chosenFlightId) + 1;
            int maxColLength = _seatRepository.getMaxColumnsFromAFlight(chosenFlightId) + 1;

            List<Seat> seatingList = _seatRepository.GetAllTheSeatsFromAFlight(chosenFlightId).ToList();



            return View(new BookTicketViewModel()
            {
                chosenFlight = chosenFlightId,
                seatingList = seatingList,
                maxColLength = maxColLength,
                maxRowLength = maxRowLength,
                PricePaid = chosenFlight.WholesalePrice + chosenFlight.CommissionRate,

            });;

        }
        [HttpPost]
        public IActionResult BookTicket(BookTicketViewModel myModel, [FromServices] IWebHostEnvironment host)
        {
            try
            {
                List<Seat> listOfSeats = _seatRepository.GetAllTheSeatsFromAFlight(myModel.chosenFlight).ToList();

                var chosenFlight = _flightRepository.GetFlight(myModel.chosenFlight);
                if(chosenFlight != null)
                {
                    DateTime currentDateTime = DateTime.Now;
                    if(chosenFlight.DepartureDate < currentDateTime)
                    {
                        myModel.seatingList = _seatRepository.GetAllTheSeatsFromAFlight(myModel.chosenFlight).ToList();
                        myModel.maxRowLength = _seatRepository.getMaxColumnsFromAFlight(myModel.chosenFlight) + 1;
                        myModel.maxColLength = _seatRepository.getMaxColumnsFromAFlight(myModel.chosenFlight) + 1;
                        TempData["error"] = "Flight had already departured! You cannot book a seat for this plane anymore";
                        return View(myModel);
                    }
                }
                
                //var chosenSeat = _seatRepository.GetSeat(myModel.SeatFk);
                

                if(listOfSeats.Any(x => x.Id == myModel.SeatFk) != true)
                {
                    myModel.seatingList = _seatRepository.GetAllTheSeatsFromAFlight(myModel.chosenFlight).ToList();
                    myModel.maxRowLength = _seatRepository.getMaxColumnsFromAFlight(myModel.chosenFlight) + 1;
                    myModel.maxColLength = _seatRepository.getMaxColumnsFromAFlight(myModel.chosenFlight) + 1;
                    TempData["error"] = "Seat does not exist!";
                    return View(myModel);
                }

                if (_seatRepository.isSeatTaken(myModel.SeatFk) == true)
                {
                    myModel.seatingList = _seatRepository.GetAllTheSeatsFromAFlight(myModel.chosenFlight).ToList();
                    myModel.maxRowLength = _seatRepository.getMaxColumnsFromAFlight(myModel.chosenFlight)+1;
                    myModel.maxColLength = _seatRepository.getMaxColumnsFromAFlight(myModel.chosenFlight)+1;
                    TempData["error"] = "The seat is taken!";
                    return View(myModel);
                }else
                {
                    _seatRepository.takeSeat(myModel.SeatFk);
                }

                string photoPath = "";
                ModelState.Remove("Seats");
                ModelState.Remove("seatingList");
                if (ModelState.IsValid == false)
                {
                    //myModel.Seats = _seatRepository.GetSeats();
                    myModel.seatingList = _seatRepository.GetAllTheSeatsFromAFlight(myModel.chosenFlight).ToList();
                    myModel.maxRowLength = _seatRepository.getMaxColumnsFromAFlight(myModel.chosenFlight) + 1;
                    myModel.maxColLength = _seatRepository.getMaxColumnsFromAFlight(myModel.chosenFlight) + 1;
                    TempData["error"] = "Please upload passport photo!";
                    return View(myModel);
                }


            
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

                _ticketRepository.book(new Ticket()
                {
                    SeatFk = myModel.SeatFk,
                    PassportImage = photoPath,
                    PricePaid = myModel.PricePaid,
                    Owner = myModel.Owner,
                    Cancelled = false
                }) ;

                
                TempData["message"] = "Ticket saved successfully!";
                return RedirectToAction("ListFlights", "Flight");
            }
            catch (Exception ex)
            {
                myModel.seatingList = _seatRepository.GetAllTheSeatsFromAFlight(myModel.chosenFlight).ToList();
                myModel.maxRowLength = _seatRepository.getMaxColumnsFromAFlight(myModel.chosenFlight) + 1;
                myModel.maxColLength = _seatRepository.getMaxColumnsFromAFlight(myModel.chosenFlight) + 1;
                TempData["error"] = "Error with ticket booking!";
                return View(myModel);
            }
        }


        public IActionResult CancelTicket(Guid chosenTicketId)
        {

            var ticketToCancel = _ticketRepository.getTicket(chosenTicketId);
            //ticketToCancel.Seat = _seatRepository.GetSeats().Where(x => x.FlightFk == chosenTicketId);
            //var seatToFree = _seatRepository.GetSeat(ticketToCancel.Seat.Id);

            if (ticketToCancel.Id != null)
            {
                
                _ticketRepository.cancelTicket(ticketToCancel.Id);
                _seatRepository.takeSeat(ticketToCancel.SeatFk);

                TempData["message"] = "Ticket canceled successfully";
                return RedirectToAction("ListTIckets", "Ticket");
            }
            else
            {
                TempData["error"] = "Error while canceling the ticket!";
                return RedirectToAction("ListTIckets", "Ticket");
            }
        }

        public IActionResult DeleteTicket(Guid Id, [FromServices] IWebHostEnvironment host)
        {
            try
            {
                string photoPath = "";
                var chosenTicket = _ticketRepository.getTicket(Id);
                var oldImagePath = chosenTicket.PassportImage;
                var absolutePassportPhotoPathToDelete = host.WebRootPath + @"\passports\" + System.IO.Path.GetFileName(oldImagePath);

                System.IO.File.Delete(absolutePassportPhotoPathToDelete);

                _ticketRepository.cancelTicket(Id);
                _seatRepository.takeSeat(chosenTicket.SeatFk);
                TempData["message"] = "Product deleted successfully";
                _ticketRepository.DeleteTicket(Id);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Product was not deleted";
            }
            return RedirectToAction("ListTickets", "Ticket");
        }



    }
}
