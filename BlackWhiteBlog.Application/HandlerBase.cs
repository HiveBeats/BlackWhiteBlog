using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlackWhiteBlog.Application
{
	public abstract class HandlerBase<TIn, TOut> : IRequestHandler<TIn, TOut>
		where TIn: IRequest<TOut>
	{
		protected virtual void Validate(TIn request)
		{

		}

		protected virtual void CheckPermissions(TIn request)
		{

		}

		protected virtual Task<TOut> ApplyDomainLogic(TIn request)
		{
			throw new NotImplementedException();
		}

		protected virtual void RaiseDomainEvents(TIn request)
		{

		}

		public abstract Task<TOut> Handle(TIn request, CancellationToken cancellationToken);
	}
}
