using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BlackWhiteBlog.Application
{
	public abstract class RequestHandlerBase<TIn, TOut> : HandlerBase<TIn, TOut> where TIn : IRequest<TOut>
	{
		public abstract TOut Map(object obj);

		public abstract Task<TOut> ReadModel(TIn request);

		public override async Task<TOut> Handle(TIn request, CancellationToken cancellationToken)
		{
			Validate(request);
			CheckPermissions(request);
			return await ReadModel(request);
		}
	}
}
