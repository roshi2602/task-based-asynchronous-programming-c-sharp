using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


//A task in C# is used to implement Task-based Asynchronous Programming 
//The Task object is typically executed asynchronously on a thread pool thread rather than synchronously on the main thread of the application


//Thread.CurrentThread Property in C#
//A Thread class is responsible for creating and managing a thread in multi-thread programming.
//It provides a property known as CurrentThread to check the current running thread

//ManagedthreadId
//A Thread class is responsible for creating and managing a thread in multi-thread programming.
//It provides a property known as ManagedThreadId
//to check the unique identifier for the current managed thread.

//Thread Pooling
//A Thread pool in C# is a collection of threads which can be used to perform a number of task in the background.
//Once a thread completes its task, then again it sent to the thread, so that it can be reused
//Thread pooling is the process of creating a collection of threads during the initialization of a multithreaded application, 
//and then reusing those threads for new tasks as and when required, instead of creating new threads.

class Program
        {

    public static void Main()
    {
        //---------------------------------------------------------------------
        //Task using start()

        //go to Display method first

        Console.WriteLine($"main thread-started{Thread.CurrentThread.ManagedThreadId}");
        //create object
        Task t1 = new Task(Display); //using display method
        t1.Start();
        Console.WriteLine($"main thread-completed{Thread.CurrentThread.ManagedThreadId}");
        Console.ReadLine();
        //now t1 will create new child class to execute functionality asynchronously on thread pool thread


        //----------------------------------------------------------
        //Task using factory property

        //go to Display method first

        Console.WriteLine($"main thread-started{Thread.CurrentThread.ManagedThreadId}");
        //create object
        Task t2 = Task.Factory.StartNew(Display); //using display method
        //Task.Factory.StartNew was the primary method for scheduling a new task.
        //it gives you the opportunity to define a lot of useful things about the thread you want to create
        //we created new thread dedicated to this task and would destroy once this is complete, to achieve this we use Task.Factory.StartNew

        Console.WriteLine($"main thread-completed{Thread.CurrentThread.ManagedThreadId}");
        Console.ReadLine();

        //----------------------------------------------------------------------
        //Task using Run method
        // The Run method allows you to create and execute a task in a single method call
        //and is a simpler alternative to the StartNew method.

        Console.WriteLine($"main thread-started{Thread.CurrentThread.ManagedThreadId}");

        //NOW call task.Run and invoke Display method
        //We pass a lambda expression to Task.Run that calls the Display() method
        Task t3 = Task.Run(() => { Display(); });

        Console.WriteLine($"main thread-completed{Thread.CurrentThread.ManagedThreadId}");
        Console.ReadLine();

        //----------------------------------------------------------------------
        //Task using wait()
        //The Wait method of Task class will block the execution of other threads until the assigned task has completed its execution.
        //Waits for the Task to complete execution.

        Task t4 = Task.Run(() => { Display(); });

        //apply task.wait
        t4.Wait();
        Console.WriteLine($"main thread-completed{Thread.CurrentThread.ManagedThreadId}");
        Console.ReadLine();
        //----------------------------------------------------------------------

        //Task return value 
        // Task<T>. Using this Task<T> class we can return data or value from a task.
        // In Task<T>, T represents the data type that you want to returns as a result of the task.

        //Create method Cal
        Console.WriteLine($"main thread started");

        //format
        Task<int> taskt = Task.Run(() =>    //as return type is int , we need to use Task<int>
        {
            return Cal(10);
        });
        Console.WriteLine($"sum{taskt.Result}");  // Result property of the Task object blocks the calling thread until the task finishes its work.

        Console.WriteLine($"main thread completed");
        Console.ReadLine();
        //----------------------------------------------------------------
        //Returning Complex Type Value From a task

        //use class Employee
        Console.WriteLine($"main task started");

        //syntax for Complex type
        //using Task<>.Factory.StartNew
        //use lambda function

        Task<Employee> task = Task<Employee>.Factory.StartNew(() =>       //returning value
       {
           //create object of employee
           Employee ee = new Employee()
           {
               Ename = "priya",
               Roll = 20,
               Gender = "female"
           };
           return ee;           //returning object
       });
        //Task.Result-    The Result property blocks the calling thread until the task finishes.

        //creating object for Employee and assigning it to task.Result
        Employee emp = task.Result;
        Console.WriteLine($"{emp.Ename}, {emp.Roll}, {emp.Gender}");
        Console.WriteLine($"main thread completed");
        Console.ReadLine();

        /*output = main task started
        priya, 20, female
        main thread completed*/
        //-------------------------------------------------------------


        //chaining tasks by using Continuation()
        // While working with asynchronous programming, it is very common to invoke one asynchronous operation from another asynchronous operation passing the data once it completes its execution.
        //This is called as continuations
        // continuation task is  as an asynchronous task which is going to be invoked by another task (i.e. known as the antecedent).


        //create task returing string and passing it in lambda using Task.Run()  

        Task<string> taskr = Task.Run(() =>                                //task1 is antescent task
        {
            return 10;
        }).ContinueWith((antecedent) =>            //chaining tasks
        {
            return $"{ antecedent.Result}";
        });

        Console.WriteLine(taskr.Result);
        Console.ReadLine();
        //output = 10
        //----------------------------------------------------------------------
    }



    //----------------------------------------------------------------------
    //now create 1 static method
    static void Display()
    {
        Console.WriteLine($"child thread-started{Thread.CurrentThread.ManagedThreadId}");
        //   A thread's ManagedThreadId property value serves to uniquely identify that thread within its process.        
        //apply for loop
        for (int i = 0; i <= 5; i++)
        {
            Console.WriteLine($"{i}");
        }
        Console.WriteLine($"child thread-completed{Thread.CurrentThread.ManagedThreadId}");
    }

    //------------------------------------------------------------
    //create method for task return type
    static int Cal(int n)
    {
        int sum = 0;
        for(int i = 0; i <= n; i++)
        {
            sum += i;
        }
        return sum;
    }
}
   //-------------------------------------------------------
class Employee
{
    public string Ename { get; set; }
    public int Roll { get; set; }
    public string Gender { get; set; }
}