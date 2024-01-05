using Data.DataContext;
using Data.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Presentation.Models.ViewModels.Flight;
using Presentation.Models.ViewModels.Ticket;

namespace Presentation.Controllers
{
    public class TicketController : Controller
    {
        private SeatDbRepository _seatRepository;
        private TicketDbRepository _ticketRepository;
        public TicketController(SeatDbRepository seatRepository, TicketDbRepository ticketRepository)
        {
            _seatRepository = seatRepository;
            _ticketRepository = ticketRepository;
        }

        public IActionResult ListTickets()
        {
            try
            {
                IQueryable<Ticket> list = _ticketRepository.getTickets();

                var output = from p in list
                             select new ListTicketViewModel()
                             {
                                 Id = p.Id,
                                 SeatFk = p.Id,
                                 PassportImage = p.PassportImage,
                                 PricePaid = p.PricePaid,
                                 Cancelled = p.Cancelled,
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
            int maxRowLength = _seatRepository.getMaxRowsFromAFlight(chosenFlightId) + 1;
            int maxColLength = _seatRepository.getMaxColumnsFromAFlight(chosenFlightId) + 1;

            List<Seat> seatingList = _seatRepository.GetAllTheSeatsFromAFlight(chosenFlightId).ToList();



            return View(new BookTicketViewModel()
            {
                chosenFlight = chosenFlightId,
                seatingList = seatingList,
                maxColLength = maxColLength,
                maxRowLength = maxRowLength,

            });;

        }



        //{057f3b69-a789-4463-5245-08dc0d58919c}
        [HttpPost]
        public IActionResult BookTicket(BookTicketViewModel myModel, [FromServices] IWebHostEnvironment host)
        {
            try
            {
                List<Seat> listOfSeats = _seatRepository.GetAllTheSeatsFromAFlight(myModel.chosenFlight).ToList(); ;
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
                    Cancelled = true
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

            if (ticketToCancel.Id != null)
            {
                _ticketRepository.cancelTicket(ticketToCancel.Id);
                TempData["message"] = "Ticket canceled successfully";
                return RedirectToAction("ListTIckets", "Ticket");
            }
            else
            {
                TempData["error"] = "Error while canceling the ticket!";
                return RedirectToAction("ListTIckets", "Ticket");
            }
        }



    }
}
