using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlackWhiteBlog.DomainModel.Models;

namespace BlackWhiteBlog.Application
{
	public class CommandHandlerBase<TIn, TOut> : HandlerBase<TIn, TOut> where TIn: IRequest<TOut>
	{
		protected DbContext _ctx;
		
		public CommandHandlerBase(DbContext ctx)
		{
			_ctx = ctx;
		}

		protected IEnumerable<IDomainEvent> DomainEvents { get; private set; }

		public override async Task<TOut> Handle(TIn request, CancellationToken cancellationToken)
		{
			Validate(request);
			CheckPermissions(request);
			var result = await ApplyDomainLogic(request);
			RaiseDomainEvents(request);
			await _ctx.SaveChangesAsync();
			return result;
		}
	}
}
