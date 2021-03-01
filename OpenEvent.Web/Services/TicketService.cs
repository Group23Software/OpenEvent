using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.Ticket;

namespace OpenEvent.Web.Services
{
    public interface ITicketService
    {
        Task BuyTicket();
        Task<List<TicketViewModel>> GetAllUsersTickets(Guid id);
        Task VerifyTicket(Guid id);

        Task<TicketDetailModel> Get(Guid id);
    }

    public class TicketService : ITicketService
    {
        private readonly ILogger<TicketService> Logger;
        private readonly ApplicationContext ApplicationContext;
        private readonly IMapper Mapper;

        public TicketService(ApplicationContext context, ILogger<TicketService> logger, IMapper mapper)
        {
            Logger = logger;
            Mapper = mapper;
            ApplicationContext = context;
        }

        public Task BuyTicket()
        {
            throw new NotImplementedException();
        }

        public async Task<List<TicketViewModel>> GetAllUsersTickets(Guid id)
        {
            var user = await ApplicationContext.Users.Include(x => x.Tickets).ThenInclude(x => x.Event).AsSplitQuery().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (user == null) throw new UserNotFoundException();

            return user.Tickets.Select(x => Mapper.Map<TicketViewModel>(x)).ToList();
        }

        public Task VerifyTicket(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<TicketDetailModel> Get(Guid id)
        {
            var ticket = await ApplicationContext.Tickets.FirstOrDefaultAsync(x => x.Id == id);

            if (ticket == null)
            {
                Logger.LogInformation("Ticket not found");
                throw new TicketNotFoundException();
            }

            return Mapper.Map<TicketDetailModel>(ticket);
        }
    }
}