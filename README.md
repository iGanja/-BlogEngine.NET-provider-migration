# -BlogEngine.NET-provider-migration
A simple provider migration tool for BlogEngine.NET

This tool was first created by Al Nyveldt (https://twitter.com/razorant) for BlogEngine.NET 1.45 "or greater" to migrate the 
original XML file based data store to a relational data store like SQLite, MySQL or Microsoft SQL Server. See the original post
here: http://nyveldt.com/blog/page/BlogEngineNET-Provider-Migration, or read on!

The lastest (and final) version of BlogEngine.NET (3.3.5) incorporates a Multi-Blog feature the original tool did not account for,
so I have updated the original tool to account for this.

USE AT YOUR OWN RISK!<br>
Back everything up before starting!

From Al Nyveldt's original post:

BlogEngine.NET Provider Migration
If you are interested in switching your BlogEngine.NET backend, this page has the tool and instructions to help you. This process 
can help you change from XML to a database (VistaDB, SQLite, SQL Server, or MySQL). It should help you go from one of these fine 
databases, back to XML or from one database to another as well.

The process is simple. You configure the new provider you'd like to use. The tool will copy most of your data from your current 
provider to another provider you select. Once it is finished, you change your default provider in your web.config. The process will 
leave your current data in place after it makes the copy so you will still have it in case something isn't right.

You might have noticed I wrote most of your blog data above. This is because the tool only transfers your blog posts, pages, settings, 
and ping services. Widget data, extension data, and user data is not dealt with. If you are transferring your membership and roles at 
the same time, you'll need to set up your users and passwords again.

Important Notes:

The steps described are for an intermediate level .NET developer. A little BlogEngine experience will help, but is not required. If 
you have no idea what a web.config file or a connection string is, you will most likely not be successful. You've been warned.

This process is not officially supported as part of the BlogEngine.NET project, but is something I've used quite a bit personally. I 
have made a few changes to make it work universally however. It likely could be made a bit easier, but configuration is really the 
only tough part.

1. Backup

We are going to need to make some changes to your web.config file as part of this process, so you'll need to make sure you back that 
up, but I'll go ahead and recommend you back up everything just in case you do something crazy in the middle of the process. If 
something does go horribly wrong, you'll be upset if you skipped this step.

2. Check Version

This process is only for people who are using BlogEngine.NET 1.4.5 or greater. In addition, if you are moving to a database (especially 
SQLite or MySQL) you are strongly recommended to upgrade to 1.4.5.5 or greater or install this simple patch to your 1.4.5.0 blog. (The 
patch is not official either, but has the important DbProvider update in it.)

3. Set up the new provider

This is the tough part of the job. We need to edit your web.config to set up your new BlogProvider. The process varies depending on 
your current data source and your destination data source.

If you are currently using XML and want to move to a database:

This is an easy set up. I'd recommend following the instructions in your Setup folder to prep your blog to run on the database of your 
choice. When you get it set up, you will see the welcome post. Then, you can go back to the web.config and change the defaultProviders 
back to the XML versions. (This would be the blogProvider near the top, and the membership and role Providers near the end of your 
web.config.) This will bring your blog back to where it was before you made the change and now you have your DbBlogProvider set up to 
the database you want to use.

If you are currently using a database and want to move back to XML:

You are extremely lucky, because you can skip this step.

If you are currently using a database and want to move to a different one:

This is the most painful choice, but not too bad, especially if you understand the basics of the Provider model. We are simply need to 
make a second DbBlogProvider entry with a different name. It will point to a different connection string which will point it to a 
different data source. In addition, if you are not just changing SQL Server databases, you'll need to add the appropriate web.config 
entries to configure BlogEngine to work with the new database. You'll also need to install the needed dll file in your bin folder. You 
will find more information on this in the Setup folder of your blog.

&lt;BlogEngine&gt;<br>
&nbsp;&nbsp;&nbsp;&lt;blogProvider defaultProvider="XmlBlogProvider"&gt;<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&lt;providers&gt;<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&lt;add name="XmlBlogProvider" type="BlogEngine.Core.Providers.XmlBlogProvider, BlogEngine.Core"/&gt;<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&lt;add name="DbBlogProvider" type="BlogEngine.Core.Providers.DbBlogProvider, BlogEngine.Core" connectionStringName="BlogEngine" /&gt;<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&lt;add name="NewDbBlogProvider" type="BlogEngine.Core.Providers.DbBlogProvider, BlogEngine.Core" connectionStringName="NewConnection" /&gt;<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&lt;/providers&gt;<br>
&nbsp;&nbsp;&nbsp;&lt;/blogProvider&gt;<br>
&lt;/BlogEngine&gt;<br>

I'd recommend changing your defaultProvider to the new one to make sure you have it set up correctly and once you get the welcome post, 
you can change it back to see your blog again.

Also, you should remember that we are focused on the blogProvider in the migration. The membership and role providers are separate. You 
could leave them the way they are or move them as well. If you move them, you will need to set them up and configure as you would when 
you first installed.

4. Clear out your destination

You need to delete the data in your destination Provider otherwise the migration will fail. If you have a database, this means you can 
run a script that looks something like this:

DELETE FROM be_PostCategory;<br>
DELETE FROM be_PostComment;<br>
DELETE FROM be_PostNotify;<br>
DELETE FROM be_PostTag;<br>
DELETE FROM be_Posts;<br>
DELETE FROM be_Categories;<br>
DELETE FROM be_Pages;<br>
DELETE FROM be_Settings;<br>
DELETE FROM be_PingService;<br>

If you are moving data into XML, you'll want to clear out your App_Data\Posts folder and your App_Data\Pages folder.
Be sure not to delete your existing blog data by accident.

5. Install the BlogMigration page

Download the BlogMigration zip file, extract the 2 files included and copy them to the admin\Pages folder of your blog.

6. Open the BlogMigration page.

You can now surf out to your blog, login, and then go to BlogMigration page by typing in the appropriate address.

7. Run it

Select your destination BlogProvider from the dropdown and click the button.

This may take a few minutes, but you'll get a notification on the screen when it is done.

8. Change your default BlogProvider

If you got through the above steps, you can now open your web.config file and change the BlogProvider section defaultProvider to the 
destination Provider. Browsing out to the site should bring it up using the new provider.

You can optionally remove your old data if you'd like after you are sure everything is working properly.

9. Update your users, extension, and widget data

As mentioned above, you'll need to set your users back up and change passwords. Also any extension and widget data is lost and will 
need to be re-entered. Other than that, you are done.

Troubleshooting

I've run across an occasional problem with the tool in the many times I've used and they are have always revolved around three things.

1. Make sure your new destination provider works. If you can't switch your defaultProvider to the destination and see the default page, 
this will not work.

2. Make sure you clear out the data in the destination before you run it. If you try to add a duplicate record in a table, it will fail.

3. I have had a few occasions where a data field in my destination database wasn't big enough to hold the incoming item. In these 
situation, you simply need to expand the field size, clear the destination, and run it again.

USE AT YOUR OWN RISK!<br>
Back everything up before starting!
