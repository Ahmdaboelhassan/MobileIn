using Data.Models;
using MobileIn.Data;


namespace Business.IRepository.Repoistory
{
	public class ApplicationUserRepo : Repository<ApplicationUser> , IApplicationUserRepo
	{
		private readonly ApplicationDbContext _context;

		public ApplicationUserRepo(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
