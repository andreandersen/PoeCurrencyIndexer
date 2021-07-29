using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

namespace PoeCurrencyIndexer.Indexer.Common
{
    public abstract class HostedService : IHostedService, IDisposable
    {
        private Task? _executingTask;
        private readonly CancellationTokenSource _cts = new();

        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _executingTask = ExecuteAsync(_cts.Token);
            return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;
        }

        public async virtual Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null)
                return;

            try
            {
                _cts.Cancel();
            }
            finally
            {
                var grace = Task.Delay(Timeout.Infinite, cancellationToken);
                _ = await Task.WhenAny(_executingTask, grace);
            }
        }

        public virtual void Dispose()
        {
            _cts.Cancel();
            GC.SuppressFinalize(this);
        }
    }
}
