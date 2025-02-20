using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Api.Infrastructure.Services.LoggedUser;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Checkouts
{
    public class RegisterBookCheckoutUseCase
    {
        private const int MAX_LOAN_DAYS = 7;

        private readonly LoggedUserService _loggedUser;

        public RegisterBookCheckoutUseCase(LoggedUserService loggedUser)
        {
            _loggedUser = loggedUser;
        }

        public void Execute(Guid bookId)
        {
            var dbContext = new TechLibraryDbContext();

            Validate(dbContext, bookId);

            var user = _loggedUser.User(dbContext);

            var entity = new Domain.Entities.Checkout();

            dbContext.Checkouts.Add(new Domain.Entities.Checkout
            {
                UserId = user.Id,
                BookId = bookId,
                CheckoutDate = DateTime.UtcNow.AddDays(MAX_LOAN_DAYS)
            });

            dbContext.SaveChanges();
        }

        private void Validate(TechLibraryDbContext dbContext, Guid bookId) 
        {
            var book = dbContext.Books.FirstOrDefault(book => book.Id == bookId);

            if (book is null)
                throw new NotFoundException("Book not found.");

            var amountBookNotReturned = dbContext
                .Checkouts
                .Count(checkout => checkout.BookId == bookId && checkout.ReturnedDate == null);

            if (amountBookNotReturned == book.Amount)
                throw new NotFoundException("Book is not available for loan.");
        }
    }
}
