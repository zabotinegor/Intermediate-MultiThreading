Multithreading in .NET
Q: What is the purpose of threading in C#?
A: In C#, the System.Threading.Thread class is used for working with threads. It allows creating and accessing individual threads in a multithreaded application. The first thread to be executed in a process is called the main thread. 
When a C# program starts execution, the main thread is automatically created. The threads created using the Thread class are called the child threads of the main thread. You can access a thread using the CurrentThread property of the Thread class.

Q: What is the difference between thread and process?
A: Both processes and threads are independent sequences of execution. The typical difference is that threads (of the same process) run in a shared memory space, while processes run in separate memory spaces. 
I'm not sure what "hardware" vs "software" threads you might be referring to. Threads are an operating environment feature, rather than a CPU feature (though the CPU typically has operations that make threads efficient). 
Erlang uses the term "process" because it does not expose a shared-memory multiprogramming model. Calling them "threads" would imply that they have shared memory.

Q: What is the difference between thread, thread pool and TPL? What is the preferred way to write multithreaded and parallel code?
A: I see "multithreading" as just what the term says: using multiple threads. 
"Parallel processing" would be: splitting up a group of work among multiple threads so the work can be processed in parallel. 
Thus, parallel processing is a special case of multithreading.
Does this mean that multi-core-d and parallel programming applications impossible using prior versions of .NET?
Not at all. You could do it using the Thread class. It was just much harder to write, and much much harder to get it right. 
Do I control a multicore/parallel usage/distribution between cores in a .NET multithreaded application? 
Not really, but you don't need to. You can mess around with processor affinity for your application, but at the .NET level that's hardly ever a winning strategy.
The Task Parallel Library includes a "partitioner" concept that can be used to control the distribution of work, which is a better solution than controlling the distribution of threads over cores. 
How can I identify a core on which a thread to be run and attribute a thread to a specific core? 
You're not supposed to do this. A .NET thread doesn't necessarily correspond with an OS thread; you're at a higher level of abstraction than that. Now, the default .NET host does map threads 1-to-1, so if you want to depend on an undocumented implementation detail, then you can poke through the abstraction and use P/invoke to determine/drive your processor affinity. But as noted above, it's not useful. 
What has the .NET 4.0+ Task Parallel Library enabled that was impossible to do in previous versions of .NET? 
Nothing. But it sure has made parallel processing (and multithreading) much easier! 
Can you give me hints on how to specifically create parallel code in pre-.NET4 (in .NET3.5), taking into account that I am familiar with multi-threading development? 
First off, there's no reason to develop for that platform. None. .NET 4.5 is already out, and the last version (.NET 4.0) supports all OSes that the next older version (.NET 3.5) did.
But if you really want to, you can do simple parallel processing by spinning up Thread objects or BackgroundWorkers, or by queueing work directly to the thread pool. All of these approaches require more code (particularly around error handling) than the Task type in the TPL.

Q: Describe a flow how exceptions are handled in threads?
A: Exceptions thrown in a thread normally couldn't be caught in another thread. 
You'd better catch it in the Go() function and pass it to the main thread explicitly. However, if you just want to log all unhandled messages from all threads, you may use AppDomain.UnhandledException event or equivalent events at Application class if you are developing a WinForms or WPF app.

Q: What is the difference between foreground and background threads?
A: A managed thread is either a background thread or a foreground thread. Background threads are identical to foreground threads with one exception: a background thread does not keep the managed execution environment running. Once all foreground threads have been stopped in a managed process (where the .exe file is a managed assembly), the system stops all background threads and shuts down.

Q: What is the difference between managed and unmanaged threads?
A: Management of all threads is done through the Thread class, including threads created by the common language runtime and those created outside the runtime that enter the managed environment to execute code. The runtime monitors all the threads in its process that have ever executed code within the managed execution environment. It does not track any other threads. Threads can enter the managed execution environment through COM interop (because the runtime exposes managed objects as COM objects to the unmanaged world), the COM DllGetClassObject function, and platform invoke.

Q: What is spinning and how is it different from Blocking?
A: When a thread attempts to acquire a lock but finds it busy, it must choose between spinning, which means repeatedly attempting to acquire the lock in the hope that it will become free, and blocking, which means suspending its execution and relinquishing its processor to some other thread.

Q: What is Mutex? How is it different from other synchronization primitives?
A: The System.Threading.Mutex class, like Monitor, grants exclusive access to a shared resource. Use one of the Mutex.WaitOne method overloads to request the ownership of a mutex. Like Monitor, Mutex has thread affinity and the thread that acquired a mutex must release it by calling the Mutex.ReleaseMutex method. 
Unlike Monitor, the Mutex class can be used for inter-process synchronization. To do that, use a named mutex, which is visible throughout the operating system. To create a named mutex instance, use a Mutex constructor that specifies a name. You can also call the Mutex.OpenExisting method to open an existing named system mutex.

Q: What ways/options to avoid deadlocks and race conditions and other threading issues do you know ?
A:  Race Conditions
Synchronizing Access to Data
Deadlocks

Q: What is the difference between lock and monitor?
A: Both Monitor and lock provide a mechanism that synchronizes access to objects. Lock is the shortcut for Monitor.Enter with try and finally. 
Lock is a shortcut and it's the option for the basic usage.

Q: How to write thread safe code?
A: 

Q: Name potential pitfalls in Task Parallelism?
A:Do Not Assume That Parallel Is Always Faster.
Avoid Writing to Shared Memory Locations.
Avoid Over-Parallelization.
Avoid Calls to Non-Thread-Safe Methods.
Limit Calls to Thread-Safe Methods.
Be Aware of Thread Affinity Issues.
Use Caution When Waiting in Delegates That Are Called by Parallel.Invoke. 

Q: What is the difference between attached and detached Child Tasks?
A: A child task can be either detached or attached. A detached child task is a task that executes independently of its parent. An attached child task is a nested task that is created with the TaskCreationOptions.AttachedToParent option whose parent does not explicitly or by default prohibit it from being attached. A task may create any number of attached and detached child tasks, limited only by system resources.
