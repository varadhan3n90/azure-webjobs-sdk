﻿using System;
using System.IO;
using System.Threading;
using Microsoft.Azure.Jobs.Host.Runners;

namespace Microsoft.Azure.Jobs.Internals
{
    // In-memory executor. 
    class WebSitesExecuteFunction : ExecuteFunctionBase
    {
        private readonly FunctionExecutionContext _ctx;

        private readonly IConfiguration _config;

        public WebSitesExecuteFunction(IConfiguration config, FunctionExecutionContext ctx)
        {
            _config = config;
            _ctx = ctx;
        }
        protected override FunctionInvocationResult Work(FunctionInvokeRequest request, INotifyNewBlob notifyNewBlob, CancellationToken cancellationToken)
        {
            var loc = request.Location;

            Func<TextWriter, CloudBlobDescriptor, FunctionExecutionResult> fpInvokeFunc =
                (consoleOutput, parameterLog) =>
                {
                    // @@@ May need to override config to set ICall
                    return RunnerProgram.MainWorker(consoleOutput, parameterLog, request, _config, cancellationToken);
                };

            // @@@ somewhere this should be async, handle long-running functions. 
            return ExecutionBase.Work(
                notifyNewBlob,
                request,
                _ctx,
                fpInvokeFunc);
        }
    }
}