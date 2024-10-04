namespace AnotherService.Repositories;

public class AccountRepository
{
    private readonly Context _context;

    public AccountRepository(
        Context context
        )
    {
        _context = context;
    }

    public async Task<bool> TryPayAsync(int orderCost)
    {
        var account = _context.Accounts.FirstOrDefault();
        if (account == null || account.MoneyCount < orderCost)
        {
            return false;
        }

        account.MoneyCount -= orderCost;
        await _context.SaveChangesAsync();
        return true;
    }
}