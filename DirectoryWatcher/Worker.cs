﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace DirectoryWatcher
{
    internal abstract class Worker
    {
        public bool Active { get; protected set; }
        protected Debug Debug;
        protected Service Service;
        protected CountdownEvent Countdown;

        public Worker(Service service, string name)
        {
            Service = service;
            Debug = new Debug(name);
        }

        public void Start(CountdownEvent countdown, CancellationToken token)
        {
            Debug.WriteLine("Attempting to start worker...");

            if (Active)
                throw new InvalidOperationException("Worker is already active");

            Debug.WriteLine("Starting worker...");

            Active = true;
            Countdown = countdown;

            Task.Run(() => Wrap(token), token);
        }

        private void Wrap(CancellationToken token)
        {
            try
            {
                Debug.WriteLine("Attempting to run worker...");
                Run(token);
                Debug.WriteLine("Running worker finished normally...");
            }

            //catch operation cancellation
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Worker cancelled");
            }

            //catch unhandled exceptions
            catch (Exception ex)
            {
                Debug.WriteLine("Uncaught worker exception: `{0}`", ex.Message);
                Service.Shutdown();
            }
            Debug.WriteLine("Running worker finished...");
            Stop();
        }

        protected abstract void Run(CancellationToken token);

        public void Stop()
        {
            Debug.WriteLine("Attempting to stop worker...");

            if (!Active)
                throw new InvalidOperationException("Worker is already inactive");

            Debug.WriteLine("Stopping worker...");
            Active = false;
            Countdown.Signal();
        }
    }
}