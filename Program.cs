using NLog;
using System.Linq;
using Microsoft.EntityFrameworkCore;

// create important items
string path = Directory.GetCurrentDirectory() + "\\nlog.config";
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger(); logger.Info("Program started");
var db = new BloggingContext();
string ans = "initialValue";


do{
    Console.Write("What would you like to do?\n\t1. Display all blogs\n\t2. Add a blog\n\t3. Create a post\n\t4. Display posts\n\t'q' to quit\n\t > "); ans = Console.ReadLine();

    if(ans == "1"){
        displayBlogs();
        Console.WriteLine();
    }

    else if(ans == "2"){
        Console.Write("Enter a name for a new Blog: ");
        var name = Console.ReadLine();
        var blog = new Blog();
        blog.Name = name;  
        blog.Posts = new List<Post>();

        db.AddBlog(blog);
        logger.Info($"Blog added - {name}");
        Console.Clear();
    }    

    else if(ans == "3"){
        displayBlogs();
        Console.Write("Select a blog to add to: "); int ansNum = Convert.ToInt32(Console.ReadLine());
        var entry = db.Blogs.FirstOrDefault(p => p.BlogId == ansNum);
        Post newPost = new Post();

        Console.Write("Select a title for your post: "); newPost.Title = Console.ReadLine();
        Console.Write("What would you like the post to say?\n > "); newPost.Content = Console.ReadLine();

        entry.Posts.Add(newPost);
        Console.Clear();
    }

    else if(ans == "4"){
        Console.WriteLine();
        displayBlogs();
        Console.Write("Select a blog to view: "); int ansNum = Convert.ToInt32(Console.ReadLine());
        var entry = db.Blogs.Include("Posts").FirstOrDefault(p => p.BlogId == ansNum);

        Console.WriteLine(entry.Name + "(" + entry.Posts.Count() + " posts)\n"); 
        
        foreach(Post post in entry.Posts){
        Console.WriteLine("\t" + post.Title + "\n\t\t" + post.Content);
        }
        Console.WriteLine();
    }

} while (ans != "q");


void displayBlogs(){
    var query = db.Blogs.OrderBy(b => b.BlogId);

    Console.WriteLine("\nAll blogs in the database:");
    foreach (var item in query)
    {
        Console.Write(item.BlogId + ". ");
        Console.WriteLine(item.Name);
    }
    Console.WriteLine();
}

logger.Info("Program ended");